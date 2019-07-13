using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace FineGameDesign.Argument
{
    [HelpURL("https://docs.unity3d.com/Manual/UnityAnalyticsStandardEvents.html")]
    public sealed class ArgumentReporter : MonoBehaviour
    {
        [SerializeField]
        private ArgumentViewer m_ArgumentViewer = default;

        private ArgumentEvaluator.Evaluate m_OnArgumentEvaluated;
        private ArgumentEvaluator.Evaluate OnArgumentEvaluated
        {
            get
            {
                if (m_OnArgumentEvaluated == null)
                {
                    m_OnArgumentEvaluated = ReportLevelEnd;
                }
                return m_OnArgumentEvaluated;
            }
        }

        private ArgumentEvaluator.Populate m_OnArgumentPopulated;
        private ArgumentEvaluator.Populate OnArgumentPopulated
        {
            get
            {
                if (m_OnArgumentPopulated == null)
                {
                    m_OnArgumentPopulated = ReportLevelStart;
                }
                return m_OnArgumentPopulated;
            }
        }

        private void OnEnable()
        {
            m_ArgumentViewer.Evaluator.OnPopulated -= OnArgumentPopulated;
            m_ArgumentViewer.Evaluator.OnPopulated += OnArgumentPopulated;
            m_ArgumentViewer.Evaluator.OnEvaluated -= OnArgumentEvaluated;
            m_ArgumentViewer.Evaluator.OnEvaluated += OnArgumentEvaluated;
        }

        private void OnDisable()
        {
            m_ArgumentViewer.Evaluator.OnPopulated -= OnArgumentPopulated;
            m_ArgumentViewer.Evaluator.OnEvaluated -= OnArgumentEvaluated;
        }

        private const string kAnswerKey = "answer";

        private readonly Dictionary<string, object> m_CachedWrongAnswer = new Dictionary<string, object>();

        private void ReportLevelStart(int levelIndex)
        {
            AnalyticsResult result = AnalyticsEvent.LevelStart(levelIndex, m_CachedWrongAnswer);
            Debug.Assert(result == AnalyticsResult.Ok,
                "ReportLevel: result=" + result +
                " levelIndex=" + levelIndex,
                context: this
            );
        }

        private void ReportLevelEnd(int levelIndex, bool levelComplete, string answerText)
        {
            AnalyticsResult result;
            if (!levelComplete)
            {
                m_CachedWrongAnswer[kAnswerKey] = answerText;
                result = AnalyticsEvent.LevelFail(levelIndex, m_CachedWrongAnswer);
            }
            else
            {
                result = AnalyticsEvent.LevelComplete(levelIndex);
            }
            Debug.Assert(result == AnalyticsResult.Ok,
                "ReportLevel: result=" + result +
                " levelIndex=" + levelIndex +
                " levelComplete=" + levelComplete,
                context: this
            );
        }
    }
}
