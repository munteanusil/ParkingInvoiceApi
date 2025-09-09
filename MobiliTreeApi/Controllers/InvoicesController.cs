using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using ParkingApi.Domain;
using ParkingApi.Services;

namespace ParkingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        //Returnează toate facturile generate pentru toți clienții care au avut sesiuni în acea parcare.
        [HttpGet("{parkingFacilityId}")]
        public ActionResult<List<Invoice>> GetInvoices(string parkingFacilityId) 
        {
            try
            {
                var invoices = _invoiceService.GetInvoices(parkingFacilityId);
                return Ok(invoices);
            }
            catch
            {
                return NotFound($"Parcarea cu Id-ul'{parkingFacilityId}' nu a fost gasita!");
            }

        }

        //Returnează o singură factură pentru un anumit client într-o anumită parcare.
        [HttpGet("{parkingFacilityId}/{customerId}")]
        public ActionResult<Invoice> GetInvoice(string parkingFacilityId, string customerId)
        {
            try
            {
                var invoice = _invoiceService.GetInvoice(parkingFacilityId, customerId);
                if (invoice == null)
                    return NotFound("Factura nu a fost găsită.");

                return Ok(invoice);
            }
            catch
            {
                return BadRequest("Eroare în procesarea facturii.");
            }
        }
    }
}
