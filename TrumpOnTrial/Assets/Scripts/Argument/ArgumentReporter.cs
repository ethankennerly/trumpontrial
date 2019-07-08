using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace FineGameDesign.Argument
{
    [HelpURL("https://docs.unity3d.com/Manual/UnityAnalyticsStandardEvents.html")]
    public sealed class ArgumentReporter : MonoBehaviour
    {
        private ArgumentViewer.Evaluate m_OnArgumentEvaluated;
        private ArgumentViewer.Evaluate OnArgumentEvaluated
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

        private ArgumentViewer.Populate m_OnArgumentPopulated;
        private ArgumentViewer.Populate OnArgumentPopulated
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
            ArgumentViewer.OnPopulated -= OnArgumentPopulated;
            ArgumentViewer.OnPopulated += OnArgumentPopulated;
            ArgumentViewer.OnEvaluated -= OnArgumentEvaluated;
            ArgumentViewer.OnEvaluated += OnArgumentEvaluated;
        }

        private void OnDisable()
        {
            ArgumentViewer.OnPopulated -= OnArgumentPopulated;
            ArgumentViewer.OnEvaluated -= OnArgumentEvaluated;
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
