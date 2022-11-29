using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MISA.WebFresher08.QLTS.BL;
using MISA.WebFresher08.QLTS.Common.Entities;

namespace MISA.WebFresher08.QLTS.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BudgetsController : BasesController<Budget>
    {
        #region Constructor

        public BudgetsController(IBaseBL<Budget> baseBL) : base(baseBL)
        {
        } 

        #endregion

    }
}
