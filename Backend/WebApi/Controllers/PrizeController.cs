using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrizeController : ControllerBase
    {
        private readonly IPrizeService _prizeService;

        public PrizeController(IPrizeService prizeService)
        {
            _prizeService = prizeService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] PrizeRequestDto prizeDto)
        {
            var result = await _prizeService.AddAsync(prizeDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _prizeService.GetAllAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _prizeService.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}