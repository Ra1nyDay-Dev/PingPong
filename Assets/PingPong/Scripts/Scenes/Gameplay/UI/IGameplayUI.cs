using PingPong.Scripts.Global.UI;

namespace PingPong.Scripts.Scenes.Gameplay.UI
{
    public interface IGameplayUI : ISceneUI
    {
        void UpdateUI();
        void Reset();
        void StartRoundCountdown(float time);
    }
}