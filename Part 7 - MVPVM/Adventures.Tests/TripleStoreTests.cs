using System;
using Adventures.Common.Utils;
using Adventures.Data.Results;
using Adventures.Data.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Adventures.Tests;

[TestClass]
public class TripleStoreTests
{
    [TestMethod]
    public void TestMethod1()
    {
        var service = new TripleStoreService();

        var data = AsyncUtil.RunSync(() =>
            service.GetDataAsync<TripleStoreResult>());

    }

}
