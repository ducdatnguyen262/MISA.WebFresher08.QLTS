using MISA.WebFresher08.QLTS.Common.Attributes;

namespace MISA.WebFresher08.QLTS.Common.Entities
{
    /// <summary>
    /// Loại tài sản
    /// </summary>
    public class Category : BaseEntity
    {
        /// <summary>
        /// ID loại tài sản
        /// </summary>
        [PrimaryKey]
        public Guid fixed_asset_category_id { get; set; }

        /// <summary>
        /// Mã loại tài sản
        /// </summary>
        public string fixed_asset_category_code { get; set; }

        /// <summary>
        /// Tên loại tài sản
        /// </summary>
        public string fixed_asset_category_name { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string organization_id { get; set; }

        /// <summary>
        /// Tỷ lệ hao mòn (%)
        /// </summary>
        public float depreciation_rate { get; set; }

        /// <summary>
        /// Số năm sử dụng
        /// </summary>
        public string life_time { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string description { get; set; }
    }
}
