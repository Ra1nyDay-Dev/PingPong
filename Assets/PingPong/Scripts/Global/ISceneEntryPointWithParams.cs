using PingPong.Scripts.Global.Services.SceneLoader;
using PingPong.Scripts.Global.UI;

namespace PingPong.Scripts.Global
{
    public interface ISceneEntryPointWithParams<TSceneParams> : ISceneParams
    {
        void Run(GameUI gameUI, TSceneParams sceneParams);
    }
}