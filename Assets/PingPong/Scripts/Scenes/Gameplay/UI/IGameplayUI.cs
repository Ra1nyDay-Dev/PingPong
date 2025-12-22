using PingPong.Scripts.Global.UI;

namespace PingPong.Scripts.Scenes.Gameplay.UI
{
    public interface IGameplayUI : ISceneUI
    {
        void StartRoundCountdown(float time);
    }
}