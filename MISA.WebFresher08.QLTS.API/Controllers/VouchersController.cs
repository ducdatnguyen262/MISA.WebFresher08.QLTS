using Microsoft.AspNetCore.Mvc;
using MISA.WebFresher08.QLTS.BL;
using MISA.WebFresher08.QLTS.Common.Entities;
using MISA.WebFresher08.QLTS.Common.Enums;
using MISA.WebFresher08.QLTS.Common.Resources;

namespace MISA.WebFresher08.QLTS.API.Controllers
{
    public class VouchersController : BasesController<Voucher>
    {
        #region Field

        private IVoucherBL _voucherBL;

        #endregion

        #region Constructor

        public VouchersController(IVoucherBL voucherBL) : base(voucherBL)
        {
            _voucherBL = voucherBL;
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
        [HttpGet("filters")]
        public IActionResult FilterVouchers([FromQuery] string? keyword, [FromQuery] int limit, [FromQuery] int page)
        {
            try
            {
                var filterResponse = _voucherBL.FilterVouchers(keyword, limit, page);

                return StatusCode(StatusCodes.Status200OK, filterResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                    QltsErrorCode.Exception,
                    Resource.DevMsg_Exception,
                    Resource.UserMsg_Exception,
                    Resource.MoreInfo_Exception,
                    HttpContext.TraceIdentifier));
            }
        }

        #endregion
    }
}
