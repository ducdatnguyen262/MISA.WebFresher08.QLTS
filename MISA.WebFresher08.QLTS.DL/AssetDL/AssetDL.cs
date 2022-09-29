using MISA.WebFresher08.QLTS.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using MISA.WebFresher08.QLTS.Common.Resources;

namespace MISA.WebFresher08.QLTS.DL
{
    public class AssetDL : BaseDL<Asset>, IAssetDL
    {
    }
}
