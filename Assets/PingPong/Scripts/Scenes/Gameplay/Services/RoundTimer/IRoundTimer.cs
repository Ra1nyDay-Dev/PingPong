using PingPong.Scripts.Global.Services;

namespace PingPong.Scripts.Scenes.Gameplay.Services.RoundTimer
{
    public interface IRoundTimer : ISceneService
    {
        float RoundTime { get; }
        bool IsRunning { get; }
        void StartTimer();
        void StopTimer();
        void Reset();
    }
}