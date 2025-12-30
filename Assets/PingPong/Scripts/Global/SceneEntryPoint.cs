using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.UI;
using UnityEngine;

namespace PingPong.Scripts.Global
{
    public abstract class SceneEntryPoint : MonoBehaviour, ISceneEntryPoint
    {
        [SerializeField] protected GameObject _sceneUIPrefab;
        
        protected GameObject _sceneUI;
        
        public virtual void Run(GameUI gameUI)
        {
            SceneServices.Dispose();
            _sceneUI = Instantiate(_sceneUIPrefab);
            gameUI.AttachSceneUI(_sceneUI.gameObject);
        }
    }
    
    public abstract class SceneEntryPoint<TSceneParams> : SceneEntryPoint, ISceneEntryPointWithParams<TSceneParams>
    {
        protected TSceneParams _sceneParams;
        
        public virtual void Run(GameUI gameUI, TSceneParams sceneParams)
        {
            SceneServices.Dispose();
            _sceneUI = Instantiate(_sceneUIPrefab);
            _sceneParams = sceneParams;
            gameUI.AttachSceneUI(_sceneUI.gameObject);
        }
    }
}