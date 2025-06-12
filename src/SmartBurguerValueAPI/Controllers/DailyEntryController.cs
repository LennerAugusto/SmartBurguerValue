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

        public async Task<DailyEntryEntity> CreateWithItemsAsync(DailyEntryEntity entry, List<DailyEntryItemDTO> items)
        {
            entry.Id = Guid.NewGuid();
            await _context.DailyEntry.AddAsync(entry);

            foreach (var item in items)
            {
                var entryItem = await _unityOfWork.DailyEntryItemRepository.BuildDailyEntryItem(entry.Id, item);
                if (entryItem != null)
                    await _context.DailyEntryItem.AddAsync(entryItem);
            }

            return entry;
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
