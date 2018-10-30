using System;
using System.Threading.Tasks;
using XOProject.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;

namespace XOProject.Tests
{
    public class ShareControllerTests : ControllerTestBase
    {

        private readonly ShareController _shareController;

        public ShareControllerTests() : base()
        {
            _shareController = new ShareController(_shareRepository);
        }

        #region Test Post

        [Test]
        public async Task Post_ShouldInsertHourlySharePrice()
        {
            // Arrange
            var hourRate = new HourlyShareRate
            {
                Symbol = "CBI",
                Rate = 330.0M,
                TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
            };
            _shareController.ModelState.Clear();

            // Act
            var result = await _shareController.Post(hourRate);

            // Assert
            Assert.IsTrue(result is CreatedResult);
        }

        [Test]
        public async Task Post_ShouldInsertFailedHourlySharePriceWhenSymbolMissingAsync()
        {
            // Arrange
            var hourRate = new HourlyShareRate
            {
                Rate = 330.0M,
                TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
            };
            _shareController.ModelState.AddModelError("Symbol", "Required");
            // Act
            var result = await _shareController.Post(hourRate);

            // Assert
            Assert.IsTrue(result is BadRequestObjectResult);
        }

        #endregion

        #region Get


        [Test]
        public async Task Get_ShouldReturnsHourlyShareWhenExists()
        {
            // Arrange
            var hourRateSymbol = "CBI";
            var hourRate = new HourlyShareRate
            {
                Symbol = "CBI",
                Rate = 330.0M,
                TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
            };
            await _shareRepository.InsertAsync(hourRate);

            // Act
            var result = await _shareController.Get(hourRateSymbol);

            // Assert
            Assert.IsTrue(result is OkObjectResult);
        }

        [Test]
        public async Task Get_ShouldReturnsHourlyShareWhenNotExists()
        {
            // Arrange
            var hourRateSymbol = "NA";

            // Act
            var result = await _shareController.Get(hourRateSymbol);

            // Assert
            Assert.IsTrue(result is BadRequestResult);
        }

        #endregion

    }
}
