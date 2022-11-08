using MISA.WebFresher08.QLTS.BL;
using MISA.WebFresher08.QLTS.Common.Entities;

namespace MISA.WebFresher08.QLTS.API.Controllers
{
    public class BudgetsController : BasesController<Budget>
    {
        #region Constructor

        public BudgetsController(IBaseBL<Budget> baseBL) : base(baseBL)
        {
        } 

        #endregion

    }
}
