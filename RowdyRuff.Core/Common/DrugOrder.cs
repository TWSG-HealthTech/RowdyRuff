using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RowdyRuff.Core.Common
{
    public class DrugOrder
    {
        public string PatientUuid { get; private set; }
        public string DrugName { get; private set; }
        public string Dose { get; private set; }
        public string DoseUnit { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public string Frequency { get; private set; }

        public DrugOrder(string patientUuid, string drugName, string dose, string doseUnit, DateTime startDate, DateTime endDate, string frequency)
        {
            PatientUuid = patientUuid;
            DrugName = drugName;
            Dose = dose;
            DoseUnit = doseUnit;
            StartDate = startDate;
            EndDate = endDate;
            Frequency = frequency;
        }

        private DrugOrder() { }
    }
}
