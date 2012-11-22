using Brilliantech.Packaging.WS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Brilliantech.Packaging.Data;
using System.Linq;

namespace TestProject
{
    
    
    /// <summary>
    ///这是 SinglePackageRepoTest 的测试类，旨在
    ///包含所有 SinglePackageRepoTest 单元测试
    ///</summary>
    [TestClass()]
    public class SinglePackageRepoTest
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
        ///FindSinglePackage 的测试
        ///</summary>
        [TestMethod()]
        public void FindSinglePackageTest()
        {
            PackagingDataDataContext context = new PackagingDataDataContext("D:\\vm\\DBBackup\\Minor-TSK-055\\package.sdf"); // TODO: 初始化为适当的值
            SinglePackageRepo target = new SinglePackageRepo(context); // TODO: 初始化为适当的值
            string pid = "P120825085853069"; // TODO: 初始化为适当的值
            string wrkstnr = string.Empty; // TODO: 初始化为适当的值
            string tnr = string.Empty ; // TODO: 初始化为适当的值
            string partnr = string.Empty; // TODO: 初始化为适当的值
            int status =-1; // TODO: 初始化为适当的值
            DateTime fromDate = new DateTime(1990,1,1); // TODO: 初始化为适当的值
            DateTime toDate = new DateTime(2100,1,1); // TODO: 初始化为适当的值
            System.Collections.Generic.List<FullPackageInfo > actual;
            actual = target.FindSinglePackage(pid, wrkstnr, tnr, partnr, status, fromDate, toDate);
            Assert.IsNotNull (actual);
            Assert.AreEqual(actual.Count(), 1);
         
        }

        /// <summary>
        ///GetItemsByPId 的测试
        ///</summary>
        [TestMethod()]
        public void GetItemsByPIdTest()
        {
            PackagingDataDataContext context = new PackagingDataDataContext("D:\\vm\\DBBackup\\Minor-TSK-055\\package.sdf"); // TODO: 初始化为适当的值
            SinglePackageRepo target = new SinglePackageRepo(context); // TODO: 初始化为适当的值
            string pid = "P120813083031247"; // TODO: 初始化为适当的值
            IQueryable<PackageItem> actual;
            actual = target.GetItemsByPId(pid);
            Assert.IsNotNull (actual);
            Assert.AreEqual (actual.ToList().Count, 18);
        }
    }
}
