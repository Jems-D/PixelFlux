using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using api.Dtos.Comment;
using System.Text.Json;
using api.Models;
using System.Runtime.CompilerServices;
using api.Helpers;
using Microsoft.AspNetCore.Authorization;
using api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using api.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _repo;
        private readonly IStockRepository _repoStock;
        private readonly UserManager<AppUser> _appUser;
        private readonly IFMPService _fmp;
        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository, UserManager<AppUser> appuser, IFMPService service)
        {
            _repo = commentRepository;
            _repoStock = stockRepository;
            _appUser = appuser;
            _fmp = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments([FromQuery] CommentQueryObject commentQuery){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var comments = await _repo.GetAllCommentAsync(commentQuery);
            
            //ef core
            //var comment = comments.Select(s=> s.ToCommentDto());

            return Ok(comments.ToCommentDTO());
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetOneComment([FromRoute] int id){
            
             if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var comment = await _repo.GetByIdAsync(id);
            if(comment.isSuccess == false){
                return NotFound("No comment found");
            }

            var commentnl = new APIResult<Comment>{
                statusCode = comment.statusCode,
                Message = comment.Message,
                isSuccess = comment.isSuccess,
                Payload = comment.Payload
            };
            return Ok(commentnl.TransformToCommentDTO());
        }

        [HttpPost("{symbol:alpha}")]
        public async Task<IActionResult> CreateComment([FromRoute]string symbol, [FromBody] CreateCommentDTO commentDto){
            
             if(!ModelState.IsValid)
                return BadRequest(ModelState);

            //var stock = await _repoStock.DoesStockExist(stockId);
           /*  if(stock.Payload == false){
                return BadRequest("Stock does not exist");
            } */
            var stock = await _repoStock.GetBySymbol(symbol);
            if(stock == null){
                stock = await _fmp.FindStockBySymbolAsync(symbol);
                if(stock == null){
                    return BadRequest("This stock does not exist");
                }else{
                    await _repoStock.CreateStockAsync(stock);
                } 
            }


            var user = User.GetUsername();
            var appuser = await _appUser.FindByNameAsync(user);



            var commentModel = commentDto.ToCommentCreateCommentDto(stock.Id);
            commentModel.AppUserId = appuser.Id;
            await _repo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetOneComment), new { id = commentModel.StockId }, commentModel.ToCommentDto());

        }


        [HttpPut]
        [Route("{Id:int}")]  //should be the same var name in the parameter below
        public async Task<IActionResult> UpdateComment([FromRoute] int Id, [FromBody] UpdateCommentRequestDTO commentDto){
            
             if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _repo.UpdateAsync(Id, commentDto.ToCommentFromUpdate());

            if(comment == null){
                return NotFound("Comment not found");
            }

            
            return Ok(comment.TransformAPIComment());
        }

        [HttpDelete]
        [Route("{Id:int}")]
        public async Task<IActionResult> DeleteOneComment([FromRoute] int Id){
            
             if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var existingComment = await _repo.DeleteCommentAsync(Id);

            if(existingComment == null){
                return NotFound("No comment deleted");

            }
            return NoContent();

        }
 
 

    }
}