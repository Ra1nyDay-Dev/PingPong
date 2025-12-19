using System;

namespace PingPong.Scripts.Global.Services.SceneLoader
{
    public interface ISceneLoader : IProjectService
    {
        void Load(string name, Action onLoaded = null);
    }
}