using System;
using System.Collections.Generic;
using Infrastructure.GamesObjects.ServiceInterfaces;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Infrastructure.GamesObjects.Managers
{
    public class SoundManager : GameService, ISoundManager
    {
        private const string k_OnMsg = "On";
        private const string k_OffMsf = "Off";

        public event EventHandler<EventArgs> BackgroudMusicVolumeChanged;

        public event EventHandler<EventArgs> EffectsSoundsVolumeChanged;

        public event EventHandler<EventArgs> ToggleVolumeChanged;

        private Dictionary<string, SoundEffectInstance> m_SoundEffects = new Dictionary<string, SoundEffectInstance>();
        private SoundEffectInstance m_BackgroundMusic;
        private float m_BackgroundSoundVolume;
        private float m_EffectsSoundVolume;
        private bool m_IsMute;

        public SoundManager(Game i_Game) : base(i_Game)
        {
            this.m_BackgroundSoundVolume = 1;
            this.m_EffectsSoundVolume = 1;
            this.m_IsMute = false;
        }

        protected override void RegisterAsService()
        {
            Game.Services.AddService(typeof(ISoundManager), this);
        }

        public void PlaySound(string i_EffectName)
        {
            if (!m_IsMute)
            {
                SoundEffectInstance soundEffectInstance = null;
                m_SoundEffects.TryGetValue(i_EffectName, out soundEffectInstance);
                if (soundEffectInstance != null)
                {
                    soundEffectInstance.Volume = m_EffectsSoundVolume;
                    soundEffectInstance.Play();
                }
            }
        }

        public void AddEffect(string i_AffectName, string i_AssetName)
        {
            SoundEffect currentSoundEffect = Game.Content.Load<SoundEffect>(i_AssetName);
            m_SoundEffects.Add(i_AffectName, currentSoundEffect.CreateInstance());
        }

        public void RemoveEffect(string i_AffectName)
        {
            m_SoundEffects.Remove(i_AffectName);
        }

        private void mute()
        {
            m_IsMute = true;
            SoundEffect.MasterVolume = 0.0f;
        }

        private void unmute()
        {
            m_IsMute = false;
            SoundEffect.MasterVolume = 1.0f;
        }

        public void ToggleSound()
        {
            if (m_IsMute)
            {
                unmute();
            }
            else
            {
                mute();
            }

            OnToggleVolumeChanged();
        }

        public string SoundStatusString()
        {
            string soundStatusString;

            if (IsMute())
            {
                soundStatusString = k_OffMsf;
            }
            else
            {
                soundStatusString = k_OnMsg;
            }

            return soundStatusString;
        }

        public void SetBackgroundMusic(string i_AssetName)
        {
            SoundEffect currentSoundEffect = Game.Content.Load<SoundEffect>(i_AssetName);
            m_BackgroundMusic = currentSoundEffect.CreateInstance();
        }

        public void RemoveBackgroundMusic()
        {
            m_BackgroundMusic = null;
        }

        public bool IsMute()
        {
            return m_IsMute;
        }

        public void PlayBackgoundMusicOnLoop()
        {
            if (!m_IsMute)
            {
                if (m_BackgroundMusic != null)
                {
                    m_BackgroundMusic.IsLooped = true;
                    m_BackgroundMusic.Volume = m_BackgroundSoundVolume;
                    m_BackgroundMusic.Play();
                }
            }
        }

        public void PlayEffectOnLoop(string i_EffectName)
        {
            if (!m_IsMute)
            {
                SoundEffectInstance soundEffectInstance = null;
                m_SoundEffects.TryGetValue(i_EffectName, out soundEffectInstance);
                if (soundEffectInstance != null)
                {
                    m_BackgroundMusic.IsLooped = true;
                    soundEffectInstance.Volume = m_EffectsSoundVolume;
                    soundEffectInstance.Play();
                }
            }
        }

        public void StopBackgroundMusic(string i_EffectName)
        {
            if (m_BackgroundMusic != null)
            {
                m_BackgroundMusic.Stop();
            }
        }

        public void StopSoundEffect(string i_EffectName)
        {
            SoundEffectInstance soundEffectInstance = null;
            m_SoundEffects.TryGetValue(i_EffectName, out soundEffectInstance);
            if (soundEffectInstance != null)
            {
                soundEffectInstance.Stop();
            }
        }

        public void PauseBackgroundMusic(string i_EffectName)
        {
            if (m_BackgroundMusic != null)
            {
                m_BackgroundMusic.Pause();
            }
        }

        public void PauseSoundEffect(string i_EffectName)
        {
            SoundEffectInstance soundEffectInstance = null;
            m_SoundEffects.TryGetValue(i_EffectName, out soundEffectInstance);
            if (soundEffectInstance != null)
            {
                soundEffectInstance.Pause();
            }
        }

        public void IncreaseMusicVolume(float i_IncreaseValue)
        {
            if (m_BackgroundSoundVolume == 1)
            {
                m_BackgroundSoundVolume = 0;
            }
            else
            {
                m_BackgroundSoundVolume += i_IncreaseValue;
                m_BackgroundSoundVolume = MathHelper.Clamp(m_BackgroundSoundVolume, 0, 1);
                m_BackgroundSoundVolume = (float)Math.Round(m_BackgroundSoundVolume, 2);
            }

            if (m_BackgroundMusic != null)
            {
                m_BackgroundMusic.Volume = m_BackgroundSoundVolume;
            }

            OnBackgroudMusicVolumeChanged();
        }

        public void OnDemandLoadAndPlaySound(string i_AssetName)
        {
            SoundEffect currentSoundEffect = Game.Content.Load<SoundEffect>(i_AssetName);
            SoundEffectInstance currentSoundEffectInstance = currentSoundEffect.CreateInstance();
            currentSoundEffect.Dispose();
            currentSoundEffectInstance.Volume = m_EffectsSoundVolume;
            currentSoundEffectInstance.Play();
        }

        public void DecreaseMusicVolume(float i_DecreaseValue)
        {
            if (m_BackgroundSoundVolume == 0)
            {
                m_BackgroundSoundVolume = 1;
            }
            else
            {
                m_BackgroundSoundVolume -= i_DecreaseValue;
                m_BackgroundSoundVolume = MathHelper.Clamp(m_BackgroundSoundVolume, 0, 1);
                m_BackgroundSoundVolume = (float)Math.Round(m_BackgroundSoundVolume, 2);
            }

            if (m_BackgroundMusic != null)
            {
                m_BackgroundMusic.Volume = m_BackgroundSoundVolume;
            }

            OnBackgroudMusicVolumeChanged();
        }

        public void IncreaseSoundEffectsVolume(float i_IncreaseValue)
        {
            if (m_EffectsSoundVolume == 1)
            {
                m_EffectsSoundVolume = 0;
            }
            else
            {
                m_EffectsSoundVolume += i_IncreaseValue;
                m_EffectsSoundVolume = MathHelper.Clamp(m_EffectsSoundVolume, 0, 1);
                m_EffectsSoundVolume = (float)Math.Round(m_EffectsSoundVolume, 2);
            }

            updateLoopEffects();
            OnEffectsSoundsVolumeChanged();
        }

        public void DecreaseSoundEffectsVolume(float i_DecreaseValue)
        {
            if (m_EffectsSoundVolume == 0)
            {
                m_EffectsSoundVolume = 1;
            }
            else
            {
                m_EffectsSoundVolume -= i_DecreaseValue;
                m_EffectsSoundVolume = MathHelper.Clamp(m_EffectsSoundVolume, 0, 1);
                m_EffectsSoundVolume = (float)Math.Round(m_EffectsSoundVolume, 2);
            }

            updateLoopEffects();
            OnEffectsSoundsVolumeChanged();
        }

        private void updateLoopEffects()
        {
            foreach (KeyValuePair<string, SoundEffectInstance> currentSoundEffectInstace in m_SoundEffects)
            {
                if (currentSoundEffectInstace.Value.IsLooped)
                {
                    currentSoundEffectInstace.Value.Volume = m_EffectsSoundVolume;
                }
            }
        }

        protected virtual void OnBackgroudMusicVolumeChanged()
        {
            if (BackgroudMusicVolumeChanged != null)
            {
                BackgroudMusicVolumeChanged.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void OnEffectsSoundsVolumeChanged()
        {
            if (EffectsSoundsVolumeChanged != null)
            {
                EffectsSoundsVolumeChanged.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void OnToggleVolumeChanged()
        {
            if (ToggleVolumeChanged != null)
            {
                ToggleVolumeChanged.Invoke(this, EventArgs.Empty);
            }
        }

        public float BackgroundSoundVolume
        {
            get
            {
                return m_BackgroundSoundVolume;
            }
        }

        public float EffectsSoundVolume
        {
            get
            {
                return m_EffectsSoundVolume;
            }
        }
    }
}
