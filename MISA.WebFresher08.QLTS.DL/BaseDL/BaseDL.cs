using Dapper;
using MISA.WebFresher08.QLTS.Common.Attributes;
using MISA.WebFresher08.QLTS.Common.Entities;
using MISA.WebFresher08.QLTS.Common.Enums;
using MISA.WebFresher08.QLTS.Common.Resources;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MISA.WebFresher08.QLTS.DL
{
    public class BaseDL<T> : IBaseDL<T>
    {
        #region API Get

        /// <summary>
        /// Lấy danh sách toàn bộ bản ghi
        /// </summary>
        /// <returns>Danh sách toàn bộ bản ghi</returns>
        /// Cretaed by: NDDAT (28/09/2022)
        public IEnumerable<T> GetAllRecords()
        {
            // Khai báo tên stored procedure
            string storedProcedureName = String.Format(Resource.Proc_GetAll, typeof(T).Name);

            // Khởi tạo kết nối tới DB MySQL
            using (var mysqlConnection = new MySqlConnection(DataContext.MySqlConnectionString))
            {
                // Thực hiện gọi vào DB
                var records = mysqlConnection.Query<T>(
                    storedProcedureName,
                    commandType: System.Data.CommandType.StoredProcedure);

                return records;
            }
        }

        /// <summary>
        /// Lấy 1 bản ghi theo id
        /// </summary>
        /// <param name="recordId">ID của bản ghi cần lấy</param>
        /// <returns>Bản ghi có ID được truyền vào</returns>
        /// Created by: NDDAT (19/09/2022)
        public T GetRecordById(Guid recordId)
        {
            // Chuẩn bị tham số đầu vào cho procedure
            var parameters = new DynamicParameters();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var primaryKeyAttribute = (PrimaryKeyAttribute)Attribute.GetCustomAttribute(property, typeof(PrimaryKeyAttribute));
                if (primaryKeyAttribute != null)
                {
                    parameters.Add($"v_{property.Name}", recordId);
                    break;
                }
            }

            // Khởi tạo kết nối tới DB MySQL
            string connectionString = DataContext.MySqlConnectionString;
            T record;
            using (var mysqlConnection = new MySqlConnection(connectionString))
            {
                // Khai báo tên prodecure
                string storedProcedureName = String.Format(Resource.Proc_Select, typeof(T).Name);

                // Thực hiện gọi vào DB để chạy procedure
                record = mysqlConnection.QueryFirstOrDefault<T>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            return record;
        }

        /// <summary>
        /// Lấy danh sách các bản ghi theo từ khóa
        /// </summary>
        /// <param name="keyword">Từ để tìm kiếm bản ghi</param>
        /// <param name="type">Loại dữ liệu được tìm kiếm</param>
        /// <returns>Danh sách các bản ghi sau khi chọn lọc</returns>
        /// Created by: NDDAT (05/10/2022)
        public PagingData<T> FilterRecords(string? keyword, string type)
        {
            // Chuẩn bị tham số đầu vào cho procedure
            var parameters = new DynamicParameters();
            parameters.Add("v_Offset", 1);
            parameters.Add("v_Limit", -1);
            parameters.Add("v_Sort", "");

            var whereConditions = new List<string>();
            if (keyword != null) whereConditions.Add($"{type} LIKE \'%{keyword}%\'");
            string whereClause = string.Join(" AND ", whereConditions);

            parameters.Add("v_Where", whereClause);

            // Khởi tạo kết nối tới DB MySQL
            string connectionString = DataContext.MySqlConnectionString;
            PagingData<T> recordsData = new PagingData<T>();
            using (var mysqlConnection = new MySqlConnection(connectionString))
            {
                // Khai báo tên prodecure
                string storedProcedureName = String.Format(Resource.Proc_GetPaging, typeof(T).Name);

                // Thực hiện gọi vào DB để chạy procedure
                var multiRecords = mysqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                 
                var records = multiRecords.Read<T>().ToList();
                var totalCount = multiRecords.Read<long>().Single();

                recordsData = new PagingData<T>(records, totalCount, 0, 0, 0, 0);
            }
            return recordsData;
        }

        #endregion

        #region API Insert

        /// <summary>
        /// Thêm mới 1 bản ghi
        /// </summary>
        /// <param name="record">Đối tượng bản ghi cần thêm mới</param>
        /// <returns>ID của bản ghi vừa thêm. Return về Guid rỗng nếu thêm mới thất bại</returns>
        /// Cretaed by: NDDAT (28/09/2022)
        public Guid InsertRecord(T record)
        {
            // Chuẩn bị tham số đầu vào cho procedure
            var parameters = new DynamicParameters();
            var newRecordID = Guid.NewGuid();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                string propertyName = property.Name;
                object propertyValue;
                var primaryKeyAttribute = (PrimaryKeyAttribute)Attribute.GetCustomAttribute(property, typeof(PrimaryKeyAttribute));
                if (primaryKeyAttribute != null)
                {
                    propertyValue = newRecordID;
                }
                else
                {
                    propertyValue = property.GetValue(record, null);
                }
                parameters.Add($"v_{propertyName}", propertyValue);
            }

            // Khởi tạo kết nối tới DB MySQL
            string connectionString = DataContext.MySqlConnectionString;
            int numberOfAffectedRows = 0;
            using (var mysqlConnection = new MySqlConnection(connectionString))
            {
                // Khai báo tên prodecure
                string storedProcedureName = String.Format(Resource.Proc_Add, typeof(T).Name);

                // Thực hiện gọi vào DB để chạy procedure
                numberOfAffectedRows = mysqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            // Xử lý dữ liệu trả về
            if (numberOfAffectedRows > 0)
            {
                return newRecordID;
            }
            else
            {
                return Guid.Empty;
            }
        }

        #endregion

        #region API Update

        /// <summary>
        /// Cập nhật 1 bản ghi
        /// </summary>
        /// <param name="recordId">ID bản ghi cần cập nhật</param>
        /// <param name="record">Đối tượng cần cập nhật theo</param>
        /// <returns>ID của bản ghi sau khi cập nhật. Return về Guid rỗng nếu cập nhật thất bại</returns>
        /// Cretaed by: NDDAT (28/09/2022)
        public Guid UpdateRecord(Guid recordId, T record)
        {
            // Chuẩn bị tham số đầu vào cho procedure
            var parameters = new DynamicParameters();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                string propertyName = property.Name;
                object propertyValue;
                var primaryKeyAttribute = (PrimaryKeyAttribute)Attribute.GetCustomAttribute(property, typeof(PrimaryKeyAttribute));
                if (primaryKeyAttribute != null)
                {
                    propertyValue = recordId;
                }
                else
                {
                    propertyValue = property.GetValue(record, null);
                }
                parameters.Add($"v_{propertyName}", propertyValue);
            }

            // Khởi tạo kết nối tới DB MySQL
            string connectionString = DataContext.MySqlConnectionString;
            int numberOfAffectedRows = 0;
            using (var mysqlConnection = new MySqlConnection(connectionString))
            {
                // Khai báo tên prodecure
                string storedProcedureName = String.Format(Resource.Proc_Update, typeof(T).Name);

                // Thực hiện gọi vào DB để chạy procedure
                numberOfAffectedRows = mysqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            // Xử lý dữ liệu trả về
            if (numberOfAffectedRows > 0)
            {
                return recordId;
            }
            else
            {
                return Guid.Empty;
            }
        }

        #endregion

        #region API Delete

        /// <summary>
        /// Xóa 1 bản ghi
        /// </summary>
        /// <param name="recordId">ID bản ghi cần xóa</param>
        /// <returns>ID bản ghi vừa xóa</returns>
        /// Cretaed by: NDDAT (19/09/2022)
        public Guid DeleteRecord(Guid recordId)
        {
            // Chuẩn bị tham số đầu vào cho procedure
            var parameters = new DynamicParameters();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var primaryKeyAttribute = (PrimaryKeyAttribute)Attribute.GetCustomAttribute(property, typeof(PrimaryKeyAttribute));
                if (primaryKeyAttribute != null)
                {
                    parameters.Add($"v_{property.Name}", recordId);
                    break;
                }
            }

            // Khởi tạo kết nối tới DB MySQL
            string connectionString = DataContext.MySqlConnectionString;
            int numberOfAffectedRows = 0;
            using (var mysqlConnection = new MySqlConnection(connectionString))
            {
                // Khai báo tên prodecure
                string storedProcedureName = String.Format(Resource.Proc_Delete, typeof(T).Name);

                // Thực hiện gọi vào DB để chạy procedure
                numberOfAffectedRows = mysqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            // Xử lý dữ liệu trả về
            if (numberOfAffectedRows > 0)
            {
                return recordId;
            }
            else
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="recordIdList">Danh sách ID các bản ghi cần xóa</param>
        /// <returns>Danh sách ID các bản ghi đã xóa</returns>
        /// Cretaed by: NDDAT (19/09/2022)
        public List<string> DeleteMultiRecords(List<string> recordIdList)
        {
            // Chuẩn bị tham số đầu vào cho procedure
            var properties = typeof(T).GetProperties();
            var propertyName = "";
            foreach (var property in properties)
            {
                var primaryKeyAttribute = (PrimaryKeyAttribute)Attribute.GetCustomAttribute(property, typeof(PrimaryKeyAttribute));
                if (primaryKeyAttribute != null)
                {
                    propertyName = property.Name;
                    break;
                }
            }

            // Khởi tạo kết nối tới DB MySQL
            string connectionString = DataContext.MySqlConnectionString;
            using (var mysqlConnection = new MySqlConnection(connectionString))
            {
                // Khai báo tên prodecure
                string storedProcedureName = String.Format(Resource.Proc_BatchDelete, typeof(T).Name);

                mysqlConnection.Open();

                // Bắt đầu transaction.
                using (var transaction = mysqlConnection.BeginTransaction())
                {
                    try
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add($"v_{propertyName}s", $"'{String.Join("','", recordIdList)}'" );
                        int numberOfAffectedRows = mysqlConnection.QueryFirstOrDefault<int>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure, transaction: transaction);

                        if (numberOfAffectedRows == recordIdList.Count)
                        {
                            transaction.Commit();
                            return recordIdList;
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

        #endregion

        /// <summary>
        /// Kiểm tra trùng mã bản ghi
        /// </summary>
        /// <param name="recordCode">Mã cần xét trùng</param>
        /// <param name="recordId">Id bản ghi đưa vào (nếu là sửa)</param>
        /// <returns>Số lượng mã tài sản bị trùng</returns>
        /// Cretaed by: NDDAT (12/10/2022)
        public int DuplicateRecordCode(object recordCode, Guid recordId)
        {
            // Khai báo tên prodecure
            string storedProcedureName = String.Format(Resource.Proc_DuplicateCode, typeof(T).Name);

            // Chuẩn bị tham số đầu vào cho procedure
            var parameters = new DynamicParameters();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var isNotDuplicateAttribute = (IsNotDuplicateAttribute)Attribute.GetCustomAttribute(property, typeof(IsNotDuplicateAttribute));
                if (isNotDuplicateAttribute != null)
                {
                    parameters.Add($"v_{property.Name}", recordCode);
                }

                var primaryKeyAttribute = (PrimaryKeyAttribute)Attribute.GetCustomAttribute(property, typeof(PrimaryKeyAttribute));
                if (primaryKeyAttribute != null)
                {
                    parameters.Add($"v_{property.Name}", recordId);
                }
            }

            // Khởi tạo kết nối tới DB MySQL
            string connectionString = DataContext.MySqlConnectionString;
            int duplicates = 0;
            using (var mysqlConnection = new MySqlConnection(connectionString))
            {
                duplicates = mysqlConnection.QueryFirstOrDefault<int>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            return duplicates;
        }

        /// <summary>
        /// Sinh mã tiếp theo
        /// </summary>
        /// <returns>Mã tiếp theo</returns>
        /// Cretaed by: NDDAT (01/10/2022)
        public string NextCode()
        {
            // Khai báo tên prodecure
            string storedProcedureName = String.Format(Resource.Proc_GetNextCode, typeof(T).Name);

            // Khởi tạo kết nối tới DB MySQL
            string connectionString = DataContext.MySqlConnectionString;
            string nextAssetCode = "";
            using (var mysqlConnection = new MySqlConnection(connectionString))
            {
                nextAssetCode = mysqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);
            }
            // Xử lý dữ liệu trả về
            if (nextAssetCode != null)
            {
                return nextAssetCode;
            }
            else
            {
                return "";
            }
        }
    }
}
