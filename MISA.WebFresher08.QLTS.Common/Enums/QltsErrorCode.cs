namespace MISA.WebFresher08.QLTS.Common.Enums
{
    /// <summary>
    /// Danh sách mã lỗi khi gọi API
    /// </summary>
    public enum QltsErrorCode
    {
        /// <summary>
        /// Lỗi do exception
        /// </summary>
        Exception = 1,

        /// <summary>
        /// Lỗi do trùng mã
        /// </summary>
        DuplicateCode = 2,

        /// <summary>
        /// Lỗi do nhập thiếu thông tin
        /// </summary>
        EmptyCode = 3,

        /// <summary>
        /// Gọi vào DB để select thất bại
        /// </summary>
        SelectFailed = 4,

        /// <summary>
        /// Gọi vào DB để insert thất bại
        /// </summary>
        InsertFailed = 5,

        /// <summary>
        /// Gọi vào DB để update thất bại
        /// </summary>
        UpdateFailed = 6,

        /// <summary>
        /// Gọi vào DB để delete thất bại
        /// </summary>
        DeleteFailed = 7,

        /// <summary>
        /// Thông tin nhập vào không hợp lệ
        /// </summary>
        InvalidInput = 8,

        /// <summary>
        /// Đăng nhập thất bại
        /// </summary>
        LoginFailed = 8,
    }
}
