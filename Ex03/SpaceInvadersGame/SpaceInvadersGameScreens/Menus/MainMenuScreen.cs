using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.GamesObjects.Managers;
using Infrastructure.GamesObjects.ObjectModel.MenuControls;
using Infrastructure.GamesObjects.ServiceInterfaces;
using Infrastructure.ObjectModel;
using SpaceInvadersGame.SpaceInvadersGameServices;
using SpaceInvadersGame.SpaceInvadersGameServices.SpaceInvadersGameInterfaces;

namespace SpaceInvadersGame.SpaceInvadersGameScreens.Menus
{
    public class MainMenuScreen : SpaceInvadersMenuScreen
    {
        private const string k_MainMenuTitleAssetName = @"Sprites\MainMenuTitle";
        private const string k_OnePlayerMsg = "One";
        private const string k_TwoPlayerMsg = "Two";
        private const string k_ScreenSettingButtonName = "ScreenSettings";
        private const string k_ScreenSettingButtonText = "Screen Settings";
        private const string k_PlayerSettingButtonName = "PlayersSettings";
        private const string k_PlayerSettingButtonText = "Players:";
        private const string k_SoundSettingButtonName = "SoundSettings";
        private const string k_SoundSettingButtonText = "Sound Settings";
        private const string k_PlayButtonName = "PlaySettings";
        private const string k_PlayButtonText = "Play";
        private const string k_QuitButtonName = "QuitSettings";
        private const string k_QuitButtonText = "Quit";
        private const int k_GapBetweenButtons = 2;

        private readonly ScreenSettingsMenuScreen r_ScreenSettingsMenuScreen;
        private readonly SoundSettingsMenuScreen r_SoundSettingsMenuScreen;
        private readonly SoundManager r_SoundManager;
        private readonly SpaceInvaderGameManagerService r_GameManager;
        private readonly Sprite r_MainMenuTitle;
        private bool m_IsFirstInit;

        public MainMenuScreen(Game i_Game)
            : base(i_Game)
        {
            this.r_GameManager = (SpaceInvaderGameManagerService)Game.Services.GetService(typeof(ISpaceInvaderGameManagerService));
            this.r_SoundManager = (SoundManager)Game.Services.GetService(typeof(ISoundManager));
            this.m_IsFirstInit = true;
            this.r_MainMenuTitle = new Sprite(k_MainMenuTitleAssetName, i_Game, 1, 1);
            this.Add(r_MainMenuTitle);
            this.r_ScreenSettingsMenuScreen = new ScreenSettingsMenuScreen(Game);
            this.r_SoundSettingsMenuScreen = new SoundSettingsMenuScreen(Game);
            this.UseFadeTransition = false;
            this.BlendState = Microsoft.Xna.Framework.Graphics.BlendState.NonPremultiplied;
        }

        private void createMenuButtons()
        {
            MenuButton currentButton;
            currentButton = CreateMenuButton(this, k_ScreenSettingButtonName, k_ScreenSettingButtonText, new Vector2(CenterOfViewPort.X, r_MainMenuTitle.Position.Y + r_MainMenuTitle.Height));
            SetMenuButtonFunctonality(k_ScreenSettingButtonName, Keys.Enter, screenSettingsButtonFunctonality);
            currentButton = CreateToggleMenuButton(this, k_PlayerSettingButtonName, k_PlayerSettingButtonText, getAmountOfPlayerString(), new Vector2(CenterOfViewPort.X, currentButton.Position.Y + (k_GapBetweenButtons * currentButton.MeasureString.Y)));
            (currentButton as ToggleMenuButton).ToggleMenuButtonChanged += increaseAmountOfPlayers;
            currentButton = CreateMenuButton(this, k_SoundSettingButtonName, k_SoundSettingButtonText, new Vector2(CenterOfViewPort.X, currentButton.Position.Y + (k_GapBetweenButtons * currentButton.MeasureString.Y)));
            SetMenuButtonFunctonality(k_SoundSettingButtonName, Keys.Enter, soundSettingsButtonFunctonality);
            currentButton = CreateMenuButton(this, k_PlayButtonName, k_PlayButtonText, new Vector2(CenterOfViewPort.X, currentButton.Position.Y + (k_GapBetweenButtons * currentButton.MeasureString.Y)));
            SetMenuButtonFunctonality(k_PlayButtonName, Keys.Enter, playButtonFunctonality);
            CreateMenuButton(this, k_QuitButtonName, k_QuitButtonText, new Vector2(CenterOfViewPort.X, currentButton.Position.Y + (k_GapBetweenButtons * currentButton.MeasureString.Y)));
            SetMenuButtonFunctonality(k_QuitButtonName, Keys.Enter, quitButtonFunctonality);
        }

        public override void Initialize()
        {
            base.Initialize();
            InitMenu();

            if (m_IsFirstInit)
            {
                r_MainMenuTitle.PositionOrigin = r_MainMenuTitle.SourceRectangleCenter;
                r_MainMenuTitle.RotationOrigin = r_MainMenuTitle.SourceRectangleCenter;
                r_MainMenuTitle.Position = new Vector2(CenterOfViewPort.X, CenterOfViewPort.Y - r_MainMenuTitle.Height);
                createMenuButtons();
                m_IsFirstInit = false;
            }
        }

        private void quitButtonFunctonality()
        {
            Game.Exit();
        }

        private void playButtonFunctonality()
        {
            r_GameManager.PlayScreen.InitGame();
            m_ScreensManager.SetCurrentScreen(new LevelTransitionScreen(Game, 1));
        }

        private void screenSettingsButtonFunctonality()
        {
            m_ScreensManager.SetCurrentScreen(r_ScreenSettingsMenuScreen);
        }

        private void soundSettingsButtonFunctonality()
        {
            m_ScreensManager.SetCurrentScreen(r_SoundSettingsMenuScreen);
        }

        private string getAmountOfPlayerString()
        {
            string amountOfPlayerString;

            if (r_GameManager.AmountOfPlayers == 1)
            {
                amountOfPlayerString = k_OnePlayerMsg;
            }
            else
            {
                amountOfPlayerString = k_TwoPlayerMsg;
            }

            return amountOfPlayerString;
        }

        private void increaseAmountOfPlayers(object i_Sender, EventArgs i_E)
        {
            r_GameManager.IncreaseAmountOfPlayers();
            (i_Sender as ToggleMenuButton).CurrentOption = getAmountOfPlayerString();
        }
    }
}