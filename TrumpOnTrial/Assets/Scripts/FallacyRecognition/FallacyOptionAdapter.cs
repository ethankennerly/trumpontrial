using FineGameDesign.UI;
using UnityEngine;

namespace FineGameDesign.FallacyRecognition
{
    public sealed class FallacyOptionAdapter : MonoBehaviour
    {
        [SerializeField]
        private FallacyParser m_Parser = default;

        [SerializeField]
        private TextOptionScroller m_Scroller = default;

        private void Start()
        {
            m_Scroller.Lister.PopulatePossibleTexts(m_Parser.Strings);
        }
    }
}

