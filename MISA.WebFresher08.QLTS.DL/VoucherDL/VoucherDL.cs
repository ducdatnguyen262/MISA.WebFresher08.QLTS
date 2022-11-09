using Dapper;
using MISA.WebFresher08.QLTS.Common.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher08.QLTS.DL
{
    public class VoucherDL : BaseDL<Voucher>, IVoucherDL
    {
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
            // Chuẩn bị tham số đầu vào cho procedure
            var parameters = new DynamicParameters();
            parameters.Add("v_Offset", (page - 1) * limit);
            parameters.Add("v_Limit", limit);
            parameters.Add("v_Sort", "");

            var whereConditions = new List<string>();
            if (keyword != null) whereConditions.Add($"(voucher_code LIKE \'%{keyword}%\' OR description LIKE \'%{keyword}%\')");
            string whereClause = string.Join(" AND ", whereConditions);

            parameters.Add("v_Where", whereClause);

            // Khai báo tên prodecure Insert
            string storedProcedureName = "Proc_voucher_GetPaging";

            // Khởi tạo kết nối tới DB MySQL
            string connectionString = DataContext.MySqlConnectionString;
            var filterResponse = new PagingData<Voucher>();
            using (var mysqlConnection = new MySqlConnection(connectionString))
            {
                // Thực hiện gọi vào DB để chạy procedure
                var multiVouchers = mysqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý dữ liệu trả về
                var vouchers = multiVouchers.Read<Voucher>();
                var totalCount = multiVouchers.Read<long>().Single();

                filterResponse = new PagingData<Voucher>(vouchers, totalCount, 0, 0, 0, 0);
            }

            return filterResponse;
        }
    }
}
