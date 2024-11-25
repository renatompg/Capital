using CapitalGains.Models;
using CapitalGains.Services;

namespace CapitalGains.Tests
{
    public class CapitalGainsServiceTests
    {
        private readonly CapitalGainsService _service;

        public CapitalGainsServiceTests()
        {
            _service = new CapitalGainsService();
        }

        [Fact]
        public void TestCase1_NoTaxForBuyAndSmallSell()
        {
            var operations = new List<Operation>
            {
                new Operation { OperationType = OperationType.Buy, UnitCost = 10.00m, Quantity = 100 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 15.00m, Quantity = 50 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 15.00m, Quantity = 50 }
            };

            var results = _service.CalculateTaxes(operations);

            Assert.Collection(results,
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(0.00m, tax.Tax));
        }

        [Fact]
        public void TestCase2_TaxOnProfit()
        {
            var operations = new List<Operation>
            {
                new Operation { OperationType = OperationType.Buy, UnitCost = 10.00m, Quantity = 10000 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 20.00m, Quantity = 5000 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 5.00m, Quantity = 5000 }
            };

            var results = _service.CalculateTaxes(operations);

            Assert.Collection(results,
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(10000.00m, tax.Tax),
                tax => Assert.Equal(0.00m, tax.Tax));
        }

        [Fact]
        public void TestCase3_LossCarriedOver()
        {
            var operations = new List<Operation>
            {
                new Operation { OperationType = OperationType.Buy, UnitCost = 10.00m, Quantity = 10000 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 5.00m, Quantity = 5000 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 20.00m, Quantity = 3000 }
            };

            var results = _service.CalculateTaxes(operations);

            Assert.Collection(results,
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(1000.00m, tax.Tax));
        }

        [Fact]
        public void TestCase4_WeightedAverage_NoProfitOrLoss()
        {
            var operations = new List<Operation>
            {
                new Operation { OperationType = OperationType.Buy, UnitCost = 10.00m, Quantity = 10000 },
                new Operation { OperationType = OperationType.Buy, UnitCost = 25.00m, Quantity = 5000 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 15.00m, Quantity = 10000 }
            };

            var results = _service.CalculateTaxes(operations);

            Assert.Collection(results,
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(0.00m, tax.Tax));
        }

        [Fact]
        public void TestCase5_ProfitAfterWeightedAverage()
        {
            var operations = new List<Operation>
            {
                new Operation { OperationType = OperationType.Buy, UnitCost = 10.00m, Quantity = 10000 },
                new Operation { OperationType = OperationType.Buy, UnitCost = 25.00m, Quantity = 5000 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 15.00m, Quantity = 10000 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 25.00m, Quantity = 5000 }
            };

            var results = _service.CalculateTaxes(operations);

            Assert.Collection(results,
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(10000.00m, tax.Tax));
        }

        [Fact]
        public void TestCase6_SmallTotalValue_NoTax()
        {
            var operations = new List<Operation>
            {
                new Operation { OperationType = OperationType.Buy, UnitCost = 10.00m, Quantity = 10000 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 2.00m, Quantity = 5000 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 20.00m, Quantity = 2000 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 20.00m, Quantity = 2000 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 25.00m, Quantity = 1000 }
            };

            var results = _service.CalculateTaxes(operations);

            Assert.Collection(results,
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(3000.00m, tax.Tax));
        }

        [Fact]
        public void TestCase7_ComplexScenarioWithLossAndWeightedAverage()
        {
            var operations = new List<Operation>
            {
                new Operation { OperationType = OperationType.Buy, UnitCost = 10.00m, Quantity = 10000 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 2.00m, Quantity = 5000 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 20.00m, Quantity = 2000 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 20.00m, Quantity = 2000 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 25.00m, Quantity = 1000 },
                new Operation { OperationType = OperationType.Buy, UnitCost = 20.00m, Quantity = 10000 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 15.00m, Quantity = 5000 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 30.00m, Quantity = 4350 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 30.00m, Quantity = 650 }
            };

            var results = _service.CalculateTaxes(operations);

            Assert.Collection(results,
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(3000.00m, tax.Tax),
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(3700.00m, tax.Tax),
                tax => Assert.Equal(0.00m, tax.Tax));
        }

        [Fact]
        public void TestCase8_LargeProfitsWithMultipleTransactions()
        {
            var operations = new List<Operation>
            {
                new Operation { OperationType = OperationType.Buy, UnitCost = 10.00m, Quantity = 10000 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 50.00m, Quantity = 10000 },
                new Operation { OperationType = OperationType.Buy, UnitCost = 20.00m, Quantity = 10000 },
                new Operation { OperationType = OperationType.Sell, UnitCost = 50.00m, Quantity = 10000 }
            };

            var results = _service.CalculateTaxes(operations);

            Assert.Collection(results,
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(80000.00m, tax.Tax),
                tax => Assert.Equal(0.00m, tax.Tax),
                tax => Assert.Equal(60000.00m, tax.Tax));
        }
    }
}
