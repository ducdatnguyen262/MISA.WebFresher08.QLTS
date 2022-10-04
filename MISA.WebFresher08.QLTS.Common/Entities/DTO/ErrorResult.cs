using MISA.WebFresher08.QLTS.Common.Enums;

namespace MISA.WebFresher08.QLTS.Common.Entities
{
    /// <summary>
    /// Kết quả hiển thị khi xảy ra lỗi
    /// </summary>
    public class ErrorResult
    {
        #region Property

        /// <summary>
        /// Mã lỗi
        /// </summary>
        public QltsErrorCode ErrorCode { get; set; }

        /// <summary>
        /// Lời nhắn cho dev
        /// </summary>
        public string DevMsg { get; set; }

        /// <summary>
        /// Lời nhắn cho người dùng
        /// </summary>
        public string UserMsg { get; set; }

        /// <summary>
        /// Một số thông tin khác
        /// </summary>
        public Object MoreInfo { get; set; }

        /// <summary>
        /// Số nhận dạng theo dõi
        /// </summary>
        public string TraceId { get; set; }

        #endregion

        #region Constructor

        public ErrorResult() { }

        public ErrorResult(QltsErrorCode errorCode, string devMsg, string userMsg, Object moreInfo, string? traceId = null)
        {
            ErrorCode = errorCode;
            DevMsg = devMsg;
            UserMsg = userMsg;
            MoreInfo = moreInfo;
            TraceId = traceId;
        }

        public ErrorResult(QltsErrorCode errorCode)
        {
            ErrorCode = errorCode;
        }

        #endregion
    }
}
