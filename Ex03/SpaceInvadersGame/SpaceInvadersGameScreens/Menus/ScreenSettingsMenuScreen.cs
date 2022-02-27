using System;
using Infrastructure.GamesObjects.ObjectModel.MenuControls;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvadersGame.SpaceInvadersGameScreens.Menus
{
    public class ScreenSettingsMenuScreen : SpaceInvadersMenuScreen
    {
        private const string k_ScreenSettingsTitleAssetName = @"Sprites\ScreenSettingsTitle";
        private const string k_OnMsg = "On";
        private const string k_OffMsg = "Off";
        private const string k_WindowResizingSettingsButtonName = "WindowResizingSettings";
        private const string k_WindowResizingSettingsButtonText = "Allow Window Resizing:";
        private const string k_FullScreenSettingsButtonName = "FullScreenSettings";
        private const string k_FullScreenSettingsButtonText = "Full Screen Mode:";
        private const string k_MouseVisabilitySettingsButtonName = "MouseVisabilitySettings";
        private const string k_MouseVisabilitySettingsButtonText = "Mouse Visability:";
        private readonly Sprite r_ScreenSettingsTitle;
        private bool m_IsFirstInit;

        public ScreenSettingsMenuScreen(Game i_Game)
            : base(i_Game)
        {
            this.m_IsFirstInit = true;
            r_ScreenSettingsTitle = new Sprite(k_ScreenSettingsTitleAssetName, i_Game, 1, 1);
            this.Add(r_ScreenSettingsTitle);
            this.UseFadeTransition = false;
            this.BlendState = Microsoft.Xna.Framework.Graphics.BlendState.NonPremultiplied;
        }

        private void createMenuButtons()
        {
            ToggleMenuButton currentButton;
            currentButton = CreateToggleMenuButton(this, k_WindowResizingSettingsButtonName, k_WindowResizingSettingsButtonText, getWindowResizingSettingsString(), new Vector2(CenterOfViewPort.X, r_ScreenSettingsTitle.Position.Y + r_ScreenSettingsTitle.Height));
            currentButton.ToggleMenuButtonChanged += windowResizingSettingsChanged;
            currentButton = CreateToggleMenuButton(this, k_FullScreenSettingsButtonName, k_FullScreenSettingsButtonText, getFullScreenSettingsString(), new Vector2(CenterOfViewPort.X, currentButton.Position.Y + (2 * currentButton.MeasureString.Y)));
            currentButton.ToggleMenuButtonChanged += fullScreenSettingsChanged;
            currentButton = CreateToggleMenuButton(this, k_MouseVisabilitySettingsButtonName, k_MouseVisabilitySettingsButtonText, getMouseVisabilitySettingsString(), new Vector2(CenterOfViewPort.X, currentButton.Position.Y + (2 * currentButton.MeasureString.Y)));
            currentButton.ToggleMenuButtonChanged += mouseVisabilitySettingsChanged;
            CreateMenuButton(this, k_DoneSettingsButtonName, k_DoneSettingsButtonText, new Vector2(CenterOfViewPort.X, currentButton.Position.Y + (2 * currentButton.MeasureString.Y)));
            SetMenuButtonFunctonality(k_DoneSettingsButtonName, Keys.Enter, DoneButtonFunctonality);
        }

        private void menuButtonUnMarked(MenuButton i_MenuButton)
        {
            UnMarkMenuButton(i_MenuButton);
        }

        private void menuButtonMarked(MenuButton i_MenuButton)
        {
            MarkMenuButton(i_MenuButton);
        }

        public override void Initialize()
        {
            base.Initialize();
            InitMenu();
            if (m_IsFirstInit)
            {
                r_ScreenSettingsTitle.PositionOrigin = r_ScreenSettingsTitle.SourceRectangleCenter;
                r_ScreenSettingsTitle.RotationOrigin = r_ScreenSettingsTitle.SourceRectangleCenter;
                r_ScreenSettingsTitle.Position = new Vector2(CenterOfViewPort.X, CenterOfViewPort.Y - r_ScreenSettingsTitle.Height);
                createMenuButtons();
                m_IsFirstInit = false;
            }
        }

        private void windowResizingSettingsFunctonality()
        {
            Game.Window.AllowUserResizing = !Game.Window.AllowUserResizing;
        }

        private void fullScreenSettingsFunctonality()
        {
            GraphicsDeviceManager graphicsDeviceManager = (GraphicsDeviceManager)Game.Services.GetService(typeof(GraphicsDeviceManager));
            graphicsDeviceManager.ToggleFullScreen();
        }

        private void mouseVisabilitySettingsFunctonality()
        {
            Game.IsMouseVisible = !Game.IsMouseVisible;
        }

        private string getWindowResizingSettingsString()
        {
            string windowResizingSettingsString;

            if (Game.Window.AllowUserResizing)
            {
                windowResizingSettingsString = k_OnMsg;
            }
            else
            {
                windowResizingSettingsString = k_OffMsg;
            }

            return windowResizingSettingsString;
        }

        private string getFullScreenSettingsString()
        {
            string fullScreenSettingsString;

            GraphicsDeviceManager graphicsDeviceManager = (GraphicsDeviceManager)Game.Services.GetService(typeof(GraphicsDeviceManager));

            if (graphicsDeviceManager.IsFullScreen)
            {
                fullScreenSettingsString = k_OnMsg;
            }
            else
            {
                fullScreenSettingsString = k_OffMsg;
            }

            return fullScreenSettingsString;
        }

        private string getMouseVisabilitySettingsString()
        {
            string mouseVisabilitySettingsString;

            if (Game.IsMouseVisible)
            {
                mouseVisabilitySettingsString = k_OnMsg;
            }
            else
            {
                mouseVisabilitySettingsString = k_OffMsg;
            }

            return mouseVisabilitySettingsString;
        }

        private void mouseVisabilitySettingsChanged(object i_Sender, EventArgs i_E)
        {
            mouseVisabilitySettingsFunctonality();
            (i_Sender as ToggleMenuButton).CurrentOption = getMouseVisabilitySettingsString();
        }

        private void windowResizingSettingsChanged(object i_Sender, EventArgs i_E)
        {
            windowResizingSettingsFunctonality();
            (i_Sender as ToggleMenuButton).CurrentOption = getWindowResizingSettingsString();
        }

        private void fullScreenSettingsChanged(object i_Sender, EventArgs i_E)
        {
            (i_Sender as ToggleMenuButton).CurrentOption = getFullScreenSettingsString();
            fullScreenSettingsFunctonality();
        }
    }
}