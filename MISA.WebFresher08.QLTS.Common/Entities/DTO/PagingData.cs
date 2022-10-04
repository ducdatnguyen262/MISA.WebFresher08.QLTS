namespace MISA.WebFresher08.QLTS.Common.Entities
{
    /// <summary>
    /// Dữ liệu cho phân trang
    /// </summary>
    public class PagingData<T>
    {
        /// <summary>
        /// Danh sách các bản ghi hiển thị
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// Tổng số bản ghi hiển thị
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// Tổng số lượng
        /// </summary>
        public long TotalQuantity { get; set; }

        /// <summary>
        /// Tổng số nguyên giá
        /// </summary>
        public double TotalCost { get; set; }

        /// <summary>
        /// Tổng số hao mòn lũy kế
        /// </summary>
        public double TotalDepreciation { get; set; }

        /// <summary>
        /// Tổng số còn lại
        /// </summary>
        public double TotalRemain { get; set; }
    }
}
