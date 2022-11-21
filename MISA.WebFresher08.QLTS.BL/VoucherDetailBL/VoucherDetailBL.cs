using MISA.WebFresher08.QLTS.Common.Entities;
using MISA.WebFresher08.QLTS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher08.QLTS.BL
{
    public class VoucherDetailBL : BaseBL<VoucherDetail>, IVoucherDetailBL
    {
        public VoucherDetailBL(IBaseDL<VoucherDetail> baseDL) : base(baseDL)
        {
        }
    }
}
