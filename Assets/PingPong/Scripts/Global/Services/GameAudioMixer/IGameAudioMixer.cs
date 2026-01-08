namespace PingPong.Scripts.Global.Services.GameAudioMixer
{
    public interface IGameAudioMixer
    {
        void GetMusicVolume();
        void SetMusicVolume(float volume);
        void GetSFXVolume();
        void SetSFXVolume(float volume);
    }
}