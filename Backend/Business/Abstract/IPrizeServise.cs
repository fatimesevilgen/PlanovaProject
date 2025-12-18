using Core.Helpers.Methods; // ApiResponse için
using Entities;
using Entities.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IPrizeService
    {
        Task<ApiResponse<Prize>> AddAsync(PrizeRequestDto prizeDto);
        Task<ApiResponse<List<Prize>>> GetAllAsync();
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<List<PrizeResponseDto>>> GetUserPrizesAsync(int userId);
    }
}
