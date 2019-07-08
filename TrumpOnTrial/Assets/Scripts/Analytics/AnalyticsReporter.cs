using UnityEngine;
using UnityEngine.Analytics;

namespace FineGameDesign.Analytics
{
    [HelpURL("https://docs.unity3d.com/Manual/UnityAnalyticsStandardEvents.html")]
    public sealed class AnalyticsReporter : MonoBehaviour
    {
        [Header("Log if DEBUG_ANALYTICS_STANDARD_EVENTS")]
        [SerializeField]
        private bool m_DebugMode = false;

        private void OnValidate()
        {
            AnalyticsEvent.debugMode = m_DebugMode;
        }

        private void Awake()
        {
            AnalyticsEvent.debugMode = m_DebugMode;
        }
    }
}
