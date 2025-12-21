using PingPong.Scripts.Global.Data;

namespace PingPong.Scripts.Global.Services.Input
{
    public abstract class PlayerInputService : IInputService
    {
        const string PLAYER1_AXIS_NAME = "Vertical";
        const string PLAYER2_AXIS_NAME = "Vertical2";
        
        protected readonly string _currentAxisName;

        protected PlayerInputService(PlayerId playerId)
        {
            if (playerId == PlayerId.Player1)
                _currentAxisName = PLAYER1_AXIS_NAME;
            else
                _currentAxisName = PLAYER2_AXIS_NAME;
        }
        
        public abstract float VerticalAxis { get; }
    }
}