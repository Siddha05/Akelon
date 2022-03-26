using Microsoft.VisualStudio.TestTools.UnitTesting;
using Akelon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akelon.Tests
{
    [TestClass]
    public class ProgramTests
    {
        private Program p = new Program();
        
        [DataTestMethod()]
        [DataRow(10000, "1000;2000;3000;5000;8000;5000", "416,67;833,33;1250;2083,33;3333,33;2083,34")]
        [DataRow(2000,"300;200;1000;300;700;500", "200;133,33;666,67;200;466,67;333,33")]
        public void SumDistributePropTest(double val, string s, string res)
        {
            var r = p.SumDistribute(val, s, "ПРОП");
            Assert.AreEqual(res, r);
        }

        [DataTestMethod()]
        [DataRow(10000, "1000;2000;3000;5000;8000;5000", "1000;2000;3000;4000;0;0")]
        [DataRow(2000, "300;200;1000;300;700;500", "300;200;1000;300;200;0")]
        public void SumDistributeFirstTest(double val, string s, string res)
        {
            var r = p.SumDistribute(val, s, "ПЕРВ");
            Assert.AreEqual(res, r);
        }

        [DataTestMethod()]
        [DataRow(10000, "1000;2000;3000;5000;8000;5000", "0;0;0;0;5000;5000")]
        [DataRow(2000, "300;200;1000;300;700;500", "0;0;500;300;700;500")]
        public void SumDistributeLastTest(double val, string s, string res)
        {
            var r = p.SumDistribute(val, s, "ПОСЛ");
            Assert.AreEqual(res, r);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNullExceptionTest()
        {
            var r = p.SumDistribute(300, null, "ПОСЛ");
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void ArgumentExceptionTest()
        {
            var r = p.SumDistribute(300, "300;200", "ccc");
        }
    }
}