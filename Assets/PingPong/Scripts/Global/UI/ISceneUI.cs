using PingPong.Scripts.Global.Services;

namespace PingPong.Scripts.Global.UI
{
    public interface ISceneUI : ISceneService
    {
        void ShowDialog(string dialogPath);
    }
}