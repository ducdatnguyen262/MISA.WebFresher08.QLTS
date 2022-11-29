using Microsoft.AspNetCore.Mvc;
using MISA.WebFresher08.QLTS.Common.Entities;
using MISA.WebFresher08.QLTS.Common.Enums;
using MISA.WebFresher08.QLTS.Common.Resources;
using MySqlConnector;
using Dapper;
using MISA.WebFresher08.QLTS.API.Controllers;
using MISA.WebFresher08.QLTS.BL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace MISA.Web08.QLTS.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DepartmentsController : BasesController<Department>
    {
        #region Constructor

        public DepartmentsController(IBaseBL<Department> baseBL) : base(baseBL)
        {
        }

        #endregion
    }
}
