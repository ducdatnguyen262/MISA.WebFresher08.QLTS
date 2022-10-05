using Microsoft.AspNetCore.Mvc;
using MISA.WebFresher08.QLTS.Common.Entities;
using MISA.WebFresher08.QLTS.Common.Enums;
using MISA.WebFresher08.QLTS.Common.Resources;
using MySqlConnector;
using Dapper;
using MISA.WebFresher08.QLTS.Common.Entities;
using MISA.WebFresher08.QLTS.API.Controllers;
using MISA.WebFresher08.QLTS.BL;

namespace MISA.Web08.QLTS.API.Controllers
{
    public class DepartmentsController : BasesController<Department>
    {
        #region Constructor

        public DepartmentsController(IBaseBL<Department> baseBL) : base(baseBL)
        {
        }

        #endregion

        /// <summary>
        /// Lấy danh sách các phòng ban có chọn lọc
        /// </summary>
        /// <param name="keyword">Từ để tìm kiếm phòng ban</param>
        /// <returns>Danh sách các phòng ban sau khi chọn lọc</returns>
        /// Created by: NDDAT (05/10/2022)
        [HttpGet("filter")]
        public IActionResult FilterDepartments([FromQuery] string? keyword, [FromQuery] string type)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web08.hcsn.nddat;Uid=root;Pwd=;";
                var mysqlConnection = new MySqlConnection(connectionString);

                // Khai báo tên prodecure Insert
                string storedProcedureName = "Proc_department_GetPaging";

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
                var multiDepartments = mysqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý dữ liệu trả về
                if (multiDepartments != null)
                {
                    var departments = multiDepartments.Read<Department>();
                    var totalCount = multiDepartments.Read<long>().Single();

                    return StatusCode(StatusCodes.Status200OK, new PagingData<Department>()
                    {
                        Data = departments.ToList(),
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
