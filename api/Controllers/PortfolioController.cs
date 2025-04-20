using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    [Authorize]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _appuser;
        private readonly IStockRepository _repo;
        private readonly IPortfolioRepository _repoPortfolio;
        private readonly IFMPService _fmp;
        public PortfolioController(UserManager<AppUser> appuser, IStockRepository stocRepo, IPortfolioRepository portfolioRepository, IFMPService service)
        {
            _appuser = appuser;
            _repo = stocRepo;
            _repoPortfolio = portfolioRepository;
            _fmp = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPortfolio( )
        {
            var user = User.GetUsername();    
            var appuser = await _appuser.FindByNameAsync(user);
            var portfolio = await _repoPortfolio.GetUserPortfolio(appuser);
            return Ok(portfolio);
        }

        [HttpPost]
        public async Task<IActionResult> AddPortfolio(string Symbol)
        {
            var user = User.GetUsername();
            var appuser = await _appuser.FindByNameAsync(user);
            var stock = await _repo.GetBySymbol(Symbol);

            if(stock == null){
                stock = await _fmp.FindStockBySymbolAsync(Symbol);
                if(stock == null){
                    return BadRequest("This stock does not exist");
                }else{
                    await _repo.CreateStockAsync(stock);
                } 
            }



            if(stock == null) return BadRequest("Not Found"); 
            var GetUserPortfolio = await _repoPortfolio.GetUserPortfolio(appuser);
            if(GetUserPortfolio.Any(s => s.Symbol.ToLower() == Symbol.ToLower())) return BadRequest("Portfolio already exist");

            var portfolio = new Portfolio{
                StockId = stock.Id,
                AppUserId = appuser.Id 
            };
            await _repoPortfolio.CreatePortfolio(portfolio);
            if(portfolio == null)
            {
                return StatusCode(500, "Could not create");
            }
            else
            {
                return Created();
            }


        }


        [HttpDelete]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var user = User.GetUsername();
            var appuser = await _appuser.FindByNameAsync(user);

            var GetUserPortfolio = await _repoPortfolio.GetUserPortfolio(appuser);

            var filteredStock =  GetUserPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower());

            if(filteredStock.Count() == 1)
            {
                await _repoPortfolio.DeletePortfolio(appuser, symbol);
            }else{
                return BadRequest("Stock not in portfolio");
            }
            return Ok();
        }

    }
}