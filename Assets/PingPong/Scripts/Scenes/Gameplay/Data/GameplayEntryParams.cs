using PingPong.Scripts.Global.Data;

namespace PingPong.Scripts.Scenes.Gameplay.Data
{
    public class GameplayEntryParams
    {
        private PaddleInputType _leftPaddlePaddleInput;
        private PaddleInputType _rightPaddlePaddleInput;

        public GameplayEntryParams(PaddleInputType leftPaddlePaddleInput, PaddleInputType rightPaddlePaddleInput)
        {
            _leftPaddlePaddleInput = leftPaddlePaddleInput;
            _rightPaddlePaddleInput = rightPaddlePaddleInput;
        }
    }
}