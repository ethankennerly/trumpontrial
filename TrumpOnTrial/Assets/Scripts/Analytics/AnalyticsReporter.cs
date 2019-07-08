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

        private bool m_FirstMouseDownReported = false;

        private void OnValidate()
        {
            AnalyticsEvent.debugMode = m_DebugMode;
        }

        private void Awake()
        {
            AnalyticsEvent.debugMode = m_DebugMode;
        }

        private void Update()
        {
            UpdateFirstMouseDown();
        }

        private void UpdateFirstMouseDown()
        {
            if (m_FirstMouseDownReported)
            {
                return;
            }
            
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }

            m_FirstMouseDownReported = true;

            AnalyticsEvent.FirstInteraction();
        }
    }
}
