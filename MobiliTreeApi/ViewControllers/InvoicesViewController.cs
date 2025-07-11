using Microsoft.AspNetCore.Mvc;
using MobiliTreeApi.Services;

namespace MobiliTreeApi.Controllers
{
    public class InvoicesViewController : Controller
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesViewController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        // Toate facturile pentru o parcare (ex: pf001)
        public IActionResult Index(string parkingFacilityId = "pf001")
        {
            var invoices = _invoiceService.GetInvoices(parkingFacilityId);
            return View(invoices); // va trimite lista în Index.cshtml
        }

        // Factura unui client specific într-o parcare
        public IActionResult Details(string parkingFacilityId, string customerId)
        {
            var invoice = _invoiceService.GetInvoice(parkingFacilityId, customerId);
            if (invoice == null)
                return NotFound();

            return View(invoice); // va trimite obiectul în Details.cshtml
        }
    }
}
