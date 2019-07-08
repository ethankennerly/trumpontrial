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
                    m_OnArgumentEvaluated = ReportLevel;
                }
                return m_OnArgumentEvaluated;
            }
        }

        private void OnEnable()
        {
            ArgumentViewer.OnEvaluated -= OnArgumentEvaluated;
            ArgumentViewer.OnEvaluated += OnArgumentEvaluated;
        }

        private void OnDisable()
        {
            ArgumentViewer.OnEvaluated -= OnArgumentEvaluated;
        }

        private void ReportLevel(int levelIndex, bool levelComplete)
        {
            AnalyticsResult result;
            if (!levelComplete)
            {
                result = AnalyticsEvent.LevelFail(levelIndex);
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

            if (AnalyticsEvent.debugMode)
            {
                Debug.Log(
                    "ReportLevel: result=" + result +
                    " levelIndex=" + levelIndex +
                    " levelComplete=" + levelComplete,
                    context: this
                );
            }
        }
    }
}
