using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framework.Utilities.EnumUtil;
using Brilliantech.Packaging.Store.Data.Enum;

namespace Brilliantech.Packaging.Store.Data.StoreModel
{
   public partial class  SinglePackage
    {
        private string statusCN;

        public string StatusCN
        {
            get
            {
                this.statusCN = EnumUitl.GetEnumDescriptionByEnumValue((PackageStatus)this.status);
                return statusCN;
            }
        }
    }
}
