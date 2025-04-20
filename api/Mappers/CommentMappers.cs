using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Identity.Client;

namespace api.Mappers
{
    public static class CommentMappers
    {
            public static CommentDTO ToCommentDto(this Comment comment){
            return new CommentDTO
            {
                Id = comment.Id,
                Title = comment.Title,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
                StockId = comment.StockId,
                CreatedBy = comment.AppUser.UserName
            };

        }


           public static Comment ToCommentCreateCommentDto(this CreateCommentDTO commentDto, int stockId){
            return new Comment
            {

                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = stockId

            };
        }

        public static Comment ToCommentFromUpdate(this UpdateCommentRequestDTO comment){
            return new Comment{
                Title = comment.Title,
                Content = comment.Content
            };
        }
    }
}