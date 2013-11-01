using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Brilliantech.Packaging.Store.Data.Enum
{
    [Flags]
    public enum TrayStatus
    {
        [Description("已入库")]
        Stored = 100,
        [Description("已取消")]
        Cancled = 200,
        [Description("已导出")]
        Exported = 300
    }
}
