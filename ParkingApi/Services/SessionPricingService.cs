using ParkingApi.Domain;
using ParkingApi.Repositories;
using System;
using System.Linq;

namespace ParkingApi.Services
{

    public  interface ISessionPricingService
    {
        decimal CalculateCost(Session session,  bool hasContract);  
    }

   
    public class SessionPricingService : ISessionPricingService
    {

        private readonly IParkingFacilityRepository _parkingFacilityRepository;

        public SessionPricingService(IParkingFacilityRepository parkingFacilityRepository)
        {
            _parkingFacilityRepository = parkingFacilityRepository;
        }
        public decimal CalculateCost(Session session, bool hasContract)
        {
            if (session.EndDateTime <= session.StartDateTime)
                throw new ArgumentException("EndDateTime must be after StartDateTime");

            var profile = _parkingFacilityRepository.GetServiceProfile(session.ParkingFacilityId);
            if (profile == null)
                throw new ArgumentException($"Unknown parking facility id '{session.ParkingFacilityId}'");

            decimal total = 0m;
            DateTime current = session.StartDateTime;

            while (current < session.EndDateTime)
            {
                bool isWeekend = current.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

                var activePrices = hasContract
                    ? (isWeekend ? profile.WeekendPrices : profile.WeekDaysPrices)
                    : (isWeekend ? profile.OverrunWeekendPrices : profile.OverrunWeekDaysPrices);

                var slot = activePrices.FirstOrDefault(p => current.Hour >= p.StartHour && current.Hour < p.EndHour);
                if (slot == null)
                {
                    current = current.AddMinutes(1); // dacă nu se potrivește cu un slot, avansează
                    continue;
                }

                var endOfSlot = current.Date.AddHours(slot.EndHour);
                var segmentEnd = session.EndDateTime < endOfSlot ? session.EndDateTime : endOfSlot;
                var minutes = (segmentEnd - current).TotalMinutes;

                total += (decimal)(minutes / 60.0) * slot.PricePerHour;

                current = segmentEnd;
            }

            return total;
        }


    }
}
