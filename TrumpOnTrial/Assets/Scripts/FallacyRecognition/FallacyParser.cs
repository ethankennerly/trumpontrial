using FineGameDesign.UI;
using FineGameDesign.Utils;
using System;
using UnityEngine;

namespace FineGameDesign.FallacyRecognition
{
    public sealed class FallacyParser : MonoBehaviour, IStringArray
    {
        [SerializeField]
        private TextAsset m_FallaciesCsv = default;

        [SerializeField]
        private FallaciesTable m_Table = default;

        public Fallacy[] Fallacies
        {
            get { return m_Table.fallacies; }
        }

        public void ParseFallaciesOnce()
        {
            m_Table.ParseCsv(m_FallaciesCsv.text);
        }

        private string[] m_Strings;
        public string[] Strings
        {
            get
            {
                if (m_Strings == null)
                {
                    ParseFallaciesOnce();
                    int numFallacies = m_Table.fallacies.Length;
                    m_Strings = new string[numFallacies];
                    for (int fallacyIndex = 0; fallacyIndex < numFallacies; ++fallacyIndex)
                    {
                        m_Strings[fallacyIndex] = m_Table.fallacies[fallacyIndex].optionText;
                    }
                }
                return m_Strings;
            }
        }
    }
}
