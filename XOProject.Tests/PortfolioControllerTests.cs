using System;
using System.Threading.Tasks;
using XOProject.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;
using Microsoft.EntityFrameworkCore;

namespace XOProject.Tests
{
    public class PortfolioControllerTests : ControllerTestBase
    {

        private readonly PortfolioController _portfolioController;

        public PortfolioControllerTests()
        {

            _portfolioController = new PortfolioController(_shareRepository, _tradeRepository, _portfolioRepository);
        }

        #region Get

        [Test]
        public async Task Get_ShouldReturnsOkResultObjectWhenPortfolioInfoExists()
        {

            // Arrange
            int portFolioid = 1;
            var portfolio = new Portfolio
            {
                Id = 1,
                Name = "John",
            };
            await _portfolioRepository.InsertAsync(portfolio);

            // Act
            var result = await _portfolioController.GetPortfolioInfo(portFolioid);

            // Assert
            Assert.IsTrue(result is OkObjectResult);
        }

        [Test]
        public async Task Get_ShouldReturnsBadResultObjectWhenPortfolioInfoNotExists()
        {

            // Arrange
            int portFolioid = 1;

            // Act
            var result = await _portfolioController.GetPortfolioInfo(portFolioid);

            // Assert
            Assert.IsTrue(result is BadRequestObjectResult);
        }

        #endregion

        #region Post

        [Test]
        public async Task Post_ShouldReturnsCreatedRequestWhenPortofolioIsValid()
        {

            // Arrange
            _portfolioController.ModelState.Remove("Name");
            var portfolio = new Portfolio
            {
                Id = 10,
                Name = "Kim",
            };
            _portfolioController.ModelState.Clear();

            // Act
            var result = await _portfolioController.Post(portfolio);

            // Assert
            Assert.IsTrue(result is CreatedResult);
        }

        [Test]
        public async Task Post_ShouldReturnsBadRequestWhenPortofolioIsValid()
        {
            // Arrange
            var portfolio = new Portfolio
            {
                Id = 1,

            };
            _portfolioController.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _portfolioController.Post(portfolio);

            // Assert
            Assert.IsTrue(result is BadRequestObjectResult);
        }

        #endregion

    }
}
