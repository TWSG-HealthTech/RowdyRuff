using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RowdyRuff.Core.Common;
using RowdyRuff.Settings;

namespace RowdyRuff.Areas.Medication.Services
{
    public interface IGetDrugOrdersService
    {
        Task<List<DrugOrder>> GetDrugOrdersForPatient(string patientUuid, BahmniConnection bahmniSettings);
    }
}
