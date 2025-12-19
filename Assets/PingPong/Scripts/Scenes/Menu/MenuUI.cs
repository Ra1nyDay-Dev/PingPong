using PingPong.Scripts.Global;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.SceneLoader;
using PingPong.Scripts.Global.UI;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Menu
{
    public class MenuUI : MonoBehaviour, ISceneUI
    {
        public void OnPlayGameClicked() => 
            ProjectServices.Container.Get<ISceneLoader>().Load(Global.Data.Scenes.GAMEPLAY);
    }
}
