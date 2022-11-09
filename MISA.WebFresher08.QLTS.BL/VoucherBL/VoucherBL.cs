using MISA.WebFresher08.QLTS.Common.Entities;
using MISA.WebFresher08.QLTS.DL;
using MISA.WebFresher08.QLTS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher08.QLTS.BL
{
    public class VoucherBL : BaseBL<Voucher>, IVoucherBL
    {
        #region Field

        private IVoucherDL _voucherDL;

        #endregion

        #region Constructor

        public VoucherBL(IVoucherDL voucherDL) : base(voucherDL)
        {
            _voucherDL = voucherDL;
        }

        #endregion

        #region Method

        /// <summary>
        /// Lấy danh sách các ghi tăng có chọn lọc
        /// </summary>
        /// <param name="keyword">Từ để tìm kiếm theo số chứng từ và nội dung</param>
        /// <param name="limit">Số bản ghi muốn lấy</param>
        /// <param name="page">Số trang bắt đầu lấy</param>
        /// <returns>Danh sách các ghi tăng sau khi chọn lọc và các giá trị khác</returns>
        /// Created by: NDDAT (08/11/2022)
        public PagingData<Voucher> FilterVouchers(string? keyword, int limit, int page)
        {
            return _voucherDL.FilterVouchers(keyword, limit, page);
        }

        #endregion
    }
}
