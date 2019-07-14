using FineGameDesign.Utils;
using System;
using UnityEngine;

namespace FineGameDesign.Argument
{
    public sealed class FallacyParser : MonoBehaviour
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
    }
}
