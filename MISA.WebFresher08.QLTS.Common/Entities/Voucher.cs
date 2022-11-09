﻿using MISA.WebFresher08.QLTS.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher08.QLTS.Common.Entities
{
    /// <summary>
    /// Chứng từ ghi tăng
    /// </summary>
    public class Voucher : BaseEntity
    {
        /// <summary>
        /// Id chứng từ
        /// </summary>
        [PrimaryKey]
        public Guid voucher_id { get; set; }

        /// <summary>
        /// Số chứng từ
        /// </summary>
        public string voucher_code { get; set; }

        /// <summary>
        /// Ngày chứng từ
        /// </summary>
        public DateTime voucher_date { get; set; }

        /// <summary>
        /// Ngày ghi tăng
        /// </summary>
        public DateTime increment_date { get; set; }

        /// <summary>
        /// Tổng nguyên giá
        /// </summary>
        public double cost { get; set; }

        /// <summary>
        /// Nội dung
        /// </summary>
        public string? description { get; set; }
    }
}
