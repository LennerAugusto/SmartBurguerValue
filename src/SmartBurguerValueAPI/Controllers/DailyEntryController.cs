using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;

namespace SmartBurguerValueAPI.Controllers
{
    //[Authorize(Policy = "Enterprise")]
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
        [HttpGet("get-all/by-enterprise-id")]
        public async Task<ActionResult<IEnumerable<DailyEntryDTO>>> GetAllDailyEntryByEnterpriseId(Guid EnterpriseId)
        {
            var DailyEntry = await _unityOfWork.DailyEntryRepository.GetAllDailyEntryByEnterpriseId(EnterpriseId);
            return Ok(DailyEntry);
        }
        [HttpGet("get-by-id/")]
        public async Task<IActionResult> GetDailyEntryById(Guid DailyEntryId)
        {
            var DailyEntry = await _unityOfWork.DailyEntryRepository.GetDailyEntryById(DailyEntryId);
            return Ok(DailyEntry);
        }
        [HttpPost("create")]
        public async Task<ActionResult<DailyEntryCreateDTO>> CreateWithItemsAsync([FromBody] DailyEntryCreateDTO dto)
        {
            dto.Id = Guid.NewGuid();
            var entity = new DailyEntryEntity
            {
                Id = dto.Id,
                EntryDate = dto.EntryDate,
                Description = dto.Description,
                EnterpriseId = dto.EnterpriseId,
                TotalOrders = dto.TotalOrders ?? 0,  
                IsActive = dto.IsActive,
                DateCreated = dto.DateCreated ?? DateTime.UtcNow,
                DateUpdated = dto.DateUpdated ?? DateTime.UtcNow,
                Items = new List<DailyEntryItemEntity>()
            };
            await _context.DailyEntry.AddAsync(entity);

            foreach (var item in dto.Items)
            {
                var entryItem = await _unityOfWork.DailyEntryItemRepository.BuildDailyEntryItem(dto.Id, item);
                if (entryItem != null)
                    await _context.DailyEntryItem.AddAsync(entryItem);
            }

            await _context.SaveChangesAsync();

            return Ok(dto);
        }

        [HttpPut("update/")]
        public async Task<ActionResult> Put([FromBody] DailyEntryDTO dailyEntry)
        {
            await _unityOfWork.DailyEntryRepository.UpdateDailyEntryAsync(dailyEntry);
            await _unityOfWork.CommitAsync();

            return Ok(dailyEntry);
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
