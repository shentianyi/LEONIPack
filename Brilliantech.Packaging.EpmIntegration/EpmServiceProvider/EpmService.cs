using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrift.Transport;
using CZ.IF.Datahouse;
using Thrift.Protocol;

namespace Brilliantech.Packaging.EpmIntegration.EpmServiceProvider
{
    public class EpmService : IDisposable
    {
        ConnectionProvider pool;
        TTransport transport;
        Datahouse.Client client;
        bool disposed;
        public EpmService()
        {
            disposed = false;
            pool = new ConnectionProvider();
            transport = pool.GetConnection();
            TProtocol protocol = new TBinaryProtocol(transport);
            client = new Datahouse.Client(protocol);

        }

        public void AddProductPack(Dictionary<string, string> dataMap)
        {
            client.addProductPack(Conf.TServiceAccessKey, dataMap);
        }

        ~EpmService()
        {
            Dispose(false);
        }
        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    pool.ReturnConnection(transport);
                }
                disposed = true;
            }

        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
