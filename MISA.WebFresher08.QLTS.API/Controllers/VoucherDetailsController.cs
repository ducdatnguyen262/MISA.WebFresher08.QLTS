using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MISA.WebFresher08.QLTS.API.Controllers;
using MISA.WebFresher08.QLTS.BL;
using MISA.WebFresher08.QLTS.Common.Entities;

namespace MISA.WebFresher08.QLTS.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VoucherDetailsController : BasesController<VoucherDetail>
    {
        #region Constructor

        public VoucherDetailsController(IBaseBL<VoucherDetail> baseBL) : base(baseBL)
        {
        }
        
        #endregion       
    }
}
