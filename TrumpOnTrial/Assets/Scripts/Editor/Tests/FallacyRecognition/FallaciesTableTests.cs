using FineGameDesign.FallacyRecognition;
using NUnit.Framework;

namespace FineGameDesign.Tests.FallacyRecognition
{
    public static class FallaciesTableTests
    {
        public static FallaciesTable AssertOneFallacyTableColumnNamedO()
        {
            FallaciesTable oTable = new FallaciesTable();
            oTable.optionTextColumnName = "O";

            AssertOneFallacy(ref oTable, "O\nO1", "O1");
            return oTable;
        }

        [Test]
        public static void ParseCsv_TwoRows_CreatesOneFallacy()
        {
            AssertOneFallacyTableColumnNamedO();
        }

        private static void AssertOneFallacy(
            ref FallaciesTable table, string csvText, string expectedOptionText)
        {
            table.ParseCsv(csvText);
            Fallacy[] expectedFallacies = new Fallacy[1]
            {
                new Fallacy()
                {
                    optionText = expectedOptionText
                }
            };

            Assert.AreEqual(expectedFallacies, table.fallacies);
        }

        [Test]
        public static void ParseCsv_EqualText_CachesNumParses()
        {
            FallaciesTable oTable = new FallaciesTable();
            oTable.optionTextColumnName = "O";
            Assert.AreEqual(0, oTable.NumParses);

            AssertOneFallacy(ref oTable, "O\nO1", "O1");
            Assert.AreEqual(1, oTable.NumParses);

            AssertOneFallacy(ref oTable, "O\nO1", "O1");
            AssertOneFallacy(ref oTable, "O\nO1", "O1");
            Assert.AreEqual(1, oTable.NumParses,
                "Same string O1");

            AssertOneFallacy(ref oTable, "O\nO2", "O2");
            Assert.AreEqual(2, oTable.NumParses,
                "Different string O2");

            AssertOneFallacy(ref oTable, "O\nO2", "O2");
            AssertOneFallacy(ref oTable, "O\nO2", "O2");
            Assert.AreEqual(2, oTable.NumParses,
                "Same string O2");
        }

        [Test]
        public static void ParseCsv_NoColumn_CreatesZeroFallacies()
        {
            FallaciesTable oTable = new FallaciesTable();
            oTable.optionTextColumnName = "O";
            oTable.ParseCsv("O\t\n");

            Assert.AreEqual(new Fallacy[0], oTable.fallacies);
        }
    }
}
