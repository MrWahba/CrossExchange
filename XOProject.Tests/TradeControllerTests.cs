using System;
using System.Threading.Tasks;
using XOProject.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;

namespace XOProject.Tests
{
    public class TradeControllerTests : ControllerTestBase
    {

        private readonly TradeController _tradeController;

        public TradeControllerTests() : base()
        {
            _tradeController = new TradeController(_shareRepository, _tradeRepository, _portfolioRepository);
        }

        #region Test GetAllTradings

        [Test]
        public async Task GetAllTradings_ShouldReturnsAllPortfolioTradesWhenIdExists()
        {
            // Arrange
            int portFolioid = 10;

            await _tradeRepository.InsertAsync(new Trade
            {
                Id = 1,
                NoOfShares = 50,
                Action = "BUY",
                Price = 5000.0M,
                Symbol = "REL",
                PortfolioId = 10
            });


            // Act
            var result = _tradeController.GetAllTradings(portFolioid);

            // Assert
            Assert.IsTrue(result is OkObjectResult);
        }

        [Test]
        public async Task GetAllTradings_ShouldReturnsBadRequestWhenIdNotExists()
        {
            // Arrange
            int portFolioid = 20;

            // Act
            var result = _tradeController.GetAllTradings(portFolioid);

            // Assert
            Assert.IsTrue(result is BadRequestResult);
        }

        #endregion

        #region Get


        [Test]
        public async Task Get_ShouldReturnsHourlyShareWhenExists()
        {
            // Arrange
            string _symbol = "REL";
            await _tradeRepository.InsertAsync(new Trade
            {
                Id = 1,
                NoOfShares = 50,
                Action = "BUY",
                Price = 530.0M,
                Symbol = "REL",
                PortfolioId = 10
            });
            await _tradeRepository.InsertAsync(new Trade
            {
                Id = 2,
                NoOfShares = 30,
                Action = "SELL",
                Price = 400.0M,
                Symbol = "REL",
                PortfolioId = 10
            });

            await _tradeRepository.InsertAsync(new Trade
            {
                Id = 3,
                NoOfShares = 100,
                Action = "BUY",
                Price = 200.0M,
                Symbol = "REL",
                PortfolioId = 10
            });
            await _tradeRepository.InsertAsync(new Trade
            {
                Id = 4,
                NoOfShares = 50,
                Action = "SELL",
                Price = 100.0M,
                Symbol = "REL",
                PortfolioId = 10
            });

            // Act
            var result = _tradeController.GetAnalysis(_symbol);

            // Assert
            Assert.IsTrue(result is OkObjectResult);
        }

        [Test]
        public async Task Get_ShouldReturnsHourlyShareWhenNotExists()
        {
            // Arrange
            string _symbol = "NA";

            // Act
            var result = _tradeController.GetAnalysis(_symbol);

            // Assert
            Assert.IsTrue(result is BadRequestResult);
        }

        #endregion

    }
}
