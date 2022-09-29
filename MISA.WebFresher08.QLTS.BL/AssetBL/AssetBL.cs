using MISA.WebFresher08.QLTS.Common.Entities;
using MISA.WebFresher08.QLTS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher08.QLTS.BL
{
    public class AssetBL : BaseBL<Asset>, IAssetBL
    {
        #region Field

        private IAssetDL _assetDL;

        #endregion

        #region Constructer

        public AssetBL (IAssetDL assetDL) : base(assetDL)
        {
            _assetDL = assetDL;
        }

        #endregion
    }
}
