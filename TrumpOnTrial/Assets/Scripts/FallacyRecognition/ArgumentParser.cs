using FineGameDesign.Utils;
using System;
using UnityEngine;

namespace FineGameDesign.FallacyRecognition
{
    /// <summary>
    /// Humble wrapper of argument table.
    /// Argument table is easier to test.
    /// </summary>
    public sealed class ArgumentParser : MonoBehaviour
    {
        [SerializeField]
        private TextAsset m_ArgumentsCsv = default;

        [SerializeField]
        private ArgumentsTable m_Table = default;

        public int NumArguments
        {
            get { return m_Table.numArguments; }
        }

        public Argument[] Arguments
        {
            get { return m_Table.arguments; }
        }

        public void ParseArguments()
        {
            m_Table.ParseCsv(m_ArgumentsCsv.text);
        }
    }
}
