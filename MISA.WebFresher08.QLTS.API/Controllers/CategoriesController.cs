using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.WebFresher08.QLTS.BL;
using MISA.WebFresher08.QLTS.Common.Entities;
using MISA.WebFresher08.QLTS.Common.Enums;
using MISA.WebFresher08.QLTS.Common.Resources;
using MySqlConnector;

namespace MISA.WebFresher08.QLTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BasesController<Category>
    {
        #region Constructor

        public CategoriesController(IBaseBL<Category> baseBL) : base(baseBL)
        {
        }

        #endregion

        /// <summary>
        /// Lấy danh sách các loại tài sản có chọn lọc
        /// </summary>
        /// <param name="keyword">Từ để tìm kiếm loại tài sản</param>
        /// <returns>Danh sách các loại tài sản sau khi chọn lọc</returns>
        /// Created by: NDDAT (05/10/2022)
        [HttpGet("filter")]
        public IActionResult FilterCategories([FromQuery] string? keyword, [FromQuery] string type)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nddat;Uid=root;Pwd=;";
                var mysqlConnection = new MySqlConnection(connectionString);

                // Khai báo tên prodecure Insert
                string storedProcedureName = "Proc_category_GetPaging";

                // Chuẩn bị tham số đầu vào cho procedure
                var parameters = new DynamicParameters();
                parameters.Add("v_Offset", 1);
                parameters.Add("v_Limit", -1);
                parameters.Add("v_Sort", "");

                var whereConditions = new List<string>();
                if (keyword != null) whereConditions.Add($"{type} LIKE \'%{keyword}%\'");
                string whereClause = string.Join(" AND ", whereConditions);

                parameters.Add("v_Where", whereClause);

                // Thực hiện gọi vào DB để chạy procedure
                var multiCategories = mysqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý dữ liệu trả về
                if (multiCategories != null)
                {
                    var categories = multiCategories.Read<Category>();
                    var totalCount = multiCategories.Read<long>().Single();

                    return StatusCode(StatusCodes.Status200OK, new PagingData<Category>()
                    {
                        Data = categories.ToList(),
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
    }
}
