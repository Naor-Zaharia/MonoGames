using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.GamesObjects.Managers;
using Infrastructure.GamesObjects.ObjectModel.MenuControls;
using Infrastructure.GamesObjects.ServiceInterfaces;
using Infrastructure.ObjectModel;

namespace SpaceInvadersGame.SpaceInvadersGameScreens.Menus
{
   public class SoundSettingsMenuScreen : SpaceInvadersMenuScreen
    {
        private const string k_SoundSettingsTitleAssetName = @"Sprites\SoundSettingsTitle";
        private const string k_ToggleSoundButtonName = "ToggleSound";
        private const string k_ToggleSoundButtonText = "Toggle Sound:";
        private const string k_BackgroundMusicVolumeButtonName = "BackgroundMusicVolume";
        private const string k_BackgroundMusicVolumeButtonText = "Background Music Volume:";
        private const string k_SoundsEffectsVolumeButtonName = "SoundsEffectsVolume";
        private const string k_SoundsEffectsVolumeButtonText = "Sounds Effects Volume:";
        private const float k_ValumeSizeChange = 0.1f;
        private readonly Sprite r_SoundSettingsTitle;
        private readonly SoundManager r_SoundManager;
        private bool m_IsFirstInit;        

        public SoundSettingsMenuScreen(Game i_Game)
            : base(i_Game)
        {
            this.m_IsFirstInit = true;
            this.r_SoundManager = (SoundManager)Game.Services.GetService(typeof(ISoundManager));
            r_SoundSettingsTitle = new Sprite(k_SoundSettingsTitleAssetName, i_Game, 1, 1);
            this.Add(r_SoundSettingsTitle);
            this.UseFadeTransition = false;
            this.BlendState = Microsoft.Xna.Framework.Graphics.BlendState.NonPremultiplied;
        }

        public override void Initialize()
        {
            base.Initialize();
            InitMenu();
            if (m_IsFirstInit)
            {
                r_SoundSettingsTitle.PositionOrigin = r_SoundSettingsTitle.SourceRectangleCenter;
                r_SoundSettingsTitle.RotationOrigin = r_SoundSettingsTitle.SourceRectangleCenter;
                r_SoundSettingsTitle.Position = new Vector2(CenterOfViewPort.X, CenterOfViewPort.Y - r_SoundSettingsTitle.Height);
                createMenuButtons();
                m_IsFirstInit = false;
            }
        }

        private void createMenuButtons()
        {
            ToggleMenuButton currentButton;
            currentButton = CreateToggleMenuButton(this, k_ToggleSoundButtonName, k_ToggleSoundButtonText, getToggleSoundString(), new Vector2(CenterOfViewPort.X, r_SoundSettingsTitle.Position.Y + r_SoundSettingsTitle.Height));
            currentButton.ToggleMenuButtonChanged += toggleSoundFunctonality;
            currentButton = CreateToggleMenuButton(this, k_BackgroundMusicVolumeButtonName, k_BackgroundMusicVolumeButtonText, getBackgroundMusicVolumeString(), new Vector2(CenterOfViewPort.X, currentButton.Position.Y + (2 * currentButton.MeasureString.Y)));
            currentButton.ToggleMenuButtonUpChanged += increaseMusicVolume;
            currentButton.ToggleMenuButtonDownChanged += decreaseMusicVolume;
            currentButton = CreateToggleMenuButton(this, k_SoundsEffectsVolumeButtonName, k_SoundsEffectsVolumeButtonText, getSoundEffectVolumeString(), new Vector2(CenterOfViewPort.X, currentButton.Position.Y + (2 * currentButton.MeasureString.Y)));
            currentButton.ToggleMenuButtonUpChanged += increaseSoundEffectVolume;
            currentButton.ToggleMenuButtonDownChanged += decreaseSoundEffectVolume;
            CreateMenuButton(this, k_DoneSettingsButtonName, k_DoneSettingsButtonText, new Vector2(CenterOfViewPort.X, currentButton.Position.Y + (2 * currentButton.MeasureString.Y)));
            SetMenuButtonFunctonality(k_DoneSettingsButtonName, Keys.Enter, DoneButtonFunctonality);
        }               

        private string getToggleSoundString()
        {
            return r_SoundManager.SoundStatusString();
        }

        private string getSoundEffectVolumeString()
        {
            return ((int)(r_SoundManager.EffectsSoundVolume * 100)).ToString();
        }

        private string getBackgroundMusicVolumeString()
        {
            return ((int)(r_SoundManager.BackgroundSoundVolume * 100)).ToString();
        }

        private void toggleSoundFunctonality(object i_Sender, EventArgs i_E)
        {
            r_SoundManager.ToggleSound();
            (i_Sender as ToggleMenuButton).CurrentOption = getToggleSoundString();
        }

        private void increaseMusicVolume(object i_Sender, EventArgs i_E)
        {
            r_SoundManager.IncreaseMusicVolume(k_ValumeSizeChange);
            (i_Sender as ToggleMenuButton).CurrentOption = getBackgroundMusicVolumeString();
        }

        private void decreaseMusicVolume(object i_Sender, EventArgs i_E)
        {
            r_SoundManager.DecreaseMusicVolume(k_ValumeSizeChange);
            (i_Sender as ToggleMenuButton).CurrentOption = getBackgroundMusicVolumeString();
        }

        private void increaseSoundEffectVolume(object i_Sender, EventArgs i_E)
        {
            r_SoundManager.IncreaseSoundEffectsVolume(k_ValumeSizeChange);
            (i_Sender as ToggleMenuButton).CurrentOption = getSoundEffectVolumeString();
        }

        private void decreaseSoundEffectVolume(object i_Sender, EventArgs i_E)
        {
            r_SoundManager.DecreaseSoundEffectsVolume(k_ValumeSizeChange);
            (i_Sender as ToggleMenuButton).CurrentOption = getSoundEffectVolumeString();
        }
    }
}
