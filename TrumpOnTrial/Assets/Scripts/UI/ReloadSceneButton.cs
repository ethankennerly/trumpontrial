using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FineGameDesign.UI
{
    public sealed class ReloadSceneButton : MonoBehaviour
    {
        [SerializeField]
        private Button m_ReloadSceneButton = null;

        private void OnEnable()
        {
            m_ReloadSceneButton.onClick.RemoveListener(ReloadActiveScene);
            m_ReloadSceneButton.onClick.AddListener(ReloadActiveScene);
        }

        private void OnDisable()
        {
            m_ReloadSceneButton.onClick.RemoveListener(ReloadActiveScene);
        }

        private void ReloadActiveScene()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
        }
    }
}
