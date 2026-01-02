using PingPong.Scripts.Global.Services;
using UnityEngine;

namespace PingPong.Scripts.Global.UI
{
    public interface ISceneUI : ISceneService
    {
        TDialog ShowDialog<TDialog>() where TDialog : SceneDialog;
        TDialog SwitchScreen<TDialog>() where TDialog : SceneDialog;
        public void HideDialog<TDialog>() where TDialog : SceneDialog;
        public void HideAllDialogs();
    }
}