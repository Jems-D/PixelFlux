using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Dtos.Stocks;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _repo;
        public AccountController(IAccountRepository repo)
        {
            _repo = repo;         
        }

        [HttpPost("register")] //static routing used for registering and login
        public async Task<IActionResult> RegisterAccount([FromBody] RegisterDto register){
            try{
                if(!ModelState.IsValid) return BadRequest(ModelState);

                var user = await _repo.RegisterAsync(register);
                var createdUser = user.FromApiToNewUser();
                if(createdUser == null){
                    return StatusCode(500, user.Error == null ? user.Message : user.Error); 
                }
                return Ok(createdUser);
            }catch(Exception ex){
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            try{
              var user = await _repo.LoginAsync(login);
              var userAcc = user.FromApiToNewUser();
              if(userAcc == null){
                return Unauthorized(user.Message);
              }
              return Ok(userAcc);

            }catch(Exception ex){
                return StatusCode(500, ex.Message);
            }
        }
        
    }
}