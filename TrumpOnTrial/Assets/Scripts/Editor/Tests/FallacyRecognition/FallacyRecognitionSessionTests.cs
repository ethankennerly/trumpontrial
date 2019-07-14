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
            OptionDifficulty difficulty = session.Lister.Difficulty;
            difficulty.addDistractorsPerCorrect = 1;
            session.Lister.Difficulty = difficulty;
            session.Lister.PopulateFallacies(FallacyParserTests.ParseFromPrefab());
            session.Evaluator.Arguments = ArgumentParserTests.ParseFromPrefab();
            session.Evaluator.ConfigureRange(0, 2);

            session.Evaluator.StartArguments();
            Assert.AreEqual(0, session.Evaluator.Range.current,
                "Current argument index");
            AssertAnswerCorrectIncrementsNumDistractors(session);

            session.Evaluator.NextArgument();
            Assert.AreEqual(1, session.Evaluator.Range.current,
                "Current argument index");
            AssertAnswerWrongDecrementsNumDistractors(session);

            session.Evaluator.NextArgument();
            Assert.AreEqual(2, session.Evaluator.Range.current,
                "Current argument index");
            Assert.IsTrue(session.Evaluator.Range.ended,
                "Session ended");
        }

        private static void AssertAnswerCorrectIncrementsNumDistractors(FallacyRecognitionSession session)
        {
            string correctFallacyText = session.Lister.Difficulty.requiredFallacyText;
            Assert.IsNotNull(correctFallacyText,
                "correct fallacy text. Was lister populated?");
            Assert.AreEqual(1, session.Lister.Difficulty.numDistractors,
                "Number of distractors before correct.");
            bool correct = session.Evaluator.EvaluateFallacy(correctFallacyText);
            Assert.IsTrue(correct,
                "correctFallacyText=" + correctFallacyText);
            Assert.AreEqual(2, session.Lister.Difficulty.numDistractors,
                "Number of distractors after correct.");
        }

        private static void AssertAnswerWrongDecrementsNumDistractors(FallacyRecognitionSession session)
        {
            string correctFallacyText = session.Lister.Difficulty.requiredFallacyText;
            Assert.IsNotNull(correctFallacyText,
                "correct fallacy text. Was lister populated?");
            string wrongFallacyText = correctFallacyText;
            foreach (Fallacy option in session.Lister.Difficulty.options)
            {
                if (option.optionText != correctFallacyText)
                {
                    wrongFallacyText = option.optionText;
                }
            }
            Assert.AreNotEqual(wrongFallacyText, correctFallacyText);

            Assert.AreEqual(2, session.Lister.Difficulty.numDistractors,
                "Number of distractors before wrong.");
            bool correct = session.Evaluator.EvaluateFallacy(wrongFallacyText);
            Assert.IsFalse(correct,
                "correctFallacyText=" + correctFallacyText);
            Assert.AreEqual(1, session.Lister.Difficulty.numDistractors,
                "Number of distractors after wrong.");
        }
    }
}
