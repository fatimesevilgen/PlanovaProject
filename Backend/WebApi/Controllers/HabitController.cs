using Business.Abstract;
using Core.Helpers.Extensions; // User.GetUserId() için
using Core.Helpers.Methods;    // ApiResponse için
using Entities;                // Habit için
using Entities.Dtos;           // HabitAddDto için
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    /// <summary>
    /// Kullanıcıların alışkanlık işlemlerini (Ekleme, Listeleme, Güncelleme, Silme, Tamamlama) yöneten API.
    /// </summary>
    [Authorize] // Sadece giriş yapmış kullanıcılar buraya erişebilir
    [Route("api/[controller]")]
    [ApiController]
    public class HabitsController : ControllerBase
    {
        private readonly IHabitService _habitService;
        /// <summary>
        /// HabitsController bağımlılıklarını (service) yükler.
        /// </summary>
        /// <param name="habitService">Alışkanlık iş mantığı servisi</param>
        public HabitsController(IHabitService habitService)
        {
            _habitService = habitService;
        }

        /// <summary>
        /// Yeni bir alışkanlık oluşturur.
        /// </summary>
        /// <param name="habitDto"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<ActionResult<ApiResponse<Habit>>> AddAsync([FromBody] HabitAddDto habitDto)
        {
            // Token'ın içindeki gizli ID'yi okuyoruz (AuthController ile aynı yöntem)
            int userId = User.GetUserId();

            var result = await _habitService.AddAsync(habitDto, userId);

            // Başarılıysa 200 OK ve datayı dönüyoruz
            if (result.Success)
            {
                return Ok(result);
            }

            // Hata varsa AuthController'daki gibi ErrorResponse formatında dönüyoruz
            return BadRequest(ApiResponse<Habit>.ErrorResponse(result.Message ?? "Alışkanlık eklenirken bir hata oluştu."));
        }

        /// <summary>
        /// Giriş yapmış kullanıcının tüm alışkanlıklarını listeler.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        public async Task<ActionResult<ApiResponse<List<Habit>>>> GetAllAsync()
        {
            int userId = User.GetUserId();

            var result = await _habitService.GetListByUserIdAsync(userId);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(ApiResponse<List<Habit>>.ErrorResponse(result.Message ?? "Listeleme başarısız."));
        }
        /// <summary>
        /// ID'si verilen alışkanlığı soft delete ile siler.
        /// </summary>
        /// <param name="id">Silinecek alışkanlığın ID numarası</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            int userId = User.GetUserId(); // Token'dan kim olduğunu bulma
            var result = await _habitService.DeleteAsync(id, userId);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(ApiResponse<bool>.ErrorResponse(result.Message));
        } 
        
        /// <summary>
        /// Var olan bir alışkanlığın detaylarını(isim, hedef, sıklık vb.) günceller.
        /// </summary>
        /// <param name="habitDto"> Güncellenecek alışkanlığın ID'si ve yeni verilerini içeren nesne.</param>
        /// <returns>Güncellenmiş alışkanlık bilgisini döner.</returns>
        [HttpPut("Update")]
        public async Task<ActionResult<ApiResponse<Habit>>> Update([FromBody] HabitUpdateDto habitDto)
        {
            int userId = User.GetUserId();
            var result = await _habitService.UpdateAsync(habitDto, userId);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(ApiResponse<Habit>.ErrorResponse(result.Message));
        }
        /// <summary>
        /// Seçilen alışkanlık için bugüne bir "Tik" atar.
        /// </summary>
        [HttpPost("tick/{id}")]
        public async Task<ActionResult<ApiResponse<HabitLog>>> Tick(int id)
        {
            int userId = User.GetUserId();
            var result = await _habitService.TickHabitAsync(id, userId);

            if (result.Success)
                return Ok(result);

            return BadRequest(ApiResponse<HabitLog>.ErrorResponse(result.Message));
        }



    }
}