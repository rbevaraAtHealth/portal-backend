using CodeMatcherReview.Test.MockData;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CodeMatcherReview.Test.Controllers
{
    public class LookUpControllerTests
    {
        private readonly Mock<ILookupTypes> _lookupTypesMock;
        private readonly Mock<ILookUp> _lookUpMock;
        public LookUpControllerTests()
        {
            _lookUpMock = new Mock<ILookUp>();
            _lookupTypesMock = new Mock<ILookupTypes>();
        }

        [Fact]
        public void LookupController_GetLookups_SholudReturn200Status()
        {
            //Arange
            _lookupTypesMock.Setup(x => x.GetLookupByNameAsync("segment")).ReturnsAsync(LookupMockData.LookupTypeMockData("segment"));
            _lookUpMock.Setup(x => x.GetLookupByIdAsync(1)).ReturnsAsync(LookupMockData.LookupData(1));
            var controller = new LookUpController(_lookUpMock.Object, _lookupTypesMock.Object);
            //Act
            var result = controller.GetLookups("segment");
            //Assert
            result.Result.GetType().Should().Be(typeof(OkObjectResult));
            (result.Result as OkObjectResult).StatusCode.Should().Be(200);
        }

    }
}