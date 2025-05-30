﻿using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Pagination;

namespace SmartBurguerValueAPI.Interfaces
{
    public interface IFixedCoastRepository : IRepositoryBase<FixedCostEntity>
    {
        Task<PagedList<FixedCoastDTO>> GetAllFixedCostByEnterpriseId(PaginationParamiters paramiters, Guid EnterpriseId);
    }
}
