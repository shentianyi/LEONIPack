using Brilliantech.DatahouseService.ServiceProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Brilliantech.BaseClassLib.Util; 

namespace TestProject
{
    
    
    /// <summary>
    ///这是 ServicerTest 的测试类，旨在
    ///包含所有 ServicerTest 单元测试
    ///</summary>
    [TestClass()]
    public class ServicerTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///AddProductPack 的测试8
        ///</summary>
        [TestMethod()]
        public void AddProductPackTest()
        {
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("entityId", "RBA4");
                data.Add("packTime",  Brilliantech.BaseClassLib.Util.TimeUtil.GetMilliseconds(DateTime.Now).ToString());
                data.Add("productNr", Guid.NewGuid().ToString());
                data.Add("partId", "91G9067");
               Brilliantech.BaseClassLib.Util.LogUtil.Logger.Error(data);
                Servicer service = new Servicer();
                service.AddProductPack(data);
            }
            catch (Exception e) {
                LogUtil.Logger.Error(e.Message);
            }
        }
    }
}
