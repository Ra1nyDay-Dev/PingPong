using System;
using System.Collections.Generic;
using PingPong.Scripts.Global.AssetManagement;
using PingPong.Scripts.Global.Services;
using UnityEngine;

namespace PingPong.Scripts.Global.UI
{
    public abstract class SceneUI : MonoBehaviour, ISceneUI
    {
        protected Dictionary<Type, string> _sceneDialogsPrefabs;  // <DialogClass, PrefabPath>
        private readonly List<SceneDialog> _sceneDialogs = new();
        private SceneDialog _currentScreen;
        
        public TDialog ShowDialog<TDialog>() where TDialog : SceneDialog
        {
            if (!_sceneDialogsPrefabs.TryGetValue(typeof(TDialog), out string prefabPath))
                throw new Exception($"Cant find prefab type of {typeof(TDialog)} in scene dialogs dictionary");
            
            var assetProvider = ProjectServices.Container.Get<IAssetProvider>();
            
            GameObject dialogObject = assetProvider.Instantiate(prefabPath);
            dialogObject.transform.SetParent(this.transform);
            FixDialogTransform(dialogObject);
            TDialog dialog = dialogObject.GetComponent<TDialog>();
            dialog.Construct(this);
            _sceneDialogs.Add(dialog);
            
            return dialog;
        }

        public void HideDialog<TDialog>() where TDialog : SceneDialog
        {
            var dialogsToRemove = _sceneDialogs.FindAll(x => x is TDialog);
            _sceneDialogs.RemoveAll(x => x is TDialog);
            
            foreach (var dialog in dialogsToRemove) 
                Destroy(dialog.gameObject);
        }

        public void HideAllDialogs()
        {
            foreach (var dialog in _sceneDialogs) 
                Destroy(dialog.gameObject);
            
            _sceneDialogs.Clear();
        }

        public TScreen SwitchScreen<TScreen>() where TScreen : SceneDialog
        {
            HideAllDialogs();
            var screen = ShowDialog<TScreen>();
            _currentScreen = screen;

            return screen;
        }
        
        private void FixDialogTransform(GameObject dialog)
        {
            var rt = dialog.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            dialog.transform.localScale = Vector3.one;
        }
    }
}