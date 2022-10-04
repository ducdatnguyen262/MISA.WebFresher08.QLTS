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

        #region API Get

        /// <summary>
        /// Lấy danh sách các tài sản có chọn lọc
        /// </summary>
        /// <param name="keyword">Từ để tìm kiếm theo mã và tên tài sản</param>
        /// <param name="departmentId">ID phòng ban</param>
        /// <param name="categoryId">ID loại tài sản</param>
        /// <param name="limit">Số bản ghi muốn lấy</param>
        /// <param name="offset">Thứ tự bản ghi bắt đầu lấy</param>
        /// <returns>Danh sách các tài sản sau khi chọn lọc</returns>
        /// Created by: NDDAT (19/09/2022)
        [HttpGet("filter")]
        public IActionResult FilterAssets([FromQuery] string? keyword, [FromQuery] Guid? departmentId, [FromQuery] Guid? categoryId, [FromQuery] int limit, [FromQuery] int page)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nddat;Uid=root;Pwd=;";
                var mysqlConnection = new MySqlConnection(connectionString);

                // Khai báo tên prodecure Insert
                string storedProcedureName = "Proc_asset_GetPaging";

                // Chuẩn bị tham số đầu vào cho procedure
                var parameters = new DynamicParameters();
                parameters.Add("v_Offset", (page - 1) * limit);
                parameters.Add("v_Limit", limit);
                parameters.Add("v_Sort", "");

                var whereConditions = new List<string>();
                if (keyword != null) whereConditions.Add($"(fixed_asset_code LIKE \'%{keyword}%\' OR fixed_asset_name LIKE \'%{keyword}%\')");
                if (departmentId != null) whereConditions.Add($"department_id LIKE \'{departmentId}\'");
                if (categoryId != null) whereConditions.Add($"fixed_asset_category_id LIKE \'{categoryId}\'");
                string whereClause = string.Join(" AND ", whereConditions);

                parameters.Add("v_Where", whereClause);

                // Thực hiện gọi vào DB để chạy procedure
                var multiAssets = mysqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý dữ liệu trả về
                if (multiAssets != null)
                {
                    var assets = multiAssets.Read<Asset>();
                    var totalCount = multiAssets.Read<long>().Single();
                    var totalQuantity = multiAssets.Read<long>().Single();
                    var totalCost = multiAssets.Read<double>().Single();
                    var totalDepreciation = multiAssets.Read<double>().Single();
                    var totalRemain = multiAssets.Read<double>().Single();

                    return StatusCode(StatusCodes.Status200OK, new PagingData<Asset>()
                    {
                        Data = assets.ToList(),
                        TotalCount = totalCount,
                        TotalQuantity = totalQuantity,
                        TotalCost = totalCost,
                        TotalDepreciation = totalDepreciation,
                        TotalRemain = totalRemain,
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                        QltsErrorCode.SelectFailed,
                        Resource.DevMsg_SelectFailed,
                        Resource.UserMsg_SelectFailed,
                        Resource.MoreInfo_SelectFailed,
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

        /// <summary>
        /// Sinh mã tài sản tiếp theo
        /// </summary>
        /// <returns>Mã tài sản tiếp theo</returns>
        /// Cretaed by: NDDAT (01/10/2022)
        [HttpGet("nextCode")]
        public IActionResult NextAssetCode()
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nddat;Uid=root;Pwd=;";
                var mysqlConnection = new MySqlConnection(connectionString);

                // Khai báo tên prodecure Insert
                string storedProcedureName = "Proc_asset_GetNextCode";

                // Xử lý dữ liệu trả về
                var nextAssetCode = mysqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý dữ liệu trả về
                if (nextAssetCode != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new NextCode()
                    {
                        Code = nextAssetCode,
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

        /// <summary>
        /// Kiểm tra trùng mã tài sản
        /// </summary>
        /// <returns>Mã tài sản nếu trùng</returns>
        /// Cretaed by: NDDAT (01/10/2022)
        [HttpGet("duplicateCode/{assetCode}")]
        public IActionResult DuplicateAssetCode([FromRoute] string assetCode)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nddat;Uid=root;Pwd=;";
                var mysqlConnection = new MySqlConnection(connectionString);

                // Khai báo tên prodecure Insert
                string storedProcedureName = "Proc_asset_DuplicateCode";

                // Chuẩn bị tham số đầu vào cho procedure
                var parameters = new DynamicParameters();
                parameters.Add("v_fixed_asset_code", assetCode);

                // Thực hiện gọi vào DB để chạy procedure
                int duplicates = mysqlConnection.QueryFirstOrDefault<int>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý dữ liệu trả về
                return StatusCode(StatusCodes.Status200OK, new DuplicateCode
                {
                    Duplicates = duplicates,
                });

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
    }
}