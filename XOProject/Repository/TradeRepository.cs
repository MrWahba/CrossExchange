namespace XOProject
{
    public class TradeRepository : GenericRepository<Trade>, ITradeRepository
    {
        public TradeRepository()
        {

        }
        public TradeRepository(ExchangeContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}