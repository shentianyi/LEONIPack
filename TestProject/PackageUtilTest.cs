using Brilliantech.Packaging.WS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Brilliantech.Framework.WCF.Data;
using System.Threading;

namespace TestProject
{
    
    
    /// <summary>
    ///这是 PackageUtilTest 的测试类，旨在
    ///包含所有 PackageUtilTest 单元测试
    ///</summary>
    [TestClass()]
    public class PackageUtilTest
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
        ///_addItem 的测试
        ///</summary>
        [TestMethod()]
        public void _addItemTest()
        {
            for (int i = 0; i <50; i++)
            {
                PackageUtil target = new PackageUtil(); // TODO: 初始化为适当的值
                string packId = "P130517131552064"; // TODO: 初始化为适当的值
                string tnr = "aaa1049910"+i.ToString(); // TODO: 初始化为适当的值
                ServiceMessage actual;
                actual = target._addItem(packId, tnr);
                //Thread.Sleep(2000);
            }
        }
    }
}
