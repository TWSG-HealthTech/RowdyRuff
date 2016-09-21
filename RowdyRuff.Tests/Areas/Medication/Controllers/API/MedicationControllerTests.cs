using System.Collections.Generic;
using System.Diagnostics;
using Machine.Specifications;
using Microsoft.Extensions.Options;
using RowdyRuff.Areas.Medication.Controllers.API;
using RowdyRuff.Areas.Medication.Services;
using RowdyRuff.Core.Common;
using RowdyRuff.Settings;
using Moq;
using It = Machine.Specifications.It;

namespace RowdyRuff.Tests.Areas.Medication.Controllers.API
{
    class MedicationControllerTests
    {
        [Subject(typeof(MedicationController))]
        public class When_get_medication_by_patient_uuid
        {
          
             Establish context = () =>
            {
                _getDrugOrdersServiceMock = new Mock<IGetDrugOrdersService>();
                _settings = new Mock<IOptions<BahmniConnection>>();
                _getDrugOrdersServiceMock.Setup(p => p.GetDrugOrdersForPatient(Moq.It.IsAny<string>(), _settings.Object.Value)).ReturnsAsync(new List<DrugOrder>());
                
                _subject = new MedicationController(_settings.Object, _getDrugOrdersServiceMock.Object);

            };

            private static Mock<IGetDrugOrdersService> _getDrugOrdersServiceMock;
            private static Mock<IOptions<BahmniConnection>> _settings;
            private static MedicationController _subject;

            private Because of = async () => await _subject.Index("abcd");
            It should_call_drug_orders_service = () => _getDrugOrdersServiceMock.Verify(service => service.GetDrugOrdersForPatient("abcd", _settings.Object.Value));
            
        }
    }
}
