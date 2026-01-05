using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services.SceneLoader;

namespace PingPong.Scripts.Scenes.Gameplay
{
    public class GameplayEntryParams : ISceneParams
    {
        public GameVersusMode GameVersusMode { get; }
        public AIDifficulty Difficulty { get; }

        public GameplayEntryParams(GameVersusMode gameVersusMode, AIDifficulty difficulty = AIDifficulty.Easy)
        {
            GameVersusMode = gameVersusMode;
            Difficulty = difficulty;
        }

    }
}