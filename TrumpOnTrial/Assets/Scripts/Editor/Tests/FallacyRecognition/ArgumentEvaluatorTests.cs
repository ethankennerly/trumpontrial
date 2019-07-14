using FineGameDesign.FallacyRecognition;
using NUnit.Framework;

namespace FineGameDesign.Tests.FallacyRecognition
{
    public static class ArgumentEvaluatorTests
    {
        private static ArgumentEvaluator SetupEvaluatorColumnsNamedAC()
        {
            ArgumentsTable acTable = ArgumentsTableTests.AssertOneArgumentTableColumnsNamedAC();
            ArgumentEvaluator acEvaluator = new ArgumentEvaluator();
            acEvaluator.Arguments = acTable.arguments;
            return acEvaluator;
        }

        [Test]
        public static void EvaluateFallacy_Equal_IsTrue()
        {
            ArgumentEvaluator acEvaluator = SetupEvaluatorColumnsNamedAC();
            Assert.IsTrue(acEvaluator.EvaluateFallacy("C1"));
        }

        [Test]
        public static void EvaluateFallacy_NotEqual_IsFalse()
        {
            ArgumentEvaluator acEvaluator = SetupEvaluatorColumnsNamedAC();
            Assert.IsFalse(acEvaluator.EvaluateFallacy("A1"),
                "A1");
            Assert.IsFalse(acEvaluator.EvaluateFallacy("c1"),
                "C1");
        }
    }
}
