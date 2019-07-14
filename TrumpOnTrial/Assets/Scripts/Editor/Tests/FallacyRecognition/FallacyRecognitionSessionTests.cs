using FineGameDesign.FallacyRecognition;
using NUnit.Framework;
using UnityEngine;

using Debug = UnityEngine.Debug;

namespace FineGameDesign.Tests.FallacyRecognition
{
    public static class FallacyRecognitionSessionTests
    {
        [Test]
        public static void Answer_TwoArguments_EndsSession()
        {
            FallacyRecognitionSession session = new FallacyRecognitionSession();
            session.Evaluator.Arguments = ArgumentParserTests.ParseFromPrefab();
            session.Lister.PopulateFallacies(FallacyParserTests.ParseFromPrefab());

            session.Evaluator.ConfigureRange(0, 2);
            session.Evaluator.StartArguments();
            AssertAnswerCorrectIncrementsNumDistractors(session);
            AssertAnswerWrongDecrementsNumDistractors(session);
            Assert.IsTrue(session.Evaluator.Range.ended);
        }

        private static void AssertAnswerCorrectIncrementsNumDistractors(FallacyRecognitionSession session)
        {
            string correctFallacyText = session.Lister.Difficulty.requiredFallacyText;
            bool correct = session.Evaluator.EvaluateFallacy(correctFallacyText);
            Assert.IsTrue(correct,
                "correctFallacyText=" + correctFallacyText);
            Assert.AreEqual(3, session.Lister.Difficulty.numDistractors,
                "Number of distractors after correct.");
        }

        private static void AssertAnswerWrongDecrementsNumDistractors(FallacyRecognitionSession session)
        {
            string correctFallacyText = session.Lister.Difficulty.requiredFallacyText;
            string wrongFallacyText = correctFallacyText;
            foreach (Fallacy option in session.Lister.Difficulty.options)
            {
                if (option.optionText != correctFallacyText)
                {
                    wrongFallacyText = option.optionText;
                }
            }
            Assert.AreNotEqual(wrongFallacyText, correctFallacyText);

            bool correct = session.Evaluator.EvaluateFallacy(wrongFallacyText);
            Assert.IsFalse(correct,
                "correctFallacyText=" + correctFallacyText);
            Assert.AreEqual(2, session.Lister.Difficulty.numDistractors,
                "Number of distractors after correct.");
        }
    }
}
