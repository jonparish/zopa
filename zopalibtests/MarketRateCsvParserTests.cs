using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zopalib;

namespace zopalibtests
{
    [TestClass]
    public class MarketRateCsvParserTests
    {
        [TestMethod]
        public void TestHeadersValid()
        {
            Dictionary<MarketRateCsvParser.MarketDataFields, int> fieldMapping;
            MarketRateCsvParser parser = new MarketRateCsvParser();

            string data = "Lender,Rate,Available";
            fieldMapping = parser.ParseHeader(data);
            Assert.AreEqual(2, fieldMapping[MarketRateCsvParser.MarketDataFields.Available], "Available check failed");
            Assert.AreEqual(1, fieldMapping[MarketRateCsvParser.MarketDataFields.Rate], "Rate check failed");
            Assert.AreEqual(0, fieldMapping[MarketRateCsvParser.MarketDataFields.Lender], "Lender check failed");

            data = "Available,Lender,Rate";
            fieldMapping = parser.ParseHeader(data);
            Assert.AreEqual(0, fieldMapping[MarketRateCsvParser.MarketDataFields.Available], "Available check failed");
            Assert.AreEqual(2, fieldMapping[MarketRateCsvParser.MarketDataFields.Rate], "Rate check failed");
            Assert.AreEqual(1, fieldMapping[MarketRateCsvParser.MarketDataFields.Lender], "Lender check failed");

            data = "Rate,Lender,Available";
            fieldMapping = parser.ParseHeader(data);
            Assert.AreEqual(2, fieldMapping[MarketRateCsvParser.MarketDataFields.Available], "Available check failed");
            Assert.AreEqual(0, fieldMapping[MarketRateCsvParser.MarketDataFields.Rate], "Rate check failed");
            Assert.AreEqual(1, fieldMapping[MarketRateCsvParser.MarketDataFields.Lender], "Lender check failed");
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestHeadersInvalid()
        {
            Dictionary<MarketRateCsvParser.MarketDataFields, int> fieldMapping;
            MarketRateCsvParser parser = new MarketRateCsvParser();

            string data = "Lender,Available";
            fieldMapping = parser.ParseHeader(data);
        }

        [TestMethod]
        public void TestParse()
        {
            List<MarketRate> rates;
            MarketRateCsvParser parser = new MarketRateCsvParser();

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("Lender,Rate,Available");
            writer.WriteLine("Alice,0.05,1000");
            writer.WriteLine("Bob,0.01,500");
            writer.Flush();
            stream.Position = 0;

            rates = parser.Parse(new StreamReader(stream));

            Assert.AreEqual(rates.Count, 2);

            MarketRate rate = rates[0];
            Assert.AreEqual("Alice", rate.Lender, "Lender check failed");
            Assert.AreEqual((float)0.05, rate.Rate, "Rate check failed");
            Assert.AreEqual(1000, rate.AmountAvailable, "Amount check failed");

            rate = rates[1];
            Assert.AreEqual("Bob", rate.Lender, "Lender check failed");
            Assert.AreEqual((float)0.01, rate.Rate, "Rate check failed");
            Assert.AreEqual(500, rate.AmountAvailable, "Amount check failed");
        }
    }
}
