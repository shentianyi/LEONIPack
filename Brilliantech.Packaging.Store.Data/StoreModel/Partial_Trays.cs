using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Packaging.Store.Data.Enum;

namespace Brilliantech.Packaging.Store.Data.StoreModel
{
   public partial class Trays
    {
        private string statusCN;

        public string StatusCN
        {
            get {
                this.statusCN = EnumUitl.GetEnumDescriptionByEnumValue((TrayStatus)this.status);
                return statusCN; 
            }
        }
    }
}
