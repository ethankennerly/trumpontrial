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

        private void Awake()
        {
            m_Parser.ParseArguments();
            PopulateText(m_Parser.Arguments[0], m_ArgumentView);
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
