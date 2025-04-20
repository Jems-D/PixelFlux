using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Helpers;
using api.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<APIResult<List<Comment>>> GetAllCommentAsync(CommentQueryObject commentQuery);
        Task<APIResult<Comment?>> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment commentModel);
        Task<APIResult<Comment?>> UpdateAsync(int id, Comment comment);
        Task<Comment?> DeleteCommentAsync(int id);
    }
}