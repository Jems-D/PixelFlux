using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stocks;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stocks")]
    [ApiController]
    [Authorize]
    public class StocksController : ControllerBase
    {
        private readonly IStockRepository _repo;

        public StocksController(IStockRepository stockRepo, ApplicationDBContext context)
        {
            _repo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query){

             if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var stocks = await _repo.GetAllAsync(query);
          /*  var stocks = (await _repo.GetAllAsync(query))
                        .Select(s => s.ToStockDto()); */
            return Ok(stocks.FromAPIToListStocks());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id){
             if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await _repo.GetOneByIdAsync(id);

              if(stock.isSuccess == false){
                return NotFound("No stock found");
            } 

            var stocknl = new APIResult<Stocks>{
                statusCode = stock.statusCode,
                Message = stock.Message,
                isSuccess = stock.isSuccess,
                Payload = stock.Payload
            }; 

            return Ok(stocknl.TransformToStockDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDTO stockDto){
            
             if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var stock = stockDto.ToStockFromCreateDto();
            await _repo.CreateStockAsync(stock);

            return CreatedAtAction(nameof(GetById), new { id = stock.Id} , stock.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromBody] UpdateStockRequestDTO updateDto, [FromRoute] int id){
            
             if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var stockModel = await _repo.UpdateStockAsync(id, updateDto);
            if (stockModel == null){
                return NotFound();
            }

            return Ok(stockModel.TransformFromUpdate());
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id){
            
             if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var stock = await _repo.DeleteStockAsync(id);
            if(stock == null){
                return NotFound();
            }

             return NoContent();


        }
    }
}