using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RowdyRuff.Common.Gateway;
using RowdyRuff.Core.Common;
using RowdyRuff.Settings;

namespace RowdyRuff.Areas.Medication.Services
{
    public class GetDrugOrdersService : IGetDrugOrdersService
    {
        private IServerGateway _gateway;
        public GetDrugOrdersService(IServerGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<List<DrugOrder>> GetDrugOrdersForPatient(string patientUuid, BahmniConnection bahmniSettings)
        {
            string path =
              "https://bahmni-sg-dev.click/openmrs/ws/rest/v1/bahmnicore/drugOrders/prescribedAndActive?getEffectiveOrdersOnly=false&getOtherActive=true&numberOfVisits=10&patientUuid=" + patientUuid;
            JObject drugOrders = await _gateway.GetAsyncWithBasicAuth<JObject>(path, bahmniSettings.Username, bahmniSettings.Password);
            var dos = ParseDrugOrderResultsData(drugOrders, patientUuid);
            return dos;
        }

        private List<DrugOrder> ParseDrugOrderResultsData(JObject input, string patientUuid)
        {
            List<DrugOrder> drugOrders = new List<DrugOrder>();
            foreach (var drugOrderInput in input.GetValue("visitDrugOrders"))
            {
                DrugOrder drugOrder = new DrugOrder(
                    patientUuid,
                    drugOrderInput["concept"]["name"].ToString(),
                    drugOrderInput["dosingInstructions"]["dose"].ToString(),
                    drugOrderInput["dosingInstructions"]["doseUnits"].ToString(),
                    convertTime(Convert.ToInt64(drugOrderInput["effectiveStartDate"].ToString())),
                    convertTime(Convert.ToInt64(drugOrderInput["effectiveStopDate"].ToString())),
                    "3");
                drugOrders.Add(drugOrder);
            }
            return drugOrders;
        }

        private DateTime convertTime(long timeInMillionseconds)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(timeInMillionseconds).UtcDateTime;
        }
    }
}
