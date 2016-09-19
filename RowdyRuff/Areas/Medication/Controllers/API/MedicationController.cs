using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RowdyRuff.Areas.Medication.DTO;
using RowdyRuff.Areas.Medication.Services;
using RowdyRuff.Settings;

namespace RowdyRuff.Areas.Medication.Controllers.API
{
    [Area("Medication")]
    [Route("/medication/api/{patientUuid}")]
    [Produces("application/json")]
    public class MedicationController : Controller
    {
        private readonly BahmniConnection _bahmniSettings;

        public MedicationController(IOptions<BahmniConnection> settings)
        {
            _bahmniSettings = settings.Value;
        }

        [HttpGet]
        [Produces(typeof(IEnumerable<GetDrugOrderDTO>))]
        public async Task<IActionResult> Index(string patientUuid)
        {
            patientUuid = "31e164fa-dd5b-4a10-a487-33e3a97e6198";
            var drugOrders = await GetDrugOrdersService.GetDrugOrdersForPatient(patientUuid, _bahmniSettings);
            return Json(drugOrders.Select(c => new GetDrugOrderDTO()
            {
                DrugName = c.DrugName,
                Dose = c.Dose,
                DoseUnit = c.DoseUnit,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Frequency = c.Frequency
            }));
        }
    }
}
