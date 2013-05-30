using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrift.Transport;
using System.Threading;

namespace Brilliantech.Packaging.EpmIntegration.EpmServiceProvider
{
    public class ConnectionProvider : IConnectionProvider
    {
        private static Stack<TTransport> pool { get; set; }
        private static AutoResetEvent resetEvent;
        private static volatile int idelCount = 0;
        private static object locker = new object();
        public ConnectionProvider()
        {
            CreateResetEvent();
            CreatePool();
        }
        public TTransport GetConnection()
        {
            lock (locker) {
                TTransport transport;
                if (idelCount == 0) {
                    if (pool.Count == Conf.TMaxActive)
                        resetEvent.WaitOne();
                    else
                        PushToPool(CreateInstance());
                }
                transport = pool.Pop();
                if (--idelCount < Conf.TMinIdel && pool.Count() < Conf.TMaxActive)
                    PushToPool(CreateInstance());
                if (Conf.TValidateOnGet)
                    ValidateInstance(transport);
                return transport;
            }
        }

        public void ReturnConnection(TTransport transport)
        {
            lock (locker) {
                if (idelCount == Conf.TMaxIdel)
                    DestoryInstance(transport);
                else
                {
                    if (Conf.TValidateOnReturn)
                        ValidateInstance(transport);
                    PushToPool(transport);
                    resetEvent.Set();
                }
            }
        }
        private void CreateResetEvent()
        {
            lock (locker)
            {
                if (resetEvent == null)
                    resetEvent = new AutoResetEvent(false);
            }
        }
        private void CreatePool()
        {
            lock (locker)
            {
                if (pool == null)
                    pool = new Stack<TTransport>();
            }
        }
        private void PushToPool(TTransport transport)
        {
            pool.Push(transport);
            idelCount++;
        }
        private TTransport CreateInstance()
        {
            TTransport transport = new TFramedTransport(new TSocket(Conf.TSeriviceHost, Conf.TServicePort));
            transport.Open();
            return transport;
        }
        private void ValidateInstance(TTransport instance)
        {
            if (!instance.IsOpen)
            {
                instance.Open();
            }
        }
        private void DestoryInstance(TTransport instance)
        {
            if (instance.IsOpen)
            {
                instance.Close();
            }
            instance.Flush();
            instance.Dispose();
        }
    }
}
