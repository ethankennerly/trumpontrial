using FineGameDesign.Argument;
using NUnit.Framework;

// Disambiguates struct from namespace.
using ArgRow = FineGameDesign.Argument.Argument;

namespace FineGameDesign.Tests.Argument
{
    public static class ArgumentsTableTests
    {
        [Test]
        public static void ParseCsv_TwoRows_CreatesOneArgument()
        {
            ArgumentsTable acTable = new ArgumentsTable();
            acTable.argumentTextColumnName = "A";
            acTable.correctFallacyOptionTextColumnName = "C";

            AssertOneArgument(ref acTable, "A\tC\nA1\tC1", "A1", "C1");
        }

        private static void AssertOneArgument(
            ref ArgumentsTable table, string csvText,
            string expectedArgumentText,
            string expectedCorrectFallacyOptionText)
        {
            table.ParseCsv(csvText);
            ArgRow[] expectedArguments = new ArgRow[1]
            {
                new ArgRow()
                {
                    argumentText = expectedArgumentText,
                    correctFallacyOptionText = expectedCorrectFallacyOptionText
                }
            };

            Assert.AreEqual(expectedArguments, table.arguments);
        }

        [Test]
        public static void ParseCsv_EqualText_CachesNumParses()
        {
            ArgumentsTable acTable = new ArgumentsTable();
            acTable.argumentTextColumnName = "A";
            acTable.correctFallacyOptionTextColumnName = "C";
            Assert.AreEqual(0, acTable.NumParses);

            AssertOneArgument(ref acTable, "A\tC\nA1\tC1", "A1", "C1");
            Assert.AreEqual(1, acTable.NumParses);

            AssertOneArgument(ref acTable, "A\tC\nA1\tC1", "A1", "C1");
            AssertOneArgument(ref acTable, "A\tC\nA1\tC1", "A1", "C1");
            Assert.AreEqual(1, acTable.NumParses,
                "Same string O1");

            AssertOneArgument(ref acTable, "A\tC\nA2\tC2", "A2", "C2");
            AssertOneArgument(ref acTable, "A\tC\nA2\tC2", "A2", "C2");
            Assert.AreEqual(2, acTable.NumParses,
                "Different string A2,C2");

            AssertOneArgument(ref acTable, "A\tC\nA2\tC2", "A2", "C2");
            AssertOneArgument(ref acTable, "A\tC\nA2\tC2", "A2", "C2");
            Assert.AreEqual(2, acTable.NumParses,
                "Same string A2,C2");
        }

        [Test]
        public static void ParseCsv_MissingColumn_ThrowsException()
        {
            ArgumentsTable acTable = new ArgumentsTable();
            acTable.argumentTextColumnName = "A";
            acTable.correctFallacyOptionTextColumnName = "C";

            Assert.Throws(typeof(System.InvalidOperationException),
                () => acTable.ParseCsv("A\tC\nA1\nC1"))
            ;
        }
    }
}
