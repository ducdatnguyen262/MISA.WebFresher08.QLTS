﻿using MISA.WebFresher08.QLTS.Common.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher08.QLTS.Common.Entities
{
    public class BaseEntity
    {
        /// <summary>
        /// Người tạo
        /// </summary>
        public string? created_by { get; set; } = Resource.DefaultUser;

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? created_date { get; set; } = DateTime.Now;

        /// <summary>
        /// Người sửa gần nhất
        /// </summary>
        public string? modified_by { get; set; } = Resource.DefaultUser;

        /// <summary>
        /// Ngày sửa gần nhất
        /// </summary>
        public DateTime? modified_date { get; set; } = DateTime.Now;
    }
}
