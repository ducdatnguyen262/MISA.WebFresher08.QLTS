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
        /// Lấy danh sách các chứng từ có chọn lọc
        /// </summary>
        /// <param name="keyword">Từ để tìm kiếm theo số chứng từ và nội dung</param>
        /// <param name="limit">Số chứng từ muốn lấy</param>
        /// <param name="page">Số trang bắt đầu lấy</param>
        /// <returns>Danh sách các chứng từ sau khi chọn lọc và các giá trị khác</returns>
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

        /// <summary>
        /// Lấy chi tiết chứng từ
        /// </summary>
        /// <param name="voucherId">Id chứng từ</param>
        /// <param name="limit">Số bản ghi muốn lấy</param>
        /// <param name="page">Số trang bắt đầu lấy</param>
        /// <returns>Danh sách các tài sản theo chứng từ</returns>
        /// Created by: NDDAT (09/11/2022)
        [HttpGet("detail/{voucherId}")]
        public IActionResult GetVoucherDetail([FromRoute] Guid voucherId, [FromQuery] int limit, [FromQuery] int page)
        {
            try
            {
                var filterResponse = _voucherBL.GetVoucherDetail(voucherId, limit, page);
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

        /// <summary>
        /// Thêm nhiều tài sản trong chứng từ
        /// </summary>
        /// <param name="voucherId">ID chứng từ đang sửa</param>
        /// <param name="assetIdList">Danh sách ID các tài sản cần thêm</param>
        /// <returns>Danh sách ID các tài sản đã thêm</returns>
        /// Cretaed by: NDDAT (21/11/2022)
        [HttpPost("detail/batch-add")]
        public IActionResult AddVoucherDetail(Guid voucherId, List<string> assetIdList)
        {
            try
            {
                List<string> results = _voucherBL.AddVoucherDetail(voucherId, assetIdList);

                if (results.Count > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, results);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                        QltsErrorCode.Exception,
                        Resource.DevMsg_DeleteFailed,
                        Resource.UserMsg_DeleteFailed,
                        assetIdList,
                        HttpContext.TraceIdentifier));
                }
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

        /// <summary>
        /// Xóa nhiều tài sản trong chứng từ
        /// </summary>
        /// <param name="voucherId">ID chứng từ đang sửa</param>
        /// <param name="assetIdList">Danh sách ID các tài sản cần xóa</param>
        /// <returns>Danh sách ID các tài sản đã xóa</returns>
        /// Cretaed by: NDDAT (21/11/2022)
        [HttpPost("detail/batch-delete")]
        public IActionResult DeleteVoucherDetail(Guid voucherId, List<string> assetIdList)
        {
            try
            {
                List<string> results = _voucherBL.DeleteVoucherDetail(voucherId, assetIdList);

                if (results.Count > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, results);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                        QltsErrorCode.Exception,
                        Resource.DevMsg_DeleteFailed,
                        Resource.UserMsg_DeleteFailed,
                        assetIdList,
                        HttpContext.TraceIdentifier));
                }
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
