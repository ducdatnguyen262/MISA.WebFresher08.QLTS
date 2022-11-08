using MISA.WebFresher08.QLTS.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher08.QLTS.Common.Entities
{
    public class Budget
    {
        [PrimaryKey]
        public Guid budget_id { get; set; }

        public string budget_name { get; set; }
    }
}
