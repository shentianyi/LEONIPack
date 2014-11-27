using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brilliantech.Packaging.Store.DLL
{
    public class ApiException : Exception
    {
        public ApiException()
            : base() { }

        public ApiException(string message)
            : base(message) { }
    }
}
