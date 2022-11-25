using Dapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using MISA.WebFresher08.QLTS.Common.Entities;
using MISA.WebFresher08.QLTS.Common.Enums;
using MISA.WebFresher08.QLTS.Common.Resources;
using MISA.WebFresher08.QLTS.Common.Attributes;
using MySqlConnector;
using System.Data;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using MISA.WebFresher08.QLTS.BL;
using MISA.WebFresher08.QLTS.API.Controllers;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace MISA.WebFresher08.QLTS.API
{
    public class AssetsController : BasesController<Asset>
    {
        #region Field

        private IAssetBL _assetBL;

        #endregion

        #region Constructor

        public AssetsController(IAssetBL assetBL) : base(assetBL)
        {
            _assetBL = assetBL;
        }

        #endregion

        #region Method

        /// <summary>
        /// Lấy danh sách các tài sản có chọn lọc
        /// </summary>
        /// <param name="keyword">Từ để tìm kiếm theo mã và tên tài sản</param>
        /// <param name="departmentId">ID phòng ban</param>
        /// <param name="categoryId">ID loại tài sản</param>
        /// <param name="limit">Số bản ghi muốn lấy</param>
        /// <param name="page">Số trang bắt đầu lấy</param>
        /// <returns>Danh sách các tài sản sau khi chọn lọc và các giá trị khác</returns>
        /// Created by: NDDAT (19/09/2022)
        [HttpPost("filters")]
        public IActionResult FilterAssets([FromQuery] string? keyword, [FromQuery] Guid? departmentId, [FromQuery] Guid? categoryId, [FromQuery] int limit, [FromQuery] int page, [FromBody] List<string> assetIdList, [FromQuery] int mode)
        {
            try
            {
                var filterResponse = _assetBL.FilterAssets(keyword, departmentId, categoryId, limit, page, assetIdList, mode);
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
        /// Kiểm tra tài sản đã ghi tăng chưa
        /// </summary>
        /// <param name="assetId">Mã tài sản cần kiểm tra</param>
        /// <returns>Mã chứng từ nếu đã ghi tăng</returns>
        /// Created by: NDDAT (24/11/2022)
        [HttpGet("checkIncrement/{assetId}")]
        public IActionResult CheckIncrement([FromRoute] Guid assetId)
        {
            try
            {
                string voucherCode = _assetBL.CheckIncrement(assetId);
                // Xử lý dữ liệu trả về
                if (voucherCode != "")
                {
                    return StatusCode(StatusCodes.Status200OK, new NextCode()
                    {
                        Code = voucherCode,
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                        QltsErrorCode.UpdateFailed,
                        Resource.DevMsg_UpdateFailed,
                        Resource.UserMsg_UpdateFailed,
                        Resource.MoreInfo_UpdateFailed,
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