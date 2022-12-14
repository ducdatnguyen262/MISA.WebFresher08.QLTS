using Dapper;
using MISA.WebFresher08.QLTS.Common.Attributes;
using MISA.WebFresher08.QLTS.Common.Entities;
using MISA.WebFresher08.QLTS.Common.Enums;
using MISA.WebFresher08.QLTS.Common.Resources;
using MISA.WebFresher08.QLTS.DL.DepartmentDL;
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
        /// Lấy danh sách các chứng từ có chọn lọc
        /// </summary>
        /// <param name="keyword">Từ để tìm kiếm theo số chứng từ và nội dung</param>
        /// <param name="limit">Số chứng từ muốn lấy</param>
        /// <param name="page">Số trang bắt đầu lấy</param>
        /// <returns>Danh sách các chứng từ sau khi chọn lọc và các giá trị khác</returns>
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

            // Khai báo tên prodecure
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
                var totalCost = multiVouchers.Read<long>().Single();

                filterResponse = new PagingData<Voucher>(vouchers, totalCount, 0, totalCost, 0, 0);
            }

            return filterResponse;
        }

        /// <summary>
        /// Lấy chi tiết chứng từ
        /// </summary>
        /// <param name="voucherId">Id chứng từ</param>
        /// <param name="limit">Số bản ghi muốn lấy</param>
        /// <param name="page">Số trang bắt đầu lấy</param>
        /// <returns>Danh sách các tài sản theo chứng từ</returns>
        /// Created by: NDDAT (09/11/2022)
        public PagingData<Asset> GetVoucherDetail(string? keyword, Guid voucherId, int limit, int page)
        {
            // Chuẩn bị tham số đầu vào cho procedure
            var parameters = new DynamicParameters();
            parameters.Add("v_voucher_id", voucherId);
            parameters.Add("v_Offset", (page - 1) * limit);
            parameters.Add("v_Limit", limit);
            parameters.Add("v_Sort", "");

            var whereConditions = new List<string>();
            if (keyword != null) whereConditions.Add($"(fa.fixed_asset_code LIKE \'%{keyword}%\' OR fa.fixed_asset_name LIKE \'%{keyword}%\')");
            string whereClause = string.Join(" AND ", whereConditions);

            parameters.Add("v_Where", whereClause);

            // Khai báo tên prodecure
            string storedProcedureName = "Proc_voucher_GetDetail";

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
                var totalCost = multiAssets.Read<long>().Single();
                var totalDepreciation = multiAssets.Read<long>().Single();
                var totalRemain = multiAssets.Read<long>().Single();

                filterResponse = new PagingData<Asset>(assets, totalCount, 0, totalCost, totalDepreciation, totalRemain);
            }

            return filterResponse;
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
            string query = "";
            foreach (var assetId in assetIdList)
            {
                if(assetIdList.Last() == assetId) query = query + $"('{Guid.NewGuid()}', '{voucherId}', '{assetId}');";
                else query = query + $"('{Guid.NewGuid()}', '{voucherId}', '{assetId}'),";
            }

            // Chuẩn bị tham số đầu vào cho procedure
            var parameters = new DynamicParameters();
            parameters.Add("v_add_query", query);
            parameters.Add($"v_asset_ids", $"'{String.Join("','", assetIdList)}'");

            // Khởi tạo kết nối tới DB MySQL
            string connectionString = DataContext.MySqlConnectionString;
            using (var mysqlConnection = new MySqlConnection(connectionString))
            {
                // Khai báo tên prodecure
                string storedProcedureName = "Proc_voucher_BatchAddDetail";

                mysqlConnection.Open();

                // Bắt đầu transaction.
                using (var transaction = mysqlConnection.BeginTransaction())
                {
                    try
                    {
                        int numberOfAffectedRows = mysqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure, transaction: transaction);

                        if (numberOfAffectedRows / 2 == assetIdList.Count)
                        {
                            transaction.Commit();
                            return assetIdList;
                        }
                        else
                        {
                            transaction.Rollback();
                            return new List<string>();
                        }
                    }
                    finally
                    {
                        mysqlConnection.Close();
                    }
                }
            }
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
            // Chuẩn bị tham số đầu vào cho procedure
            var parameters = new DynamicParameters();
            parameters.Add("v_voucher_id", voucherId);
            parameters.Add($"v_asset_ids", $"'{String.Join("','", assetIdList)}'");

            // Khởi tạo kết nối tới DB MySQL
            string connectionString = DataContext.MySqlConnectionString;
            using (var mysqlConnection = new MySqlConnection(connectionString))
            {
                // Khai báo tên prodecure
                string storedProcedureName = "Proc_voucher_BatchDeleteDetail";

                mysqlConnection.Open();

                // Bắt đầu transaction.
                using (var transaction = mysqlConnection.BeginTransaction())
                {
                    try
                    {
                        int numberOfAffectedRows = mysqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure, transaction: transaction);

                        if (numberOfAffectedRows / 2 == assetIdList.Count)
                        {
                            transaction.Commit();
                            return assetIdList;
                        }
                        else
                        {
                            transaction.Rollback();
                            return new List<string>();
                        }
                    }
                    finally 
                    { 
                        mysqlConnection.Close(); 
                    }
                }
            }
        }
    }
}
