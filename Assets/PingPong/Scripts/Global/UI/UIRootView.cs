using UnityEngine;

namespace PingPong.Scripts.Global.UI
{
    public class UIRootView : MonoBehaviour
    {
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private Transform _sceneUI;

        public void ShowLoadingScreen() => _loadingScreen.Show();
        public void HideLoadingScreen() => _loadingScreen.Hide();

        public void AttachSceneUI(GameObject sceneUI)
        {
            ClearSceneUI();
            sceneUI.transform.SetParent(_sceneUI, false);
            var rt = sceneUI.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            sceneUI.transform.localScale = Vector3.one;
        }

        private void ClearSceneUI()
        {
            var childCount = _sceneUI.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Destroy(_sceneUI.GetChild(i).gameObject);
            }
        }
    }
}
