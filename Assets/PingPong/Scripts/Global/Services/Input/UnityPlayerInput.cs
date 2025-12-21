using PingPong.Scripts.Global.Data;

namespace PingPong.Scripts.Global.Services.Input
{
    public class UnityPlayerInput : PlayerInputService
    {
        public UnityPlayerInput(PlayerId playerId) : base(playerId){}

        public override float VerticalAxis => 
            UnityEngine.Input.GetAxis(_currentAxisName);
    }
}