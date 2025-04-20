using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Dtos.Comment;
using api.Dtos.Stocks;
using api.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace api.Mappers
{
    public static class ApiResultMappers
    {
        //create own mapper for api result which will return  an dto for comment
        public static IEnumerable<CommentDTO> ToCommentDTO(this APIResult<List<Comment>> apiResult ){
            if(apiResult.Payload == null){
                return Enumerable.Empty<CommentDTO>();
            }

            return apiResult.Payload.Select(comment => comment.ToCommentDto());
        }

        public static CommentDTO TransformToCommentDTO(this APIResult<Comment> apiResult){
            if(apiResult.Payload == null){
                return new CommentDTO();
            }

            return apiResult.Payload.ToCommentDto();


        }

        public static CommentDTO TransformAPIComment(this APIResult<Comment?> apiResult){  //update
            if(apiResult.Payload == null){
                return new CommentDTO();
            }   
            return apiResult.Payload.ToCommentDto();
        }

        public static StockDTO TransformToStockDTO(this APIResult<Stocks> aPIResult){
            if(aPIResult.Payload == null){
                return new StockDTO();
            }

            return aPIResult.Payload.ToStockDto();
        }

        public static StockDTO TransformFromUpdate(this APIResult<Stocks?> aPIResult){
            if(aPIResult.Payload == null){
                return new StockDTO();
            }

            return aPIResult.Payload.ToStockDto();
        }

        public static IEnumerable<StockDTO> FromAPIToListStocks(this APIResult<List<Stocks>> aPIResult){
            if(aPIResult.Payload == null){
                return Enumerable.Empty<StockDTO>();
            }

            return aPIResult.Payload.Select(stock => stock.ToStockDto());
        }

        public static NewUserDto FromApiToNewUser(this APIAccounResult<NewUserDto> aPIResult){
            if(aPIResult.Payload == null){
                return null;
            }
            return new NewUserDto{
                Username = aPIResult.Payload.Username,
                Email = aPIResult.Payload.Email,
                Token = aPIResult.Payload.Token,
            };
        }



    }
}