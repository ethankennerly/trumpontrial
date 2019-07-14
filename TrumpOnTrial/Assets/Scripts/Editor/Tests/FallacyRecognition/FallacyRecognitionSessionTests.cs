using FineGameDesign.FallacyRecognition;
using NUnit.Framework;
using UnityEngine;

using Debug = UnityEngine.Debug;

namespace FineGameDesign.Tests.FallacyRecognition
{
    public static class FallacyRecognitionSessionTests
    {
        [Test]
        public static void TODO_Answer_TwoArguments_EndsSession()
        {
            FallacyRecognitionSession session = new FallacyRecognitionSession();
            session.Evaluator.Arguments = ArgumentParserTests.ParseFromPrefab();
            session.Lister.PopulateFallacies(FallacyParserTests.ParseFromPrefab());
            AddOnOptionsChangedNextArgument(session);
            session.Evaluator.ConfigureRange(0, 2);
            session.Evaluator.StartArguments();
            AssertAnswerCorrectIncrementsNumDistractors(session);
            AssertAnswerWrongDecrementsNumDistractors(session);
            AssertSessionEnded(session);

            Assert.AreEqual("Tests implemented", "TODO");
        }

        private static void AddOnOptionsChangedNextArgument(FallacyRecognitionSession session)
        {
            session.Lister.OnOptionsChanged += (fallaciesNotUsed) =>
            {
                session.Evaluator.NextArgument();
            };
        }

        private static void AssertAnswerCorrectIncrementsNumDistractors(FallacyRecognitionSession session)
        {
            Debug.LogWarning("AssertAnswerCorrectIncrementsNumDistractors: TODO");
        }

        private static void AssertAnswerWrongDecrementsNumDistractors(FallacyRecognitionSession session)
        {
            Debug.LogWarning("AssertAnswerWrongDecrementsNumDistractors: TODO");
        }

        private static void AssertSessionEnded(FallacyRecognitionSession session)
        {
            Debug.LogWarning("AssertSessionEnded: TODO");
        }
    }
}
