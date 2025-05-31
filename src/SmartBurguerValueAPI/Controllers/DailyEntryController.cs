using Microsoft.AspNetCore.Mvc;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;

namespace SmartBurguerValueAPI.Controllers
{
    [Route("api/daily-entry")]
    public class DailyEntryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUnityOfWork _unityOfWork;

        public DailyEntryController(AppDbContext context, IUnityOfWork unityOfWork)
        {
            _context = context;
            _unityOfWork = unityOfWork;
        }
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<DailyEntryDTO>>> GetAllDailyEntry()
        {
            var DailyEntryes = await _unityOfWork.DailyEntryRepository.GetAllAsync();
            return Ok(DailyEntryes);
        }

        [HttpGet("get-by-id/")]
        public async Task<IActionResult> GetDailyEntryById(Guid DailyEntryId)
        {
            var DailyEntry = _unityOfWork.DailyEntryRepository.GetByIdAsync(DailyEntryId);
            return Ok(DailyEntry);
        }

        [HttpPost("create")]
        public async Task<ActionResult<DailyEntryDTO>> CreateDailyEntry([FromBody] DailyEntryEntity dailyEntry)
        {
            var DailyEntry = _unityOfWork.DailyEntryRepository.Create(dailyEntry);
            await _unityOfWork.CommitAsync();
            return Ok(DailyEntry);
        }


        [HttpPut("update/")]
        public ActionResult Put([FromBody] DailyEntryEntity dailyEntry)
        {
            var DailyEntry = _unityOfWork.DailyEntryRepository.Update(dailyEntry);
            _unityOfWork.CommitAsync();
            return Ok(DailyEntry);
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var DailyEntry = await _unityOfWork.DailyEntryRepository.GetByIdAsync(id);

            await _unityOfWork.DailyEntryRepository.Delete(DailyEntry);
            await _unityOfWork.CommitAsync();
            return Ok(DailyEntry);
        }
    }
}
