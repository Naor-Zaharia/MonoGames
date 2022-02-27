using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.GamesObjects.ServiceInterfaces
{
    public interface ISoundManager
    {
         event EventHandler<EventArgs> BackgroudMusicVolumeChanged;

         event EventHandler<EventArgs> EffectsSoundsVolumeChanged;

         event EventHandler<EventArgs> ToggleVolumeChanged;

        void PlaySound(string i_EffectName);

        void AddEffect(string i_AffectName, string i_AssetName);

        void RemoveEffect(string i_AffectName);

        void ToggleSound();

        void SetBackgroundMusic(string i_AssetName);

        void RemoveBackgroundMusic();

        bool IsMute();

        void PlayBackgoundMusicOnLoop();

        void PlayEffectOnLoop(string i_EffectName);

        void OnDemandLoadAndPlaySound(string i_AssetName);

        void StopBackgroundMusic(string i_EffectName);

        void StopSoundEffect(string i_EffectName);

        void PauseBackgroundMusic(string i_EffectName);

        void PauseSoundEffect(string i_EffectName);

        void IncreaseMusicVolume(float i_IncreaseValue);

        void DecreaseMusicVolume(float i_DecreaseValue);

        void IncreaseSoundEffectsVolume(float i_IncreaseValue);

        void DecreaseSoundEffectsVolume(float i_DecreaseValue);
    }
}
