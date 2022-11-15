using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher08.QLTS.Common.Entities
{
    /// <summary>
    /// Chi tiết chứng từ
    /// </summary>
    public class VoucherDetail : BaseEntity
    {
        /// <summary>
        /// Id chi tiết chứng từ
        /// </summary>
        public Guid voucher_detail_id { get; set; }

        /// <summary>
        /// Mã chứng từ
        /// </summary>
        public string voucher_code { get; set; }

        /// <summary>
        /// Id tài sản
        /// </summary>
        public Guid fixed_asset_id { get; set; }
    }
}
