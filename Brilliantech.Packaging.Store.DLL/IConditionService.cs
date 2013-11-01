using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framework.Utilities.EnumUtil;
using System.Collections;
using Brilliantech.Framework.Messages;

namespace Brilliantech.Packaging.Store.DLL
{
    public interface IConditionService
    {
        List<EnumItem> GetEnumItemOptions<T>(T _enum);
        ProcessMsg GenPosition(string positionFormat);
    }
}
