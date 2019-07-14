using FineGameDesign.Animation;
using FineGameDesign.UI;
using System;
using TMPro;
using UnityEngine;

namespace FineGameDesign.FallacyRecognition
{
    [Serializable]
    public struct ArgumentRange
    {
        public int start;
        public int current;
        public int end;
        public int length;
        public bool ended;
    }

    /// <summary>
    /// Parses CSV into arguments.
    /// Tests:
    /// <see cref="FineGameDesign.Tests.FallacyRecognition.ArgumentEvaluatorTests"/>
    /// </summary>
    public sealed class ArgumentEvaluator
    {
        public delegate void Populate(Argument argument, ArgumentRange range);
        public event Populate OnPopulated;

        public delegate void Evaluate(bool correct);
        public event Evaluate OnEvaluated;

        public delegate void EndSession();
        public event EndSession OnSessionEnded;

        private bool m_Correct;

        private ArgumentRange m_ArgumentRange;
        public ArgumentRange Range
        {
            get { return m_ArgumentRange; }
        }

        private Argument[] m_Arguments;
        public Argument[] Arguments
        {
            get { return m_Arguments; }
            set { m_Arguments = value; }
        }

        public Argument CurrentArgument
        {
            get
            {
                if (m_ArgumentRange.current >= m_Arguments.Length)
                {
                    return default;
                }
                return m_Arguments[m_ArgumentRange.current];
            }
        }

        public void ConfigureRange(int start, int end)
        {
            m_ArgumentRange.start = start;
            m_ArgumentRange.end = end;
            m_ArgumentRange.length = end - start;
            m_ArgumentRange.current = start - 1;
        }

        public int NumArgumentsInRange()
        {
            return m_ArgumentRange.length;
        }

        public void StartArguments()
        {
            m_ArgumentRange.current = m_ArgumentRange.start - 1;
            m_ArgumentRange.ended = false;
            NextArgument();
        }

        public void NextArgument()
        {
            m_ArgumentRange.current++;
            m_ArgumentRange.ended = m_ArgumentRange.current >= m_ArgumentRange.end;
            if (m_ArgumentRange.ended)
            {
                if (OnSessionEnded != null)
                {
                    OnSessionEnded();
                }
                return;
            }

            if (OnPopulated != null)
            {
                OnPopulated.Invoke(CurrentArgument, m_ArgumentRange);
            }
        }

        public bool EvaluateFallacy(string fallacyOptionText)
        {
            Argument argument = m_Arguments[m_ArgumentRange.current];
            m_Correct = fallacyOptionText == argument.correctFallacyOptionText;

            if (OnEvaluated != null)
            {
                OnEvaluated.Invoke(m_Correct);
            }

            return m_Correct;
        }
    }
}
