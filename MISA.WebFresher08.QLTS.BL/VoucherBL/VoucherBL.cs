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
        /// Lấy danh sách các chứng từ có chọn lọc
        /// </summary>
        /// <param name="keyword">Từ để tìm kiếm theo số chứng từ và nội dung</param>
        /// <param name="limit">Số chứng từ muốn lấy</param>
        /// <param name="page">Số trang bắt đầu lấy</param>
        /// <returns>Danh sách các chứng từ sau khi chọn lọc và các giá trị khác</returns>
        /// Created by: NDDAT (08/11/2022)
        public PagingData<Voucher> FilterVouchers(string? keyword, int limit, int page)
        {
            return _voucherDL.FilterVouchers(keyword, limit, page);
        }

        /// <summary>
        /// Lấy chi tiết chứng từ
        /// </summary>
        /// <param name="voucherId">Id chứng từ</param>
        /// <param name="limit">Số bản ghi muốn lấy</param>
        /// <param name="page">Số trang bắt đầu lấy</param>
        /// <returns>Danh sách các tài sản theo chứng từ</returns>
        /// Created by: NDDAT (09/11/2022)
        public PagingData<Asset> GetVoucherDetail(Guid voucherId, int limit, int page)
        {
            return _voucherDL.GetVoucherDetail(voucherId, limit, page);
        }

        /// <summary>
        /// Thêm nhiều tài sản trong chứng từ
        /// </summary>
        /// <param name="voucherId">ID chứng từ đang sửa</param>
        /// <param name="assetIdList">Danh sách ID các tài sản cần thêm</param>
        /// <returns>Danh sách ID các tài sản đã thêm</returns>
        /// Cretaed by: NDDAT (21/11/2022)
        public List<string> AddVoucherDetail(Guid voucherId, List<string> assetIdList)
        {
            return _voucherDL.AddVoucherDetail(voucherId, assetIdList);
        }

        /// <summary>
        /// Xóa nhiều tài sản trong chứng từ
        /// </summary>
        /// <param name="voucherId">ID chứng từ đang sửa</param>
        /// <param name="assetIdList">Danh sách ID các tài sản cần xóa</param>
        /// <returns>Danh sách ID các tài sản đã xóa</returns>
        /// Cretaed by: NDDAT (21/11/2022)
        public List<string> DeleteVoucherDetail(Guid voucherId, List<string> assetIdList)
        {
            return _voucherDL.DeleteVoucherDetail(voucherId, assetIdList);
        }

        #endregion
    }
}
