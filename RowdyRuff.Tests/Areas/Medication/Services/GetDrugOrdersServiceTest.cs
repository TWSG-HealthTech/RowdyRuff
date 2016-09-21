using System.Threading.Tasks;
using Machine.Specifications;
using Newtonsoft.Json.Linq;
using RowdyRuff.Areas.Medication.Services;
using RowdyRuff.Common.Gateway;
using RowdyRuff.Settings;
using Moq;
using It = Machine.Specifications.It;

namespace RowdyRuff.Tests.Areas.Medication.Services
{
    class GetDrugOrdersServiceTest
    {
        [Subject(typeof(GetDrugOrdersService))]
        public class When_get_drug_orders_for_patient
        {
            private static Mock<IServerGateway> _gatewayMock;
            private static BahmniConnection _bahmniConnection = new BahmniConnection();
            private static IGetDrugOrdersService _service;
            private static JObject _drugOrders;
            
            Establish context = () =>
            {
                string json = @"{
                    visitDrugOrders: [{ 
                        concept: {name: 'new drug'},
                        dosingInstructions: {
                            dose: '2', 
                            doseUnits: 'ml'},
                        effectiveStartDate: '1474355262000',
                        effectiveStopDate: '1474700861000'
                    }]
                }";
                
                _drugOrders = JObject.Parse(json);
                _gatewayMock = new Mock<IServerGateway>();
                _gatewayMock.Setup(g => g.GetAsyncWithBasicAuth<JObject>(
                    Moq.It.IsAny<string>(), Moq.It.IsAny<string>(), Moq.It.IsAny<string>()))
                    .Returns(Task.FromResult(_drugOrders));

                _service = new GetDrugOrdersService(_gatewayMock.Object);
                _bahmniConnection.Username = "username";
                _bahmniConnection.Password = "password";
            };
            
            static string path =
                "https://bahmni-sg-dev.click/openmrs/ws/rest/v1/bahmnicore/drugOrders/prescribedAndActive?getEffectiveOrdersOnly=false&getOtherActive=true&numberOfVisits=10&patientUuid=patientUuid";
                        
            private Because of = () => _service.GetDrugOrdersForPatient("patientUuid", _bahmniConnection).Wait();
            
            It should_invoke =
                () => _gatewayMock.Verify(g => g.GetAsyncWithBasicAuth<JObject>(path, "username", "password"));

        }


    }
}
