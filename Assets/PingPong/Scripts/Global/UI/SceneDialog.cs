using UnityEngine;

namespace PingPong.Scripts.Global.UI
{
    public abstract class SceneDialog : MonoBehaviour, ISceneDialog
    {
        protected ISceneUI _sceneUI;

        public void Construct(ISceneUI sceneUI) => 
            _sceneUI = sceneUI;
    }
}