using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.UI;
using UnityEngine;

namespace PingPong.Scripts.Global
{
    public abstract class SceneEntryPoint : MonoBehaviour, ISceneEntryPoint
    {
        [SerializeField] private GameObject _sceneUIPrefab;
        
        protected GameObject _sceneUI;
        
        public virtual void Run(UIRootView uiRoot)
        {
            SceneServices.Dispose();
            _sceneUI = Instantiate(_sceneUIPrefab);
            uiRoot.AttachSceneUI(_sceneUI.gameObject);
        }
    }
}