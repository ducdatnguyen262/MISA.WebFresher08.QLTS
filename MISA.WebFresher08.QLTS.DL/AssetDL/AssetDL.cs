using MISA.WebFresher08.QLTS.Common.Entities;
using Dapper;
using MySqlConnector;


namespace MISA.WebFresher08.QLTS.DL
{
    public class AssetDL : BaseDL<Asset>, IAssetDL
    {
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
        public PagingData<Asset> FilterAssets(string? keyword, Guid? departmentId, Guid? categoryId, int limit, int page)
        {
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

            // Khai báo tên prodecure Insert
            string storedProcedureName = "Proc_asset_GetPaging";

            // Khởi tạo kết nối tới DB MySQL
            string connectionString = DataContext.MySqlConnectionString;
            var filterResponse = new PagingData<Asset>();
            using(var mysqlConnection = new MySqlConnection(connectionString))
            {
                // Thực hiện gọi vào DB để chạy procedure
                var multiAssets = mysqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý dữ liệu trả về
                var assets = multiAssets.Read<Asset>();
                var totalCount = multiAssets.Read<long>().Single();
                var totalQuantity = multiAssets.Read<long>().Single();
                var totalCost = multiAssets.Read<double>().Single();
                var totalDepreciation = multiAssets.Read<double>().Single();
                var totalRemain = multiAssets.Read<double>().Single();

                filterResponse = new PagingData<Asset>(assets, totalCount, totalQuantity, totalCost, totalDepreciation, totalRemain);
            }

            return filterResponse;
        }

        /// <summary>
        /// Lấy danh sách các tài sản theo chứng từ
        /// </summary>
        /// <param name="voucherCode">Số chứng từ</param>
        /// <param name="limit">Số bản ghi muốn lấy</param>
        /// <param name="page">Số trang bắt đầu lấy</param>
        /// <returns>Danh sách các tài sản theo chứng từ</returns>
        /// Created by: NDDAT (09/11/2022)
        public PagingData<Asset> Voucher(string voucherCode, int limit, int page)
        {
            // Chuẩn bị tham số đầu vào cho procedure
            var parameters = new DynamicParameters();
            parameters.Add("v_voucher_code", $"(\'{voucherCode}\')");
            parameters.Add("v_Offset", (page - 1) * limit);
            parameters.Add("v_Limit", limit);
            parameters.Add("v_Sort", "");

            // Khai báo tên prodecure Insert
            string storedProcedureName = "Proc_asset_Voucher";

            // Khởi tạo kết nối tới DB MySQL
            string connectionString = DataContext.MySqlConnectionString;
            var filterResponse = new PagingData<Asset>();
            using (var mysqlConnection = new MySqlConnection(connectionString))
            {
                // Thực hiện gọi vào DB để chạy procedure
                var multiAssets = mysqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý dữ liệu trả về
                var assets = multiAssets.Read<Asset>();
                var totalCount = multiAssets.Read<long>().Single();

                filterResponse = new PagingData<Asset>(assets, totalCount, 0, 0, 0, 0);
            }

            return filterResponse;
        }
    }
}
