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
        public IActionResult Index(string patientUuid)
        {
            var connections = new List<Prescription>();
            connections.Add(new Prescription("a0123456", "Drug A", "2", "Tablet(s)", convertTime(1472008425000), convertTime(1472181224000), "3"));
            connections.Add(new Prescription("a0123456", "Drug B", "1", "IU", convertTime(1471944220000), convertTime(1472203419000), "1"));
            return Json(connections.Select(c => new GetPrescriptionDTO()
            {
                DrugName = c.DrugName,
                Dose = c.Dose,
                DoseUnit = c.DoseUnit,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Frequency = c.Frequency
            }));
        }

        private DateTime convertTime(long timeInMillionseconds)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(timeInMillionseconds);
        }
    }
}
