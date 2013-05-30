//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using Brilliantech.Packaging.Data;
//using Brilliantech.Packaging.EpmIntegration;
//using Brilliantech.Packaging.EpmIntegration.Util;

//namespace TestProject
//{
    
    
//    /// <summary>
//    ///这是 IntegrationTest 的测试类，旨在
//    ///包含所有 IntegrationTest 单元测试
//    ///</summary>
//    [TestClass()]
//    public class IntegrationTest
//    {


//        private TestContext testContextInstance;

//        /// <summary>
//        ///获取或设置测试上下文，上下文提供
//        ///有关当前测试运行及其功能的信息。
//        ///</summary>
//        public TestContext TestContext
//        {
//            get
//            {
//                return testContextInstance;
//            }
//            set
//            {
//                testContextInstance = value;
//            }
//        }

//        #region 附加测试特性
//        // 
//        //编写测试时，还可使用以下特性:
//        //
//        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
//        //[ClassInitialize()]
//        //public static void MyClassInitialize(TestContext testContext)
//        //{
//        //}
//        //
//        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
//        //[ClassCleanup()]
//        //public static void MyClassCleanup()
//        //{
//        //}
//        //
//        //使用 TestInitialize 在运行每个测试前先运行代码
//        //[TestInitialize()]
//        //public void MyTestInitialize()
//        //{
//        //}
//        //
//        //使用 TestCleanup 在运行完每个测试后运行代码
//        //[TestCleanup()]
//        //public void MyTestCleanup()
//        //{
//        //}
//        //
//        #endregion


//        /// <summary>
//        ///Integration 构造函数 的测试
//        ///</summary>
//        [TestMethod()]
//        public void IntegrationConstructorTest()
//        {
//            Integration target = new Integration();
//            Assert.Inconclusive("TODO: 实现用来验证目标的代码");
//        }

//        /// <summary>
//        ///log 的测试
//        ///</summary>
//        [TestMethod()]
//        public void logTest()
//        {
//            Integration target = new Integration(); // TODO: 初始化为适当的值
//            string msg = string.Empty; // TODO: 初始化为适当的值
//            DateTime time = new DateTime(); // TODO: 初始化为适当的值
//            string exceptionStr = string.Empty; // TODO: 初始化为适当的值
//            target.log(msg, time, exceptionStr);
//            Assert.Inconclusive("无法验证不返回值的方法。");
//        }

//        /// <summary>
//        ///notifyEpm 的测试
//        ///</summary>
//        [TestMethod()]
//        public void notifyEpmTest()
//        {
//            try
//            {
//                for (int i = 0; i < 100; i++)
//                {                    LogUtil.Logger.Error("************************");
//                    //Integration target = new Integration(); // TODO: 初始化为适当的值
//                    //Guid guid = Guid.NewGuid();
//                    //SinglePackage pack = new SinglePackage() { partNr = "91G004702" }; // TODO: 初始化为适当的值
//                    //PackageItem newItem = new PackageItem() { itemUid = guid }; // TODO: 初始化为适当的值
//                    //TestContext.WriteLine(newItem.itemUid.ToString());
//                    //string productionline = "MB"; // TODO: 初始化为适当的值
//                    //target.notifyEpm(pack, newItem, productionline);
//                }            }
//            catch (Exception e)
//            {
//                TestContext.WriteLine(e.Message);
//            }
//        }
//    }
//}
