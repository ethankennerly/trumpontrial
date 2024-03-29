using FineGameDesign.FallacyRecognition;
using FineGameDesign.UI;
using NUnit.Framework;
using UnityEngine;

using Debug = UnityEngine.Debug;

namespace FineGameDesign.Tests.FallacyRecognition
{
    public static class EvaluatorListerBridgeTests
    {
        [Test]
        public static void Answer_ThreeArguments_EndsSession()
        {
            EvaluatorListerBridge bridge = SetUpBridge();
            AssertAnswerRange_ChangesNumDistractors_And_EndsSession(bridge, 0, 3);
        }

        [Test]
        public static void Answer_FifteenArguments_EndsSession()
        {
            EvaluatorListerBridge bridge = SetUpBridge();
            AssertAnswerRange_ChangesNumDistractors_And_EndsSession(bridge, 3, 18);
            Assert.AreEqual(15, bridge.Evaluator.Range.length,
                "Range length");
        }

        [Test]
        public static void ConfigureRange_OutOfRange_Clamps()
        {
            EvaluatorListerBridge bridge = SetUpBridge();
            AssertAnswerRange_ChangesNumDistractors_And_EndsSession(bridge, 3, 99);
            Assert.AreEqual(15, bridge.Evaluator.Range.length,
                "Range length");
        }

        private static void AssertAnswerRange_ChangesNumDistractors_And_EndsSession(
            EvaluatorListerBridge bridge, int argumentStart, int argumentEnd)
        {
            bridge.Evaluator.ConfigureRange(argumentStart, argumentEnd);
            argumentStart = bridge.Evaluator.Range.start;
            argumentEnd = bridge.Evaluator.Range.end;

            bridge.Evaluator.StartArguments();
            Assert.AreEqual(argumentStart, bridge.Evaluator.Range.current,
                "Current argument index");

            bool correct = true;
            for (int argumentIndex = argumentStart; argumentIndex < argumentEnd; ++argumentIndex)
            {
                if (correct)
                {
                    AssertAnswerCorrectIncrementsNumDistractors(bridge);
                }
                else
                {
                    AssertAnswerWrongDecrementsNumDistractors(bridge);
                }

                Assert.AreEqual(argumentIndex, bridge.Evaluator.Range.current,
                    "Current argument index");
                bridge.Evaluator.NextArgument();

                correct = !correct;
            }

            Assert.AreEqual(argumentEnd, bridge.Evaluator.Range.current,
                "Current argument index after ended.");
            Assert.IsTrue(bridge.Evaluator.Range.ended,
                "Session ended");
        }

        private static EvaluatorListerBridge SetUpBridge()
        {
            EvaluatorListerBridge bridge = new EvaluatorListerBridge();
            bridge.Evaluator = new ArgumentEvaluator();
            bridge.Lister = new TextLister();
            bridge.AddListeners();
            OptionDifficulty difficulty = bridge.Lister.Difficulty;
            difficulty.addDistractorsPerCorrect = 1;
            bridge.Lister.Difficulty = difficulty;
            bridge.Lister.PopulatePossibleTexts(FallacyParserTests.ParseFromPrefab());
            bridge.Evaluator.Arguments = ArgumentParserTests.ParseFromPrefab();
            return bridge;
        }


        private static void AssertAnswerCorrectIncrementsNumDistractors(EvaluatorListerBridge bridge)
        {
            string correctFallacyText = bridge.Lister.Difficulty.requiredText;
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
            string correctFallacyText = bridge.Lister.Difficulty.requiredText;
            Assert.IsNotNull(correctFallacyText,
                "correct fallacy text. Was lister populated?");
            string wrongFallacyText = correctFallacyText;
            foreach (string optionText in bridge.Lister.Difficulty.options)
            {
                if (optionText != correctFallacyText)
                {
                    wrongFallacyText = optionText;
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
