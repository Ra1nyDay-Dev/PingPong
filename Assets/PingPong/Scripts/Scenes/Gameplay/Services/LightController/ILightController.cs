using PingPong.Scripts.Global.Services;

namespace PingPong.Scripts.Scenes.Gameplay.Services.LightController
{
    public interface ILightController : ISceneService
    {
        void HighlightLeftGates();
        void HighlightRightGates();
        void ResetLights();
    }
}