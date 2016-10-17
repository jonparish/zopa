using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zopalib;

namespace zopalibtests
{
    [TestClass]
    public class LoanTests
    {
        [TestMethod]
        public void TestLoanCalculation()
        {
            Loan l = new Loan("Alice", (float)0.07, 1000, 36);
            l.CalculatePayments();

            Assert.AreEqual("Alice", l.Lender, "Lender check failed");
            Assert.AreEqual((float)0.07, l.Rate, "Rate check failed");
            Assert.AreEqual(1000, l.LoanAmount, "LoanAmount check failed");
            Assert.AreEqual("30.88", l.Monthly, "MonthlyPayments check failed");
            Assert.AreEqual("1,111.57", l.Total, "TotalRepayment check failed");
        }
    }
}
