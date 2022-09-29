using MISA.WebFresher08.QLTS.Common.Entities;
using MISA.WebFresher08.QLTS.Common.Enums;
using MISA.WebFresher08.QLTS.Common.Resources;
using MISA.WebFresher08.QLTS.Common.Attributes;
using MISA.WebFresher08.QLTS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MISA.WebFresher08.QLTS.BL
{
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Field

        private IBaseDL<T> _baseDL;

        #endregion

        #region Constructor

        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }

        #endregion

        #region Method

        /// <summary>
        /// Lấy danh sách toàn bộ bản ghi
        /// </summary>
        /// <returns>Danh sách toàn bộ bản ghi</returns>
        /// Cretaed by: NDDAT (28/09/2022)
        public IEnumerable<T> GetAllRecords()
        {
            return _baseDL.GetAllRecords();
        }

        /// <summary>
        /// Thêm mới 1 bản ghi
        /// </summary>
        /// <param name="record">Đối tượng bản ghi cần thêm mới</param>
        /// <returns>ID của bản ghi vừa thêm. Return về Guid rỗng nếu thêm mới thất bại</returns>
        /// Cretaed by: NDDAT (28/09/2022)
        public ServiceResponse InsertRecord(T record)
        {
            var validateResult = ValidateRequestData(record);

            if (validateResult != null && validateResult.Success)
            {
                var newRecordID = _baseDL.InsertRecord(record);

                if (newRecordID != Guid.Empty)
                {
                    return new ServiceResponse
                    {
                        Success = true,
                        Data = newRecordID
                    };
                }
                else
                {
                    return new ServiceResponse
                    {
                        Success = true,
                        Data = new ErrorResult(
                            QltsErrorCode.InvalidInput,
                            Resource.DevMsg_InsertFailed,
                            Resource.UserMsg_InsertFailed,
                            Resource.MoreInfo_InsertFailed)
                    };
                }
            }
            else
            {
                return new ServiceResponse
                {
                    Success = false,
                    Data = validateResult?.Data
                };
            }
        }

        /// <summary>
        /// Validate dữ liệu truyền lên từ API
        /// </summary>
        /// <param name="record">Đối tượng cần validate</param>
        /// <returns>Đối tượng ServiceResponse mô tả validate thành công hay thất bại</returns>
        /// Cretaed by: NDDAT (28/09/2022)
        private ServiceResponse ValidateRequestData(T record)
        {
            // Validate dữ liệu đầu vào
            var properties = typeof(T).GetProperties();
            var validateFailures = new List<string>();
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(record);
                var IsNotNullOrEmptyAttribute = (IsNotNullOrEmptyAttribute?)Attribute.GetCustomAttribute(property, typeof(IsNotNullOrEmptyAttribute));
                if (IsNotNullOrEmptyAttribute != null && string.IsNullOrEmpty(propertyValue?.ToString()))
                {
                    validateFailures.Add(IsNotNullOrEmptyAttribute.ErrorMessage);
                }
            }

            if (validateFailures.Count > 0)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Data = validateFailures
                };
            }
            return new ServiceResponse
            {
                Success = true
            };
        }

        #endregion
    }
}
