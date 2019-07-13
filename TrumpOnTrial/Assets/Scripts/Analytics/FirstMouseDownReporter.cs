using UnityEngine;
using UnityEngine.Analytics;

namespace FineGameDesign.Analytics
{
    [HelpURL("https://docs.unity3d.com/Manual/UnityAnalyticsStandardEvents.html")]
    public sealed class FirstMouseDownReporter : MonoBehaviour
    {
        [Header("Logs all analytics events.")]
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

        private void Update()
        {
            UpdateFirstMouseDown();
        }

        private void UpdateFirstMouseDown()
        {
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }

            AnalyticsEvent.FirstInteraction();

            enabled = false;
        }
    }
}
