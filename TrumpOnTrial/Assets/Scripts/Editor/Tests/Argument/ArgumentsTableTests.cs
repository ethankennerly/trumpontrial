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
            acTable.ParseCsv("A\tC\nA1\tC1");
            ArgRow[] acArguments = new ArgRow[1]
            {
                new ArgRow()
                {
                    argumentText = "A1",
                    correctFallacyOptionText = "C1"
                }
            };

            Assert.AreEqual(acArguments, acTable.arguments);
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
