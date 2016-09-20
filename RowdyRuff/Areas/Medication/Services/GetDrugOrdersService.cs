using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using RowdyRuff.Common.Gateway;
using RowdyRuff.Core.Common;
using RowdyRuff.Settings;

namespace RowdyRuff.Areas.Medication.Services
{
    public class GetDrugOrdersService : IGetDrugOrdersService
    {
        async Task<List<DrugOrder>> IGetDrugOrdersService.GetDrugOrdersForPatient(string patientUuid, BahmniConnection bahmniSettings)
        {
            string path =
              "https://bahmni-sg-dev.click/openmrs/ws/rest/v1/bahmnicore/drugOrders/prescribedAndActive?getEffectiveOrdersOnly=false&getOtherActive=true&numberOfVisits=10&patientUuid=" + patientUuid;
            IServerGateway gateway = new ServerGatewayBase();
            Debug.WriteLine(path);
            JObject drugOrders = await gateway.GetAsyncWithBasicAuth<JObject>(path, bahmniSettings.Username, bahmniSettings.Password);
            return ParseDrugOrderResultsData(drugOrders, patientUuid);
        }

        private List<DrugOrder> ParseDrugOrderResultsData(JObject input, string patientUuid)
        {
            List<DrugOrder> drugOrders = new List<DrugOrder>();
            foreach (var drugOrderInput in input.GetValue("visitDrugOrders"))
            {
                DrugOrder drugOrder = new DrugOrder(
                    patientUuid, 
                    drugOrderInput.GetValue<JObject>("concept").GetValue("name").ToString(),
                    drugOrderInput.GetValue<JObject>("dosingInstructions").GetValue("dose").ToString(),
                    drugOrderInput.GetValue<JObject>("dosingInstructions").GetValue("doseUnits").ToString(),
                    convertTime(drugOrderInput.GetValue<long>("effectiveStartDate")),
                    convertTime(drugOrderInput.GetValue<long>("effectiveStopDate")),
                    "3");
                drugOrders.Add(drugOrder);
            }
            return drugOrders;
        }

        private DateTime convertTime(long timeInMillionseconds)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(timeInMillionseconds);
        }
    }
}
