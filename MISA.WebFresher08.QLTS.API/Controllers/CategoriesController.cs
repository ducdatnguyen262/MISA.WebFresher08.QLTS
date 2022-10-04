using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.WebFresher08.QLTS.BL;
using MISA.WebFresher08.QLTS.Common.Entities;

namespace MISA.WebFresher08.QLTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BasesController<Category>
    {
        #region Constructor

        public CategoriesController(IBaseBL<Category> baseBL) : base(baseBL)
        {
        }

        #endregion
    }
}
