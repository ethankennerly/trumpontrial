using FineGameDesign.FallacyRecognition;
using NUnit.Framework;
using UnityEngine;

using Debug = UnityEngine.Debug;

namespace FineGameDesign.Tests.FallacyRecognition
{
    public static class EvaluatorListerBridgeTests
    {
        [Test]
        public static void Answer_TwoArguments_EndsSession()
        {
            EvaluatorListerBridge bridge = new EvaluatorListerBridge();
            bridge.Evaluator = new ArgumentEvaluator();
            bridge.Lister = new FallacyLister();
            bridge.AddListeners();
            OptionDifficulty difficulty = bridge.Lister.Difficulty;
            difficulty.addDistractorsPerCorrect = 1;
            bridge.Lister.Difficulty = difficulty;
            bridge.Lister.PopulateFallacies(FallacyParserTests.ParseFromPrefab());
            bridge.Evaluator.Arguments = ArgumentParserTests.ParseFromPrefab();
            bridge.Evaluator.ConfigureRange(0, 2);

            bridge.Evaluator.StartArguments();
            Assert.AreEqual(0, bridge.Evaluator.Range.current,
                "Current argument index");
            AssertAnswerCorrectIncrementsNumDistractors(bridge);

            bridge.Evaluator.NextArgument();
            Assert.AreEqual(1, bridge.Evaluator.Range.current,
                "Current argument index");
            AssertAnswerWrongDecrementsNumDistractors(bridge);

            bridge.Evaluator.NextArgument();
            Assert.AreEqual(2, bridge.Evaluator.Range.current,
                "Current argument index");
            Assert.IsTrue(bridge.Evaluator.Range.ended,
                "Session ended");
        }

        private static void AssertAnswerCorrectIncrementsNumDistractors(EvaluatorListerBridge bridge)
        {
            string correctFallacyText = bridge.Lister.Difficulty.requiredFallacyText;
            Assert.IsNotNull(correctFallacyText,
                "correct fallacy text. Was lister populated?");
            Assert.AreEqual(1, bridge.Lister.Difficulty.numDistractors,
                "Number of distractors before correct.");
            bool correct = bridge.Evaluator.EvaluateFallacy(correctFallacyText);
            Assert.IsTrue(correct,
                "correctFallacyText=" + correctFallacyText);
            Assert.AreEqual(2, bridge.Lister.Difficulty.numDistractors,
                "Number of distractors after correct.");
        }

        private static void AssertAnswerWrongDecrementsNumDistractors(EvaluatorListerBridge bridge)
        {
            string correctFallacyText = bridge.Lister.Difficulty.requiredFallacyText;
            Assert.IsNotNull(correctFallacyText,
                "correct fallacy text. Was lister populated?");
            string wrongFallacyText = correctFallacyText;
            foreach (Fallacy option in bridge.Lister.Difficulty.options)
            {
                if (option.optionText != correctFallacyText)
                {
                    wrongFallacyText = option.optionText;
                }
            }
            Assert.AreNotEqual(wrongFallacyText, correctFallacyText);

            Assert.AreEqual(2, bridge.Lister.Difficulty.numDistractors,
                "Number of distractors before wrong.");
            bool correct = bridge.Evaluator.EvaluateFallacy(wrongFallacyText);
            Assert.IsFalse(correct,
                "correctFallacyText=" + correctFallacyText);
            Assert.AreEqual(1, bridge.Lister.Difficulty.numDistractors,
                "Number of distractors after wrong.");
        }
    }
}
