﻿using MISA.WebFresher08.QLTS.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher08.QLTS.DL
{
    public interface IBaseDL<T>
    {
        #region API Get

        /// <summary>
        /// Lấy danh sách toàn bộ bản ghi
        /// </summary>
        /// <returns>Danh sách toàn bộ bản ghi</returns>
        /// Cretaed by: NDDAT (28/09/2022)
        public IEnumerable<T> GetAllRecords();

        /// <summary>
        /// Lấy 1 bản ghi theo id
        /// </summary>
        /// <param name="recordId">ID của bản ghi cần lấy</param>
        /// <returns>Bản ghi có ID được truyền vào</returns>
        /// Created by: NDDAT (19/09/2022)
        public T GetRecordById(Guid recordId);

        /// <summary>
        /// Lấy danh sách các bản ghi theo từ khóa
        /// </summary>
        /// <param name="keyword">Từ để tìm kiếm bản ghi</param>
        /// <param name="type">Loại dữ liệu được tìm kiếm</param>
        /// <returns>Danh sách các bản ghi sau khi chọn lọc</returns>
        /// Created by: NDDAT (05/10/2022)
        public PagingData<T> FilterRecords(string? keyword, string type);

        #endregion

        #region API Insert

        /// <summary>
        /// Thêm mới 1 bản ghi
        /// </summary>
        /// <param name="record">Đối tượng bản ghi cần thêm mới</param>
        /// <returns>ID của bản ghi vừa thêm. Return về Guid rỗng nếu thêm mới thất bại</returns>
        /// Cretaed by: NDDAT (28/09/2022)
        public Guid InsertRecord(T record);

        #endregion

        #region API Update

        /// <summary>
        /// Cập nhật 1 bản ghi
        /// </summary>
        /// <param name="recordId">ID bản ghi cần cập nhật</param>
        /// <param name="record">Đối tượng cần cập nhật theo</param>
        /// <returns>Đối tượng sau khi cập nhật</returns>
        /// Cretaed by: NDDAT (28/09/2022)
        public Guid UpdateRecord(Guid recordId, T record);

        #endregion

        #region API Delete

        /// <summary>
        /// Xóa 1 bản ghi
        /// </summary>
        /// <param name="recordId">ID bản ghi cần xóa</param>
        /// <returns>ID bản ghi vừa xóa</returns>
        /// Cretaed by: NDDAT (19/09/2022)
        public Guid DeleteRecord(Guid recordId);

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="recordIdList">Danh sách ID các bản ghi cần xóa</param>
        /// <returns>Danh sách ID các bản ghi đã xóa</returns>
        /// Cretaed by: NDDAT (19/09/2022)
        public List<string> DeleteMultiRecords(List<string> recordIdList);

        #endregion

        /// <summary>
        /// Kiểm tra trùng mã bản ghi
        /// </summary>
        /// <param name="recordCode">Mã cần xét trùng</param>
        /// <param name="recordId">Id bản ghi đưa vào (nếu là sửa)</param>
        /// <returns>Số lượng mã tài sản bị trùng</returns>
        /// Cretaed by: NDDAT (12/10/2022)
        public int DuplicateRecordCode(object recordCode, Guid recordId);
    }
}
