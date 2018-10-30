using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace XOProject.Controller
{
    [Route("api/Trade/")]
    public class TradeController : ControllerBase
    {
        private IShareRepository _shareRepository { get; set; }
        private ITradeRepository _tradeRepository { get; set; }
        private IPortfolioRepository _portfolioRepository { get; set; }

        public TradeController(IShareRepository shareRepository, ITradeRepository tradeRepository, IPortfolioRepository portfolioRepository)
        {
            _shareRepository = shareRepository;
            _tradeRepository = tradeRepository;
            _portfolioRepository = portfolioRepository;
        }


        [HttpGet("{portfolioid}")]
        public IActionResult GetAllTradings([FromRoute]int portFolioid)
        {
            var trades = _tradeRepository.Query().Where(x => x.PortfolioId.Equals(portFolioid));

            if (trades.Count() > 0)
                return Ok(trades);

            return BadRequest();
        }


        /// <summary>
        /// For a given symbol of share, get the statistics for that particular share calculating the maximum, minimum, average and Sum of all the trades that happened for that share. 
        /// Group statistics individually for all BUY trades and SELL trades separately.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>

        [HttpGet("Analysis/{symbol}")]
        public IActionResult GetAnalysis([FromRoute]string symbol)
        {
            var list = new List<TradeAnalysis>();

            var res = _tradeRepository.Query().Where(t => t.Symbol == symbol).ToArray();
            var buyTrades = res.Where(t => t.Action == "BUY");
            var sellTrades = res.Where(t => t.Action == "SELL");

            if (buyTrades.Count() > 0)
            {
                list.Add(new TradeAnalysis
                {
                    Action = "BUY",
                    Average = buyTrades.Average(t => t.Price),
                    Maximum = buyTrades.Max(t => t.Price),
                    Minimum = buyTrades.Min(t => t.Price),
                    Sum = buyTrades.Sum(t => t.Price)
                });
            }

            if (sellTrades.Count() > 0)
            {
                list.Add(new TradeAnalysis
                {
                    Action = "SELL",
                    Average = sellTrades.Average(t => t.Price),
                    Maximum = sellTrades.Max(t => t.Price),
                    Minimum = sellTrades.Min(t => t.Price),
                    Sum = sellTrades.Sum(t => t.Price)
                });
            }

            if (list.Count() > 0)
            {
                return Ok(list);
            }

            return BadRequest();
        }


    }
}
