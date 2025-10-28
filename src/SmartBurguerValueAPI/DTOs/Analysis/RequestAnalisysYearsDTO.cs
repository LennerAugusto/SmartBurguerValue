using SmartBurguerValueAPI.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Analysis
{
    public class RequestAnalisysYearsDTO
    {
        public Guid EnterpriseId { get; set; }
        public PeriodYears PeriodYear { get; set; }
    }
}
