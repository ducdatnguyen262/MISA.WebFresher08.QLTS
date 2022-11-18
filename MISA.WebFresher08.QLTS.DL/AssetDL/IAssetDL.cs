﻿using MISA.WebFresher08.QLTS.Common.Entities;

namespace MISA.WebFresher08.QLTS.DL
{
    public interface IAssetDL : IBaseDL<Asset>
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
        public PagingData<Asset> FilterAssets(string? keyword, Guid? departmentId, Guid? categoryId, int limit, int page, List<string> assetIdList, Boolean chooseOnly);
    }
}
