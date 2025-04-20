using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _token;
        private readonly SignInManager<AppUser> _signinManager;
        private readonly ApplicationDBContext _context;
        public AccountRepository(UserManager<AppUser> userManager, ITokenService token, SignInManager<AppUser> signInManager, ApplicationDBContext context)
        {
            _userManager = userManager;
            _token = token;
            _signinManager = signInManager;
            _context = context;
        }

        public async Task<APIAccounResult<NewUserDto>> LoginAsync(LoginDto loginDto)
        {
            var result = new APIAccounResult<NewUserDto>{
                statusCode  = 200,
                isSuccess = true
            };
            try{
                var user = await _userManager.Users
                                            .FirstOrDefaultAsync(u => u.UserName == loginDto.UserName);
                if(user == null){ 
                    result.Message = "Invalid Username";
                    return result;
                }

                var validationResult = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if(!validationResult.Succeeded){ 
                    result.Message = "Incorrect Password";
                    return result;
                }
                result.Payload = new NewUserDto{
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _token.CreateToken(user)
                };


            }catch(Exception ex){
                result.statusCode = 500;
                result.Message = ex.Message;
                result.isSuccess = false;
            }
            return result;
        }

        public async Task<APIAccounResult<NewUserDto>> RegisterAsync(RegisterDto registerDto)
        {
            var result = new APIAccounResult<NewUserDto>{
                statusCode = 200,
                isSuccess = true,
            };
            try{
              
                var user = new AppUser{
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };
                var existingUser = await _context.Users
                                                .Select(e => new { e.UserName, e.Email })
                                                .Where(s=> s.UserName == registerDto.Username || s.Email == registerDto.Email)
                                                .FirstOrDefaultAsync();
                if(existingUser == null){
    
                    var createdUser = await _userManager.CreateAsync(user, registerDto.Password);

                    if(createdUser.Succeeded){
                        var roleResult = await _userManager.AddToRoleAsync(user, "User");
                        if(roleResult.Succeeded){
                            var newAcc = new NewUserDto{    
                                Username = user.UserName,
                                Email = user.Email,
                                Token  = _token.CreateToken(user)
                            };         
                            result.Payload = newAcc;   

                        }else{
                            result.statusCode = 500;
                            result.Error = roleResult.Errors.ToList();
                            result.isSuccess = false;

                        }
                    }else{
                        result.statusCode = 500;
                        result.Error = createdUser.Errors.ToList();
                        result.isSuccess = false;
                    }
                }else{
                    result.isSuccess = false;
                    result.Message = "Email or Username already exist";
                }


                


            }catch(Exception ex){
                result.statusCode = 500;
                result.Message = ex.Message;
                result.isSuccess = false;
            }
            return result;
        }
    }
}