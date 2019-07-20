using FineGameDesign.UI;
using UnityEngine;

namespace FineGameDesign.FallacyRecognition
{
    public sealed class FallacyOptionAdapter : MonoBehaviour
    {
        [Header("String replacements.")]
        [SerializeField]
        private FallacyParser m_Parser = default;

        [Header("Lister possible texts replaced.")]
        [SerializeField]
        private TextOptionScroller m_Scroller = default;

        private void OnEnable()
        {
            OptionDifficulty difficulty = m_Scroller.Lister.Difficulty;
            difficulty.possibleTexts = m_Parser.Strings;
            m_Scroller.Lister.Difficulty = difficulty;
        }
    }
}

