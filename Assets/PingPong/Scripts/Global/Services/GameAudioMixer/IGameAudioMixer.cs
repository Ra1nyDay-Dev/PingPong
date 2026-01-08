namespace PingPong.Scripts.Global.Services.GameAudioMixer
{
    public interface IGameAudioMixer : IProjectService
    {
        float GetMusicVolume();
        void SetMusicVolume(float volume);
        float GetSFXVolume();
        void SetSFXVolume(float volume);
    }
}