using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace FineGameDesign.Argument
{
    /// <summary>
    /// Only reports level index.
    /// Otherwise, Unity Tech requires a Pro license to access custom data.
    /// </summary>
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

        private int m_LevelIndex;

        private void ReportLevelStart(Argument argumentNotUsed, ArgumentRange range)
        {
            m_LevelIndex = range.current;
            AnalyticsResult result = AnalyticsEvent.LevelStart(range.current);
            Debug.Assert(result == AnalyticsResult.Ok,
                "ReportLevel: result=" + result +
                " levelIndex=" + range.current,
                context: this
            );
        }

        private void ReportLevelEnd(bool levelComplete)
        {
            AnalyticsResult result;
            if (!levelComplete)
            {
                result = AnalyticsEvent.LevelFail(m_LevelIndex);
            }
            else
            {
                result = AnalyticsEvent.LevelComplete(m_LevelIndex);
            }
            Debug.Assert(result == AnalyticsResult.Ok,
                "ReportLevel: result=" + result +
                " levelIndex=" + m_LevelIndex +
                " levelComplete=" + levelComplete,
                context: this
            );
        }
    }
}
