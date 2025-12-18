using Business.Abstract;
using Core.Helpers.Methods;
using Entities;
using Entities.Dtos;
using Repositories.Abstract; // IPrizeRepository'nin olduğu yer
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class PrizeManager : IPrizeService
    {
        private readonly IPrizeRepository _prizeRepository;
        private readonly IUserPrizeRepository _userPrizeRepository;

        public PrizeManager(IPrizeRepository prizeRepository, IUserPrizeRepository userPrizeRepository)
        {
            _prizeRepository = prizeRepository;
            _userPrizeRepository = userPrizeRepository;
        }

        // Ödül Ekleme
        public async Task<ApiResponse<Prize>> AddAsync(PrizeRequestDto prizeDto)
        {
            var prize = new Prize
            {
                Name = prizeDto.Name,
                Description = prizeDto.Description,
                ImgUrl = prizeDto.ImgUrl,
                PointRequired = prizeDto.PointRequired
                // BaseEntity'den gelen CreatedDate otomatik atanır genelde
            };

            await _prizeRepository.AddAsync(prize);
            return ApiResponse<Prize>.SuccessResponse(prize, "Ödül başarıyla eklendi.");
        }

        // Tüm Ödülleri Listeleme
        public async Task<ApiResponse<List<Prize>>> GetAllAsync()
        {
            var prizes = await _prizeRepository.GetAllAsync();
            return ApiResponse<List<Prize>>.SuccessResponse(prizes.ToList());
        }

        // Ödül Silme
        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var prize = await _prizeRepository.GetByIdAsync(id);
            if (prize == null)
                return ApiResponse<bool>.ErrorResponse("Ödül bulunamadı.");

            await _prizeRepository.SoftDeleteAsync(id);
            return ApiResponse<bool>.SuccessResponse(true, "Ödül silindi.");
        }

        // En alta ekleyebilirsin
        public async Task<ApiResponse<List<PrizeResponseDto>>> GetUserPrizesAsync(int userId)
        {
            // Kullanıcının kazandığı ödülleri (UserPrize) çekiyoruz, Prize detaylarını da dahil ediyoruz
            var userPrizes = await _userPrizeRepository.GetAllAsync(
                up => up.UserId == userId,
                up => up.Prize // Prize tablosunu da getir (Include)
            );

            // Gelen veriyi DTO'ya çeviriyoruz ki frontend temiz veri görsün
            var prizeDtos = userPrizes.Select(up => new PrizeResponseDto
            {
                Id = up.Prize.Id,
                Name = up.Prize.Name,
                Description = up.Prize.Description,
                ImgUrl = up.Prize.ImgUrl,
                PointRequired = up.Prize.PointRequired,
                ClaimedAt = up.ClaimedAt // Ne zaman kazandığı bilgisi
            }).ToList();

            return ApiResponse<List<PrizeResponseDto>>.SuccessResponse(prizeDtos);
        }
    }
}