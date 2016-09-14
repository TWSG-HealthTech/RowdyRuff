using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace RowdyRuff.Areas.Medication.DTO
{
    public class GetPrescriptionDTO
    {
        public List<String> DrugOrders { get; set; }
    }
}
