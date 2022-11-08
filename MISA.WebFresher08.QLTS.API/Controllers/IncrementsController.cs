using MISA.WebFresher08.QLTS.BL;
using MISA.WebFresher08.QLTS.Common.Entities;

namespace MISA.WebFresher08.QLTS.API.Controllers
{
    public class IncrementsController : BasesController<Increment>
    {
        #region Constructor

        public IncrementsController(IBaseBL<Increment> baseBL) : base(baseBL)
        {
        } 

        #endregion
    }
}
