using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using api.Constants;
using api.Dtos.Comment;
using api.Helpers;
using api.Mappers;
using api.Models;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {
            
        }

        public DbSet<Stocks> Stock {get; set;}
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

    #region AppUser
    // for creating new accounts
    //Just for adding  or inserting the new roles, must have a static id and concurrency to properly work
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Portfolio>(s => s.HasKey(p => new { p.AppUserId, p.StockId}));
            builder.Entity<Portfolio>()
                    .HasOne(u => u.AppUser)
                    .WithMany(u => u.Portfolios)
                    .HasForeignKey(p => p.AppUserId);

            
            builder.Entity<Portfolio>()
                    .HasOne(u => u.Stock)
                    .WithMany(u => u.Portfolios)
                    .HasForeignKey(p => p.StockId);



            List<IdentityRole> roles = new List<IdentityRole>{
                new IdentityRole{
                    Id = "1",
                    Name  = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "e1a86b3d-1234-4567-8901-abcdef123456"
                },
                new IdentityRole{
                    Id = "2",
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "f2b97c4e-2345-5678-9012-bcdefa234567"
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

        }
        #endregion

        #region  Comment

        [HttpGet]
        public async Task<APIResult<List<Comment>>> GetAllCommentsAsync(CommentQueryObject queryObject){
            var result = new APIResult<List<Comment>>{
                statusCode = 200,
                Message = "Success",
                isSuccess = true
            };

            try{
                using(var command = Database.GetDbConnection().CreateCommand()) { //creates a db command object to execute agains the db
                command.CommandType = CommandType.StoredProcedure;   //sets the command type to stored procedure
                command.CommandText = PixelFluxConstants.SP_GetAllComments.ToString(); //assign the name of the stored procedure
                command.Parameters.Add(new SqlParameter("@Symbol", queryObject.Symbol)); 
                command.Parameters.Add(new SqlParameter("@IsDescending", queryObject.IsDescending)); 

                await Database.OpenConnectionAsync(); // open the db connection asynchronously to avoid blocking the thread.
                var comments = new List<Comment>();
                using(var reader = await command.ExecuteReaderAsync()){  //executes the sp and retrieved the results set as a dbdatareader

                    while(await reader.ReadAsync()){ //iterate throught each row(use while for a list)
                            var Id = reader.GetOrdinal("Id");
                            var Title = reader.GetOrdinal("Title");
                            var Content = reader.GetOrdinal("Content");
                            var CreatedOn = reader.GetOrdinal("CreatedOn");
                            var StockId = reader.GetOrdinal("StockId");

                        var comment = new Comment{
                            Id = reader.GetInt32(Id),
                            Title = reader.GetString(Title),
                            Content = reader.GetString(Content),
                            CreatedOn = reader.GetDateTime(CreatedOn),
                            StockId = reader.GetInt32(StockId),
                            AppUserId = reader.GetString(reader.GetOrdinal("AppUserId")), 
                        };

                        //comments.Add(comment);
                        if(!await reader.IsDBNullAsync("AppUserId")){
                            var user = new AppUser{
                                UserName = reader.GetString(reader.GetOrdinal("Username"))
                            };
                            comment.AppUser = user;
                        }
                        comments.Add(comment);
                    }

                    result.Payload = comments;
                }

            }

            }catch(Exception ex){
                result.statusCode = 500;
                result.Message = ex.Message;
                result.isSuccess = false;
            }
            return result;
        }


        [HttpGet]
        public async Task<APIResult<Comment>> GetOneCommentAsync(int id){
            var result = new APIResult<Comment>{
                statusCode = 200,
                Message = "Success",
                isSuccess = true
            };

            try{

                using(var command = Database.GetDbConnection().CreateCommand()){
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = PixelFluxConstants.SP_GetOneCommentById.ToString();
                    command.Parameters.Add(new SqlParameter("@Id", id));

                    await Database.OpenConnectionAsync();
                    var comment = new Comment();
                    using(var reader = await command.ExecuteReaderAsync()){
                        if(reader.HasRows){
                            if(await reader.ReadAsync()){
                                comment.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                                comment.Title = reader.GetString(reader.GetOrdinal("Title"));
                                comment.Content = reader.GetString(reader.GetOrdinal("Content"));
                                comment.CreatedOn = reader.GetDateTime(reader.GetOrdinal("CreatedOn"));
                                comment.StockId = reader.GetInt32(reader.GetOrdinal("StockId"));
                                comment.AppUserId = reader.GetString(reader.GetOrdinal("AppUserId"));

                                if(!await reader.IsDBNullAsync(reader.GetOrdinal("AppUserId")))
                                {
                                    var user = new AppUser{
                                        UserName = reader.GetString(reader.GetOrdinal("Username"))
                                    };
                                    comment.AppUser = user;
                                }
               
                                result.Payload = comment;
                            }
                        }else{
                            result.statusCode = 404;
                            result.Message = "No comment found";
                            result.isSuccess = false;
                        }
                       
                    }
                }


            }catch(Exception ex){
                result.statusCode = 500;
                result.Message = ex.Message;
                result.isSuccess = false;
            }
            return result;
        }


        public async Task<APIResult<int>> AddOneCommentAsnyc(Comment comment){
            var result = new APIResult<int>{
                statusCode = 200,
                Message = "Success",
                isSuccess = true
            };

            try{
                using(var command = Database.GetDbConnection().CreateCommand()){
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = PixelFluxConstants.SP_AddOneComment.ToString();
                    command.Parameters.Add(new SqlParameter("@Title", comment.Title));
                    command.Parameters.Add(new SqlParameter("@Content", comment.Content));
                    command.Parameters.Add(new SqlParameter("@CreatedOn", comment.CreatedOn));
                    command.Parameters.Add(new SqlParameter("@StockId", comment.StockId));

                    await Database.OpenConnectionAsync();

                    using(var reader = await command.ExecuteReaderAsync()){
                        if(await reader.ReadAsync()){
                            result.Payload = reader.GetInt32(0);
                        }else{
                            result.statusCode = 500;
                            result.isSuccess = false;
                            result.Message = "Failed";
                        }
                    }
                }

            }catch(Exception ex){
                result.statusCode = 500;
                result.Message = ex.Message;
                result.isSuccess = false;
            }

            return result;
        }

        public async Task<APIResult<Comment>> UpdateCommentAsync(Comment comment, int id){
            var result = new APIResult<Comment>{
                statusCode = 200,
                Message = "Success",
                isSuccess = true
            };

            try{
                using(var command = Database.GetDbConnection().CreateCommand()){
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = PixelFluxConstants.SP_UpdateOneComment.ToString();
                    command.Parameters.Add(new SqlParameter("@Id", id));
                    command.Parameters.Add(new SqlParameter("@Title", comment.Title));
                    command.Parameters.Add(new SqlParameter("@Content", comment.Content));

                    await Database.OpenConnectionAsync();
                    Comment? commentS = null;
                    using(var reader = await command.ExecuteReaderAsync()){
                        if(reader.HasRows){
                            if(await reader.ReadAsync()){
                                commentS = new Comment{
                         
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Title = reader.GetString(reader.GetOrdinal("Title")),
                                    Content = reader.GetString(reader.GetOrdinal("Content")),
                                    CreatedOn = reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                                    StockId = reader.GetInt32(reader.GetOrdinal("StockId"))
                                };

                                result.Payload = commentS;
                            }   
                        }else{
                            result.statusCode = 404;
                            result.isSuccess = false;
                            result.Message = "Failed";
                        }
                        
                    }
                }

            }catch(Exception ex){
                result.statusCode = 500;
                result.Message = ex.Message;
                result.isSuccess = false;
            }

            return result;
        }


        #endregion





        #region  Stocks
        //refractor this code
        [HttpGet]
        public async Task<APIResult<bool>> DoesAStockExist(int id){
            var result = new APIResult<bool>{
                statusCode = 200,
                Message = "Success",
                isSuccess = true
            };

            try{

                using(var command = Database.GetDbConnection().CreateCommand()){

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = PixelFluxConstants.SP_DoesStockExist.ToString();
                    command.Parameters.Add(new SqlParameter("@Id", id));

                    await Database.OpenConnectionAsync();

                    using(var reader = await command.ExecuteReaderAsync()){
                        if(await reader.ReadAsync()){
                           var exist = reader.GetBoolean(0);

                           result.Message = "Stock exist";
                           result.Payload = exist;       
                        }else{
                            result.Message = "Stock does not exist";
                            result.isSuccess = false;
                            result.statusCode = 404;                       
                        }
                    }

                }

            }catch(Exception ex){
                    result.statusCode = 500;
                    result.Message = ex.Message;
                    result.isSuccess = false;
            }

            return result;
        }

        public async Task<APIResult<int>> AddOneStockAsync(Stocks stock){
            var result = new APIResult<int>{
                statusCode = 200,
                Message = "Success",
                isSuccess = true
            };


            try{
                using(var command = Database.GetDbConnection().CreateCommand()){
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = PixelFluxConstants.SP_AddOneStock.ToString();
                    command.Parameters.Add(new SqlParameter("@CompanyName", stock.CompanyName));
                    command.Parameters.Add(new SqlParameter("@Symbol", stock.Symbol));
                    command.Parameters.Add(new SqlParameter("@Purchase", stock.Purchase));
                    command.Parameters.Add(new SqlParameter("@LastDiv", stock.LastDiv));
                    command.Parameters.Add(new SqlParameter("@Industry", stock.Industry));
                    command.Parameters.Add(new SqlParameter("@MarketCap", stock.MarketCap));
 
                    await Database.OpenConnectionAsync();
                    using(var reader = await command.ExecuteReaderAsync()){
                        if(await reader.ReadAsync()){
                            result.Payload = reader.GetInt32(0);
                            
                            
                        }else{
                            result.isSuccess = false;
                            result.statusCode = 500;
                            result.Message  = "Failed";


                        }
                    }

                }

            }catch(Exception ex){
                result.statusCode = 500;
                result.Message = ex.Message;
                result.isSuccess = false;
            }

            return result;
        }

        public async Task<APIResult<Stocks>> GetOneStockAsync(int id){
            var result = new APIResult<Stocks> {
                statusCode = 200,
                isSuccess = true,
                Message = "Success"
            };

            try{
                using(var command = Database.GetDbConnection().CreateCommand()){
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = PixelFluxConstants.SP_GetOneStockById.ToString();
                    command.Parameters.Add(new SqlParameter("@Id",id));

                    await Database.OpenConnectionAsync();
                    Stocks? stock = null;
                    using(var reader = await command.ExecuteReaderAsync()){
                        if(reader.HasRows){
                             while(await reader.ReadAsync()){

                                if(stock == null){
                                    stock = new Stocks{
                                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                        Symbol = reader.GetString(reader.GetOrdinal("Symbol")),   
                                        CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),   
                                        Purchase = reader.GetDecimal(reader.GetOrdinal("Purchase")),   
                                        LastDiv = reader.GetDecimal(reader.GetOrdinal("LastDiv")),   
                                        Industry = reader.GetString(reader.GetOrdinal("Industry")),   
                                        MarketCap = reader.GetInt64(reader.GetOrdinal("MarketCap")),   
                                    };
                                }


                                if(!await reader.IsDBNullAsync(reader.GetOrdinal("Title"))){
                                    var comment = new Comment{
                                        Id = reader.GetInt32(reader.GetOrdinal("CommentId")),
                                        Title = reader.GetString(reader.GetOrdinal("Title")), 
                                        Content = reader.GetString(reader.GetOrdinal("Content")), 
                                        CreatedOn = reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                                        StockId = reader.GetInt32(reader.GetOrdinal("StockId")),
                                        AppUserId = reader.GetString(reader.GetOrdinal("AppUserId"))
                                        
                                    };
                                    if(!await reader.IsDBNullAsync(reader.GetOrdinal("AppUserId"))){
                                        var user = new AppUser{
                                            UserName = reader.GetString(reader.GetOrdinal("Username")),
                                        };
                                        comment.AppUser = user;
                                    }


                                        stock.Comments.Add(comment);
                                }


                            result.Payload = stock;
                            }
                        }else{
                            result.statusCode = 404;
                            result.Message = "No stock found";
                            result.isSuccess = false;
                        }
                       
                    } 
                
                }

            }catch(Exception ex){
                result.statusCode = 500;
                result.isSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<APIResult<Stocks>> UpdateStockAsync(int id, Stocks stock){
            var result = new APIResult<Stocks>{
                statusCode = 200,
                Message  = "Success",
                isSuccess = true
            };
            try{    
                using(var command = Database.GetDbConnection().CreateCommand()){
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = PixelFluxConstants.SP_UpdateOneStock.ToString();
                    command.Parameters.Add(new SqlParameter("@Id", id));
                    command.Parameters.Add(new SqlParameter("@Symbol", stock.Symbol));
                    command.Parameters.Add(new SqlParameter("@CompanyName", stock.CompanyName));
                    command.Parameters.Add(new SqlParameter("@Purchase", stock.Purchase));
                    command.Parameters.Add(new SqlParameter("@LastDiv", stock.LastDiv));
                    command.Parameters.Add(new SqlParameter("@Industry", stock.Industry));
                    command.Parameters.Add(new SqlParameter("@MarketCap", stock.MarketCap));

                    await Database.OpenConnectionAsync();
                    Stocks? st = null;
                    using(var reader = await command.ExecuteReaderAsync()){
                        if(reader.HasRows){
                            if(await reader.ReadAsync()){
                                st = new Stocks{
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Symbol = reader.GetString(reader.GetOrdinal("Symbol")),
                                    CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                                    Purchase = reader.GetDecimal(reader.GetOrdinal("Purchase")),
                                    LastDiv = reader.GetDecimal(reader.GetOrdinal("LastDiv")),
                                    Industry = reader.GetString(reader.GetOrdinal("Industry")),
                                    MarketCap = reader.GetInt64(reader.GetOrdinal("MarketCap"))
                                };

                                result.Payload = st;
                            }
                        }else{
                            result.statusCode = 500;
                            result.Message = "Failed";
                            result.isSuccess = true;          
                        }
                    }
                }

            }catch(Exception ex ){
                result.statusCode = 500;
                result.Message = ex.Message;
                result.isSuccess = true;
            }
            return result;
        }


        [HttpGet]
        public async Task<APIResult<List<Stocks>>> GetAllStocksAsync(QueryObject query){
            var result = new APIResult<List<Stocks>>{
                statusCode = 200,
                Message = "Success",
                isSuccess = true
            };

            try{    
                using(var command = Database.GetDbConnection().CreateCommand()){
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = PixelFluxConstants.SP_GetAllStocks.ToString();
                    command.Parameters.Add(new SqlParameter("@Symbol", query.Symbol));
                    command.Parameters.Add(new SqlParameter("@CompanyName", query.CompanyName));
                    command.Parameters.Add(new SqlParameter("@SortBy", query.SortBy));
                    command.Parameters.Add(new SqlParameter("@IsDescending", query.isDescending));
                    command.Parameters.Add(new SqlParameter("@PageNumber", query.PageNumber));
                    command.Parameters.Add(new SqlParameter("@PageSize", query.PageSize));


                    await Database.OpenConnectionAsync();
                    var stocksDictionary = new Dictionary<int, Stocks>();
                    using(var reader = await command.ExecuteReaderAsync()){
                        if(reader.HasRows){
                            while(await reader.ReadAsync()){
                                int stockId = reader.GetInt32(reader.GetOrdinal("Id"));
                                if(!stocksDictionary.TryGetValue(stockId, out Stocks? stock)){ //check if the stock already exist using its pkwhat
                                     stock = new Stocks {
                                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                        Symbol = reader.GetString(reader.GetOrdinal("Symbol")),
                                        CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                                        Purchase = reader.GetDecimal(reader.GetOrdinal("Purchase")),
                                        LastDiv = reader.GetDecimal(reader.GetOrdinal("LastDiv")),
                                        Industry = reader.GetString(reader.GetOrdinal("Industry")),
                                        MarketCap = reader.GetInt64(reader.GetOrdinal("MarketCap")),
                                        Comments = new List<Comment>()
                                    };
                                    stocksDictionary.Add(stockId, stock);
                                }
                                
                               
                                if(!await reader.IsDBNullAsync(reader.GetOrdinal("CommentId"))){
                                    var comment = new Comment{
                                        Id = reader.GetInt32(reader.GetOrdinal("CommentId")),
                                        Title = reader.GetString(reader.GetOrdinal("Title")),
                                        Content = reader.GetString(reader.GetOrdinal("Content")),
                                        CreatedOn = reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                                        StockId = reader.GetInt32(reader.GetOrdinal("StockId")),
                                        AppUserId = reader.GetString(reader.GetOrdinal("AppUserId"))
                                    };

                                   if(!await reader.IsDBNullAsync(reader.GetOrdinal("AppUserId")))
                                   {
                                        var user  = new AppUser{
                                            UserName = reader.GetString(reader.GetOrdinal("Username"))
                                        };
                                        comment.AppUser = user;     
                                   }
                                    stock.Comments.Add(comment);
                                }
                                
                               result.Payload = stocksDictionary.Values.ToList();
                            }
                             
                        }else{
                            result.statusCode = 404;
                            result.Message = "No stocks found";
                            result.isSuccess = false;
                        }
                    }
                }
            }catch(Exception ex){
                result.statusCode = 500;
                result.Message = ex.Message;
                result.isSuccess = false;
            }
            return result;
        }



        #endregion
        
    }
}