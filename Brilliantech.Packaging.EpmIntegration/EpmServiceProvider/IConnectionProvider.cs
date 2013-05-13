using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrift.Transport;

namespace Brilliantech.Packaging.EpmIntegration.EpmServiceProvider
{
   public interface IConnectionProvider
    {
        TTransport GetConnection();
        void ReturnConnection(TTransport transport);
    }
}
