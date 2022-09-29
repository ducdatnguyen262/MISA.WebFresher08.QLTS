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
    }
}
