﻿using System;

namespace RowdyRuff.Areas.Medication.DTO
{
    public class GetDrugOrderDTO
    {
        public string DrugName { get; set; }
        public string Dose { get; set; }
        public string DoseUnit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Frequency { get; set; }
    }
}
