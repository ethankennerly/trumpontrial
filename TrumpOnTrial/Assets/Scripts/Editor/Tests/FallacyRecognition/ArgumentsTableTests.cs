using FineGameDesign.FallacyRecognition;
using NUnit.Framework;

namespace FineGameDesign.Tests.FallacyRecognition
{
    public static class ArgumentsTableTests
    {
        public static ArgumentsTable AssertOneArgumentTableColumnsNamedAC()
        {
            ArgumentsTable acTable = new ArgumentsTable();
            acTable.argumentTextColumnName = "A";
            acTable.correctFallacyOptionTextColumnName = "C";

            AssertOneArgument(ref acTable, "A\tC\nA1\tC1", "A1", "C1");
            return acTable;
        }

        [Test]
        public static void ParseCsv_TwoRows_CreatesOneArgument()
        {
            AssertOneArgumentTableColumnsNamedAC();
        }

        private static void AssertOneArgument(
            ref ArgumentsTable table, string csvText,
            string expectedArgumentText,
            string expectedCorrectFallacyOptionText)
        {
            table.ParseCsv(csvText);
            Argument[] expectedArguments = new Argument[1]
            {
                new Argument()
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
            ArgumentsTable acTable = AssertOneArgumentTableColumnsNamedAC();
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
