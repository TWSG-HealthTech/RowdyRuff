using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RowdyRuff.Areas.Medication.DTO;
using RowdyRuff.Core.Common;

namespace RowdyRuff.Areas.Medication.Controllers.API
{
    [Area("Medication")]
    [Route("/medication/api/{patientUuid}")]
    [Produces("application/json")]
    public class MedicationController : Controller
    {

        [HttpGet]
        [Produces(typeof(IEnumerable<GetPrescriptionDTO>))]
        public IActionResult Index(string profileId)
        {
            return Json(new GetPrescriptionDTO
            {
                DrugOrders = new List<string>()
                {
                    "abc"
                }
            });
        }
    }
}
