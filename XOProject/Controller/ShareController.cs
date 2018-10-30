using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace XOProject.Controller
{
    [Route("api/Share")]
    public class ShareController : ControllerBase
    {
        public IShareRepository _shareRepository { get; set; }

        public ShareController(IShareRepository shareRepository)
        {
            _shareRepository = shareRepository;
        }

        [HttpPut("{symbol}")]
        public async Task<IActionResult> UpdateLastPrice([FromRoute]string symbol)
        {
            var shares = await _shareRepository.Query().Where(x => x.Symbol.Equals(symbol)).ToListAsync();
            if (shares.Count > 0)
            {
                var share = shares.OrderByDescending(x => x.Rate).FirstOrDefault();
                HourlyShareRate _newShare = new HourlyShareRate
                {
                    Symbol = share.Symbol,
                    Rate = share.Rate + 10,
                    TimeStamp = DateTime.Now
                };
                await _shareRepository.InsertAsync(_newShare);

                return Ok();
            }

            return BadRequest();
        }


        [HttpGet("{symbol}")]
        public async Task<IActionResult> Get([FromRoute]string symbol)
        {
            var shares = await _shareRepository.Query().Where(x => x.Symbol.Equals(symbol)).ToListAsync();

            if (shares.Count > 0)
                return Ok(shares);

            return BadRequest();
        }


        [HttpGet("{symbol}/Latest")]
        public async Task<IActionResult> GetLatestPrice([FromRoute]string symbol)
        {
            var share = await _shareRepository.Query().Where(x => x.Symbol.Equals(symbol))
                .OrderByDescending(s => s.TimeStamp).FirstOrDefaultAsync();

            if (share != default(HourlyShareRate))
                return Ok(share?.Rate);
            return BadRequest(0);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]HourlyShareRate value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _shareRepository.InsertAsync(value);
            return Created($"Share/{value.Id}", value);
        }

    }
}
