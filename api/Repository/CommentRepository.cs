using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;

        public CommentRepository(ApplicationDBContext context, UserManager<AppUser> appuser)
        {
            _context = context;
        }
        #region Comment
        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
           /*  var comment = await _context.AddOneCommentAsnyc(commentModel);
            if(comment.isSuccess = true && comment.Payload > 0){
                commentModel.Id = comment.Payload;
            } */
            return commentModel;
        }

        public async Task<APIResult<List<Comment>>> GetAllCommentAsync(CommentQueryObject commentQuery)
        {
            //return await _context.Comments.Include(a => a.AppUser).ToListAsync();
             //var comments =  _context.Comments.Include(a => a.AppUser).AsQueryable();
           /*  if(!string.IsNullOrWhiteSpace(commentQuery.Symbol)){
                comments = comments.Where(s => s.Stock.Symbol == commentQuery.Symbol);
            }

            if(commentQuery.IsDescending == true){
                comments = comments.OrderByDescending(s => s.CreatedOn);
            } 
            return await comments.ToListAsync(); */
            return await _context.GetAllCommentsAsync(commentQuery);
        }

        public async Task<APIResult<Comment?>> GetByIdAsync(int id)
        {
             //ef core
            /* var comment = await _context.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(c => c.Id == id);
            if(comment  == null)
            {
                return  null;
            } */

            var comment = await _context.GetOneCommentAsync(id);

            return new APIResult<Comment?>{
                statusCode = comment.statusCode,
                Message = comment.Message,
                isSuccess = comment.isSuccess,
                Payload = comment.Payload
            }; 
        }

        public async Task<APIResult<Comment?>> UpdateAsync(int id, Comment comment)
        {
            //var existingComment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            var existingComment  = await _context.UpdateCommentAsync(comment, id);
            if(!existingComment.isSuccess){
                return null;
            }

            //existingComment.Title = comment.Title;
            //existingComment.Content = comment.Content;
            //await _context.SaveChangesAsync();

            return new APIResult<Comment?>{
                statusCode = existingComment.statusCode,
                Message  = existingComment.Message,
                isSuccess = existingComment.isSuccess,
                Payload = existingComment.Payload
            };
        }


        public async Task<Comment?> DeleteCommentAsync(int id){
            var existingComment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if(existingComment == null){
                return null;
            }

            _context.Comments.Remove(existingComment);
            await _context.SaveChangesAsync();

            return existingComment;
        }

        #endregion



        #region Stocks










        #endregion
    }
}