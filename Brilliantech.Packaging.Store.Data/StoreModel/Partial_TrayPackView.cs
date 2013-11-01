using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framework.Utilities.EnumUtil;
using Brilliantech.Packaging.Store.Data.Enum;

namespace Brilliantech.Packaging.Store.Data.StoreModel
{
    public partial class TrayPackView
    {
        private string tsStatusCN;
        private string spStatusCN;

        public string TsStatusCN
        {
            get
            {
                this.tsStatusCN = EnumUitl.GetEnumDescriptionByEnumValue((TrayStatus)this.tstatus);
                return tsStatusCN;
            }
        }

        public string SpStatusCN
        {
            get
            {
                this.spStatusCN = EnumUitl.GetEnumDescriptionByEnumValue((PackageStatus)this.spsatus);
                return spStatusCN;
            }
        }
   
    }
}
