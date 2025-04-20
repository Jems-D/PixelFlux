using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stocks;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<APIResult<List<Stocks>>> GetAllAsync(QueryObject query);
        Task<APIResult<Stocks?>> GetOneByIdAsync(int id);
        Task<Stocks?> GetBySymbol(string Symbol); 
        Task<Stocks> CreateStockAsync(Stocks stockModel);
        Task<APIResult<Stocks?>> UpdateStockAsync(int id, UpdateStockRequestDTO stockRequestDTO);
        Task<Stocks?> DeleteStockAsync(int id);
        Task<APIResult<bool>> DoesStockExist(int id);
    }

}