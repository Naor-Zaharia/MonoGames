using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.GamesObjects.Managers;
using Infrastructure.GamesObjects.ObjectModel.MenuControls;
using Infrastructure.GamesObjects.ObjectModel.MenuControls.MenuEvents;
using Infrastructure.GamesObjects.ServiceInterfaces;
using Infrastructure.ObjectGeneralGame;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Screens;

namespace SpaceInvadersGame.SpaceInvadersGameScreens.Menus
{
    public class SpaceInvadersMenuScreen : GameMenuScreen
    {
        private const string k_FontName = @"Fonts\Consolas";
        protected const string k_DoneSettingsButtonName = "DoneSettings";
        protected const string k_DoneSettingsButtonText = "Done";
        private const string k_BackgroundAssetName = @"Sprites\BG_Space01_1024x768";
        private const string k_ButtonBackgroundAssetName = @"Sprites\ButtonBackground";
        private const string k_MenuMoveSoundName = "MenuMove";
        private const string k_AnimatorPulseName = "Pulse";
        private const float k_BackgroundPercentage = 0.8f;
        private const float k_PulseTargetScale = 1.4f;
        private const float k_PulsePerSec = 0.6f;
        private readonly Color r_InActiveButtonTextColor = Color.DarkMagenta;
        private readonly Color r_ActiveButtonTextColor = Color.DarkBlue;
        private readonly Color r_InActiveButtonTintColor = Color.Lavender;
        private readonly Color r_ActiveButtonTintColor = Color.RoyalBlue;
        private readonly SoundManager r_SoundManager;
        private MenuButton m_CurrentMarkButton;
        protected Background m_Background;

        public SpaceInvadersMenuScreen(Game i_Game) : base(i_Game)
        {
            this.r_SoundManager = (SoundManager)Game.Services.GetService(typeof(ISoundManager));
            this.m_Background = new Background(i_Game, k_BackgroundAssetName, 1, 1, k_BackgroundPercentage);
            this.Add(m_Background);
        }

        public void UnMarkMenuButton(MenuButton i_MenuButton)
        {
            playMovmentMenuSound(i_MenuButton, EventArgs.Empty);
            i_MenuButton.TintColor = r_InActiveButtonTintColor;
            i_MenuButton.TextColor = r_InActiveButtonTextColor;
            i_MenuButton.Animations.Reset();
            i_MenuButton.Animations.Pause();
        }

        public void MarkMenuButton(MenuButton i_MenuButton)
        {
            m_CurrentMarkButton = i_MenuButton;
            i_MenuButton.TintColor = r_ActiveButtonTintColor;
            i_MenuButton.TextColor = r_ActiveButtonTextColor;
            i_MenuButton.Animations.Restart();
        }        

        public MenuButton CreateMenuButton(GameScreen i_GameScreen, string i_ButtonName, string i_ButtonText, Vector2 i_Position)
        {
            MenuButton menuButton = new MenuButton(Game, k_FontName, i_ButtonName);
            initMenuButton(i_GameScreen, menuButton, i_ButtonText, i_Position);
            return menuButton;
        }
        
        public ToggleMenuButton CreateToggleMenuButton(GameScreen i_GameScreen, string i_ButtonName, string i_ButtonText, string i_CurrentOptionString, Vector2 i_Position)
        {
            ToggleMenuButton menuToggleButton = new ToggleMenuButton(Game, k_FontName, i_ButtonName, i_ButtonText, i_CurrentOptionString, Keys.PageUp, Keys.PageDown);
            initMenuButton(i_GameScreen, menuToggleButton, i_ButtonText, i_Position);
            return menuToggleButton;
        }

        public void SetMenuButtonFunctonality(string i_ButtonName, Keys i_Key, MenuButtonPressed.MenuButtonPressedEventHandler i_MenuButtonPressed)
        {
            ButtonFunctonality buttonFunctonality = new ButtonFunctonality(i_Key);
            buttonFunctonality.AddFunctonalityForPress(i_MenuButtonPressed);
            GetMenuButton(i_ButtonName).AddFunctonality(buttonFunctonality);
        }
        
        protected void DoneButtonFunctonality()
        {
            ExitScreen();
        }

        private void initMenuButton(GameScreen i_GameScreen, MenuButton i_MenuButton, string i_ButtonText, Vector2 i_Position)
        {
            i_GameScreen.Add(i_MenuButton);
            i_MenuButton.Text = i_ButtonText;
            i_MenuButton.Initialize();
            if (i_MenuButton is ToggleMenuButton)
            {
                (i_MenuButton as ToggleMenuButton).InitButton();
                (i_MenuButton as ToggleMenuButton).ToggleMenuButtonChanged += playMovmentMenuSound;
            }

            i_MenuButton.Scales = new Vector2(1.2f);
            i_MenuButton.TintColor = r_InActiveButtonTintColor;
            i_MenuButton.TextColor = r_InActiveButtonTextColor;
            i_MenuButton.Texture = this.ContentManager.Load<Texture2D>(k_ButtonBackgroundAssetName);
            i_MenuButton.RotationOrigin = i_MenuButton.SourceRectangleCenter;
            i_MenuButton.PositionOrigin = i_MenuButton.SourceRectangleCenter;
            i_MenuButton.Position = i_Position;
            i_MenuButton.Animations.Add(new PulseAnimator(k_AnimatorPulseName, TimeSpan.Zero, k_PulseTargetScale, k_PulsePerSec));
            i_MenuButton.MenuButtonMarked += MarkMenuButton;
            i_MenuButton.MenuButtonUnMarked += UnMarkMenuButton;
            AddButtonToMenu(i_MenuButton);
        }

        private void playMovmentMenuSound(object i_Sender, EventArgs i_E)
        {
            r_SoundManager.PlaySound(k_MenuMoveSoundName);
        }
    }
}
