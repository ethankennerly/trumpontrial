using FineGameDesign.UI;
using UnityEngine;

namespace FineGameDesign.FallacyRecognition
{
    public sealed class FallacyOptionAdapter : MonoBehaviour
    {
        [SerializeField]
        private FallacyParser m_Parser = default;

        [SerializeField]
        private TextOptionViewer m_TextOptionViewer = default;

        private void Start()
        {
            m_TextOptionViewer.Lister.PopulatePossibleTexts(m_Parser.Strings);
        }
    }
}

