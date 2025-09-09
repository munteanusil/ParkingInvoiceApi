using System;
using System.Collections.Generic;
using System.Linq;
using ParkingApi.Domain;
using ParkingApi.Repositories;

namespace ParkingApi.Services
{
    public interface IInvoiceService
    {
        List<Invoice> GetInvoices(string parkingFacilityId);
        Invoice GetInvoice(string parkingFacilityId, string customerId);
    }

    public class InvoiceService: IInvoiceService
    {
        private readonly ISessionsRepository _sessionsRepository;
        private readonly IParkingFacilityRepository _parkingFacilityRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ISessionPricingService _pricingService;

        public InvoiceService(ISessionsRepository sessionsRepository, IParkingFacilityRepository parkingFacilityRepository, ICustomerRepository customerRepository, ISessionPricingService pricingService)
        {
            _sessionsRepository = sessionsRepository;
            _parkingFacilityRepository = parkingFacilityRepository;
            _customerRepository = customerRepository;
            _pricingService = pricingService;
        }

        public Invoice GetInvoice(string parkingFacilityId, string customerId)
        {
            var serviceProfile = _parkingFacilityRepository.GetServiceProfile(parkingFacilityId);
            if (serviceProfile == null)
                throw new ArgumentException($"Invalid parking facility id '{parkingFacilityId}'");

            var sessions = _sessionsRepository.GetSessions(parkingFacilityId)
                .Where(s => s.CustomerId == customerId)
                .ToList();

            if (sessions.Count == 0)
                return null;

            var customer = _customerRepository.GetCustomer(customerId);
            var hasContract = customer?.ContractedParkingFacilityIds.Contains(parkingFacilityId) == true;

            var totalAmount = sessions.Sum(session =>
                _pricingService.CalculateCost(session, hasContract));

            return new Invoice
            {
                ParkingFacilityId = parkingFacilityId,
                CustomerId = customerId,
                Amount = totalAmount
            };
        }

        public List<Invoice> GetInvoices(string parkingFacilityId)
        {
            var serviceProfile = _parkingFacilityRepository.GetServiceProfile(parkingFacilityId);
            if (serviceProfile == null)
                throw new ArgumentException($"Invalid parking facility id '{parkingFacilityId}'");

            var sessions = _sessionsRepository.GetSessions(parkingFacilityId);
            if (sessions == null || sessions.Count == 0)
                return new List<Invoice>();

            var groupedSessions = sessions.GroupBy(s => s.CustomerId);
            var invoices = new List<Invoice>();

            foreach (var group in groupedSessions)
            {
                var customerId = group.Key;
                var customer = _customerRepository.GetCustomer(customerId);
                var hasContract = customer?.ContractedParkingFacilityIds.Contains(parkingFacilityId) == true;

                var totalAmount = group.Sum(session =>
                    _pricingService.CalculateCost(session, hasContract));

                invoices.Add(new Invoice
                {
                    ParkingFacilityId = parkingFacilityId,
                    CustomerId = customerId,
                    Amount = totalAmount
                });
            }

            return invoices;
        }






    }
}
