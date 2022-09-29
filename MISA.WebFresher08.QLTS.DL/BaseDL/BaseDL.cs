using Dapper;
using MISA.WebFresher08.QLTS.Common.Attributes;
using MISA.WebFresher08.QLTS.Common.Entities;
using MISA.WebFresher08.QLTS.Common.Enums;
using MISA.WebFresher08.QLTS.Common.Resources;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher08.QLTS.DL
{
    public class BaseDL<T> : IBaseDL<T>
    {
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
            foreach(var property in properties)
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
                // Khai báo tên prodecure Insert
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
    }
}
