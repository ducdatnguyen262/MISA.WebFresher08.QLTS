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
        /// Lấy 1 tài sản theo id
        /// </summary>
        /// <param name="assetId">ID của tài sản cần lấy</param>
        /// <returns>Đối tượng tài sản có ID được truyền vào</returns>
        /// Created by: NDDAT (19/09/2022)
        [HttpGet("{assetId}")]
        public IActionResult GetAssetById([FromRoute] Guid assetId)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nddat;Uid=root;Pwd=;";
                var mysqlConnection = new MySqlConnection(connectionString);

                // Khai báo tên prodecure Insert
                string storedProcedureName = "Proc_asset_Select";

                // Chuẩn bị tham số đầu vào cho procedure
                var parameters = new DynamicParameters();
                parameters.Add("v_fixed_asset_id", assetId);

                // Thực hiện gọi vào DB để chạy procedure
                var asset = mysqlConnection.QueryFirstOrDefault<Asset>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý dữ liệu trả về
                if(asset != null)
                {
                    return StatusCode(StatusCodes.Status200OK, asset);
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
                parameters.Add("v_Offset", (page-1)*limit);
                parameters.Add("v_Limit", limit);
                parameters.Add("v_Sort", "");

                var whereConditions = new List<string>();
                if (keyword != null) whereConditions.Add($"(fixed_asset_name LIKE \'%{keyword}%\' OR fixed_asset_name LIKE \'%{keyword}%\')");
                if (departmentId != null) whereConditions.Add($"department_id LIKE \'{departmentId}\'");
                if (categoryId != null) whereConditions.Add($"fixed_asset_category_id LIKE \'{categoryId}\'");
                string whereClause = string.Join(" AND ", whereConditions);

                parameters.Add("v_Where", whereClause);

                // Thực hiện gọi vào DB để chạy procedure
                var multiAssets = mysqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý dữ liệu trả về
                if(multiAssets != null)
                {
                    var assets = multiAssets.Read<Asset>();
                    var totalCount = multiAssets.Read<long>().Single();
                    return StatusCode(StatusCodes.Status200OK, new PagingData<Asset>()
                    {
                        Data = assets.ToList(),
                        TotalCount = totalCount,
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

        #region API Update

        /// <summary>
        /// Cập nhật 1 tài sản
        /// </summary>
        /// <param name="assetId">ID tài sản cần cập nhật</param>
        /// <param name="asset">Đối tượng tài sản cần cập nhật theo</param>
        /// <returns>Đối tượng tài sản sau khi cập nhật</returns>
        /// Cretaed by: NDDAT (19/09/2022)
        [HttpPut("{assetId}")]
        public IActionResult UpdateAsset([FromRoute] Guid assetId, [FromBody] Asset asset)
        {
            try
            {
                // Validate dữ liệu đầu vào
                var properties = typeof(Asset).GetProperties();
                var validateFailures = new List<string>();
                foreach (var property in properties)
                {
                    string propertyName = property.Name;
                    var propertyValue = property.GetValue(asset);
                    var IsNotNullOrEmptyAttribute = (IsNotNullOrEmptyAttribute?)Attribute.GetCustomAttribute(property, typeof(IsNotNullOrEmptyAttribute));
                    if (IsNotNullOrEmptyAttribute != null && string.IsNullOrEmpty(propertyValue?.ToString()))
                    {
                        validateFailures.Add(IsNotNullOrEmptyAttribute.ErrorMessage);
                    }
                }

                if (validateFailures.Count > 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                        QltsErrorCode.InvalidInput,
                        Resource.DevMsg_ValidateFailed,
                        Resource.UserMsg_ValidateFailed,
                        validateFailures,
                        HttpContext.TraceIdentifier));
                }

                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nddat;Uid=root;Pwd=;";
                var mysqlConnection = new MySqlConnection(connectionString);

                // Khai báo tên prodecure Insert
                string storedProcedureName = "Proc_asset_Update";

                // Chuẩn bị tham số đầu vào cho procedure
                var parameters = new DynamicParameters();
                parameters.Add("v_fixed_asset_id", assetId);
                parameters.Add("v_fixed_asset_code", asset.fixed_asset_code);
                parameters.Add("v_fixed_asset_name", asset.fixed_asset_name);
                parameters.Add("v_organization_id", "");
                parameters.Add("v_organization_code", "");
                parameters.Add("v_organization_name", "");
                parameters.Add("v_department_id", asset.department_id);
                parameters.Add("v_department_code", "");
                parameters.Add("v_department_name", "");
                parameters.Add("v_fixed_asset_category_id", asset.fixed_asset_category_id);
                parameters.Add("v_fixed_asset_category_code", "");
                parameters.Add("v_fixed_asset_category_name", "");
                parameters.Add("v_purchase_date", asset.purchase_date);
                parameters.Add("v_cost", asset.cost);
                parameters.Add("v_quantity", asset.quantity);
                parameters.Add("v_depreciation_rate", asset.depreciation_rate);
                parameters.Add("v_tracked_year", asset.tracked_year);
                parameters.Add("v_life_time", asset.life_time);
                parameters.Add("v_production_year", asset.production_year);
                parameters.Add("v_active", asset.active);
                parameters.Add("v_created_by", asset.created_by);
                parameters.Add("v_created_date", asset.created_date);
                parameters.Add("v_modified_by", asset.modified_by);

                // Thực hiện gọi vào DB để chạy procedure
                var numberOfAffectedRows = mysqlConnection.Execute(storedProcedureName,
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý dữ liệu trả về
                if (numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, asset);
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

        #region API Delete

        /// <summary>
        /// Xóa 1 tài sản
        /// </summary>
        /// <param name="assetId">ID tài sản cần xóa</param>
        /// <returns>ID tài sản vừa xóa</returns>
        /// Cretaed by: NDDAT (19/09/2022)
        [HttpDelete("{assetId}")]
        public IActionResult DeleteAsset([FromRoute] Guid assetId)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nddat;Uid=root;Pwd=;";
                var mysqlConnection = new MySqlConnection(connectionString);

                // Khai báo tên prodecure Insert
                string storedProcedureName = "Proc_asset_Delete";

                // Chuẩn bị tham số đầu vào cho procedure
                var parameters = new DynamicParameters();
                parameters.Add("v_fixed_asset_id", assetId);

                // Xử lý dữ liệu trả về
                var numberOfAffectedRows = mysqlConnection.Execute(storedProcedureName,
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý dữ liệu trả về
                if (numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, assetId);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                        QltsErrorCode.DeleteFailed,
                        Resource.DevMsg_DeleteFailed,
                        Resource.UserMsg_DeleteFailed,
                        Resource.MoreInfo_DeleteFailed,
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
        /// Xóa nhiều tài sản
        /// </summary>
        /// <param name="assetIds">Danh sách ID các tài sản cần xóa</param>
        /// <returns></returns>
        /// Cretaed by: NDDAT (19/09/2022)
        [HttpPost("batch-delete")]
        public IActionResult DeleteMultiAssets([FromBody] List<string> assetIdList)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nddat;Uid=root;Pwd=;";
                var mysqlConnection = new MySqlConnection(connectionString);

                // Khai báo tên prodecure Insert
                string storedProcedureName = "Proc_asset_BatchDelete";

                // Chuẩn bị tham số đầu vào cho procedure
                var parameters = new DynamicParameters();
                var queryList = new List<string>();
                for (int i = 0; i < assetIdList.Count; i++)
                {
                    queryList.Add($"fixed_asset_id=\'{assetIdList[i]}\'");
                }
                string assetIds = string.Join(" OR ", queryList);
                parameters.Add("v_fixed_asset_ids_query", assetIds);

                // Xử lý dữ liệu trả về
                var numberOfAffectedRows = mysqlConnection.Execute(storedProcedureName,
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý dữ liệu trả về
                if (numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, assetIdList);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult(
                        QltsErrorCode.DeleteFailed,
                        Resource.DevMsg_DeleteFailed,
                        Resource.UserMsg_DeleteFailed,
                        Resource.MoreInfo_DeleteFailed,
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