using System;
using TMPro;
using UnityEngine;

namespace FineGameDesign.Argument
{
    [Serializable]
    public struct ArgumentView
    {
        public TMP_Text argumentText;
    }

    public sealed class ArgumentViewer : MonoBehaviour
    {
        [SerializeField]
        private ArgumentParser m_Parser;

        [SerializeField]
        private ArgumentView m_ArgumentView;

        private int m_ArgumentIndex = -1;

        private FallacySubmitter.Submit m_NextArgumentDelegate;

        private void Awake()
        {
            m_Parser.ParseArguments();
            NextArgument();
        }

        private void OnEnable()
        {
            if (m_NextArgumentDelegate == null)
                m_NextArgumentDelegate = NextArgument;

            FallacySubmitter.OnSubmitted -= m_NextArgumentDelegate;
            FallacySubmitter.OnSubmitted += m_NextArgumentDelegate;
        }

        private void OnDisable()
        {
            FallacySubmitter.OnSubmitted -= m_NextArgumentDelegate;
        }

        private void NextArgument()
        {
            m_ArgumentIndex++;
            if (m_ArgumentIndex >= m_Parser.Arguments.Length)
                m_ArgumentIndex = 0;

            PopulateText(m_Parser.Arguments[m_ArgumentIndex], m_ArgumentView);
        }

        /// <summary>
        /// Copies each text.
        /// </summary>
        private static void PopulateText(Argument argument, ArgumentView argumentView)
        {
            argumentView.argumentText.text = argument.argumentText;
        }
    }
}
