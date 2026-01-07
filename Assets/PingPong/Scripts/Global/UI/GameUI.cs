using PingPong.Scripts.Global.AssetManagement;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.GameMusicPlayer;
using UnityEngine;

namespace PingPong.Scripts.Global.UI
{
    public class GameUI : MonoBehaviour, IGameUI
    {
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private Transform _sceneUI;

        private const string MUSIC_POPUP_PATH = "Game/UI/MusicPopUp/MusicPopUp"; 
        
        public void ShowLoadingScreen() => _loadingScreen.Show();
        public void HideLoadingScreen() => _loadingScreen.Hide();

        public void AttachSceneUI(GameObject sceneUI)
        {
            ClearSceneUI();
            sceneUI.transform.SetParent(_sceneUI, false);
            FixUiTransform(sceneUI);
        }

        public void ShowMusicPopUp(MusicTrack track)
        {
            var assetProvider = ProjectServices.Container.Get<IAssetProvider>();
            var musicPopUp = assetProvider.Instantiate(MUSIC_POPUP_PATH);
            musicPopUp.transform.SetParent(this.transform);
            FixUiTransform(musicPopUp);
            musicPopUp.GetComponent<MusicPopup>().ShowPopUp(track);
        }

        private void ClearSceneUI()
        {
            var childCount = _sceneUI.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Destroy(_sceneUI.GetChild(i).gameObject);
            }
        }

        private void FixUiTransform(GameObject UIObject)
        {
            var rt = UIObject.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            UIObject.transform.localScale = Vector3.one;
        }
    }
}
