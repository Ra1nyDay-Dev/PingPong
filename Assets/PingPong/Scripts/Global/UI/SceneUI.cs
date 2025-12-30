using UnityEngine;

namespace PingPong.Scripts.Global.UI
{
    public abstract class SceneUI : MonoBehaviour, ISceneUI
    {
        private GameObject _currentDialog;
        
        public void ShowDialog(string dialogPath)
        {
            var dialog = Resources.Load(dialogPath) as GameObject;
            
            if (_currentDialog != null)
                Destroy(_currentDialog);
            
            _currentDialog = GameObject.Instantiate(dialog, transform);;
        }
    }
}