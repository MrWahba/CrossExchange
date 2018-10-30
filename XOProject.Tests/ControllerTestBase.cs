using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace XOProject.Tests
{
    public class ControllerTestBase : ControllerBase
    {
        protected ExchangeContext _exchangeContext;
        protected readonly IPortfolioRepository _portfolioRepository;
        protected readonly ITradeRepository _tradeRepository;
        protected readonly IShareRepository _shareRepository;

        public ControllerTestBase()
        {
            var options = new DbContextOptionsBuilder<ExchangeContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;
            _exchangeContext = new ExchangeContext(options);
            _shareRepository = new ShareRepository(_exchangeContext);
            _tradeRepository = new TradeRepository(_exchangeContext);
            _portfolioRepository = new PortfolioRepository(_exchangeContext);

        }
    }
}