﻿
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Brilliantech.Packaging.Data;
using System.Collections.Generic;

namespace TestProject
{
    
    
    /// <summary>
    ///这是 PackProcessTest 的测试类，旨在
    ///包含所有 PackProcessTest 单元测试
    ///</summary>
    [TestClass()]
    public class PackProcessTest
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


        ///// <summary>
        /////GetItemsByPackageId 的测试
        /////</summary>
        //[TestMethod()]
        //public void GetItemsByPackageIdTest()
        //{
        //    IPackProcess target = new PackProcess(); // TODO: 初始化为适当的值
        //    string pId = "P120813083031247"; // TODO: 初始化为适当的值
        //    List<PackageItem> actual;
        //    actual = target.GetItemsByPackageId(pId);
        //    Assert.IsNotNull (actual);
        //    Assert.Equals(actual.Count, 18);
        //}

        ///// <summary>
        /////GetPackageStatus 的测试
        /////</summary>
        //[TestMethod()]
        //public void GetPackageStatusTest()
        //{
        //    IPackProcess target = new PackProcess(); // TODO: 初始化为适当的值
        //    List<EnumObject> actual;
        //    actual = target.GetPackageStatus();
        //    Assert.IsNotNull(actual);
        //    Assert.AreEqual(actual.Count, 19);
        //}
    }
}
