using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sledgehammer.KTesting.Tests
{
    [TestClass()]
    public class PairSearchTests
    {
        [TestMethod()]
        public void SearchForTest()
        {
            PairSearch ps = new PairSearch();
            ps.Items = new ulong[] { 7, 6, 5, 4, 3, 2, 1, 0, 1, 2, 3, 4, 5, 6, 7 };
            ulong x = 6;
            IEnumerable<ulong> res = ps.SearchFor(x);
            Assert.IsNotNull(res, "Пустой набор вместо полного.");
            // проверим правильность просто по числу элементов, хотя это неправильно :)
            Assert.AreEqual(4, res.Count(), "Число элементов результата не верно.");

            Debug.Print("Выводим пары составляющие в сумме {0}:", x);
            foreach (ulong i in res)
            {
                Debug.WriteLine("{0} {1}", i, x - i);
            }
        }
    }
}