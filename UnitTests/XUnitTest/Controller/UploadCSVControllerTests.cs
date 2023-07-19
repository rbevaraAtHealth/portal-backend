using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Controllers;
using Moq;
using XUnitTest.MockData;

namespace XUnitTest.Controller
{
    public class UploadCSVControllerTests
    {
        private readonly Mock<IUploadCSV> _Upload;
        public UploadCSVControllerTests()
        {
            _Upload = new Mock<IUploadCSV>();
        }

        [Fact]
        public async Task UploadCSVController_SholudCallUploadCSVAsyncOnce()
        {
            //Arrange
            var newUpload = UploadCSVMockData.UploadData();
            var controller = new UploadCSVController(_Upload.Object);
            //Act
            var result = await controller.UploadCsv(newUpload);
            //Assert
            _Upload.Verify(c => c.GetUploadCSVAsync(newUpload),Times.Exactly(1));
        }
    }
}
