using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.WebFresher08.QLTS.BL;
using MISA.WebFresher08.QLTS.Common.Entities;
using MISA.WebFresher08.QLTS.Common.Enums;
using MISA.WebFresher08.QLTS.Common.Resources;
using MySqlConnector;

namespace MISA.WebFresher08.QLTS.API.Controllers
{
    public class CategoriesController : BasesController<Category>
    {
        #region Constructor

        public CategoriesController(IBaseBL<Category> baseBL) : base(baseBL)
        {
        }

        #endregion
    }
}
