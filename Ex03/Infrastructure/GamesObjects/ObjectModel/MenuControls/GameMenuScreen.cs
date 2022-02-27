using System.Collections.Generic;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.GamesObjects.ObjectModel.MenuControls
{
    public class GameMenuScreen : GameScreen
    {
        protected List<MenuButton> m_MenuButtons;
        private MenuButton m_CurrentMarkButton;

        public GameMenuScreen(Game i_Game) : base(i_Game)
        {
            base.Initialize();
            this.m_MenuButtons = new List<MenuButton>();
            this.m_CurrentMarkButton = null;
        }

        public MenuButton CurrentButton
        {
            get
            {
                return m_CurrentMarkButton;
            }
        }

        public void InitMenu()
        {
            if (m_CurrentMarkButton != null)
            {
                m_CurrentMarkButton.OnMenuButtonUnMarked();
            }

            if (m_MenuButtons.Count != 0)
            {
                this.m_CurrentMarkButton = m_MenuButtons[0];
                this.m_CurrentMarkButton.OnMenuButtonMarked();
            }
        }

        public void AddButtonsToMenu(List<MenuButton> i_MenuButtons)
        {
            foreach (MenuButton currentMenuButton in i_MenuButtons)
            {
                m_MenuButtons.Add(currentMenuButton);
            }

            InitMenu();
        }

        public void AddButtonToMenu(MenuButton i_MenuButtons)
        {
            m_MenuButtons.Add(i_MenuButtons);
            InitMenu();
        }

        public MenuButton GetMenuButton(string i_MenuButtonName)
        {
            MenuButton menuButton = null;
            foreach (MenuButton currentMenuButton in m_MenuButtons)
            {
                if (currentMenuButton.ButtonName == i_MenuButtonName)
                {
                    menuButton = currentMenuButton;
                    break;
                }
            }

            return menuButton;
        }

        public void RemoveButtonsFromMenu(List<MenuButton> i_MenuButtons)
        {
            foreach (MenuButton currentMenuButton in i_MenuButtons)
            {
                m_MenuButtons.Remove(currentMenuButton);
            }

            InitMenu();
        }

        public void ScrollUpMenu()
        {
            if (m_MenuButtons.IndexOf(m_CurrentMarkButton) != -1)
            {
                int newIndex = m_MenuButtons.IndexOf(m_CurrentMarkButton) - 1;
                if (newIndex < 0)
                {
                    newIndex = m_MenuButtons.Count - 1;
                }

                m_CurrentMarkButton.OnMenuButtonUnMarked();
                m_CurrentMarkButton = getButtonAtIndex(newIndex);
                m_CurrentMarkButton.OnMenuButtonMarked();
            }
        }

        public void ScrollDownMenu()
        {
            m_CurrentMarkButton.OnMenuButtonUnMarked();
            if (m_MenuButtons.IndexOf(m_CurrentMarkButton) != -1)
            {
                m_CurrentMarkButton = getButtonAtIndex((m_MenuButtons.IndexOf(m_CurrentMarkButton) + 1) % m_MenuButtons.Count);
            }
            else
            {
                m_CurrentMarkButton = getButtonAtIndex(0);
            }

            m_CurrentMarkButton.OnMenuButtonMarked();
        }

        public void MarkMouseHoverButton()
        {
            foreach (MenuButton currentMenuButton in m_MenuButtons)
            {
                Rectangle mouseRectangle = new Rectangle(InputManager.MouseState.Position.X, InputManager.MouseState.Position.Y, 0, 0);

                if (isMouseOverButton(currentMenuButton) && m_CurrentMarkButton != currentMenuButton)
                {
                    m_CurrentMarkButton.OnMenuButtonUnMarked();
                    m_CurrentMarkButton = currentMenuButton;
                    m_CurrentMarkButton.OnMenuButtonMarked();
                }
            }
        }

        private bool isMouseOverButton(MenuButton i_MenuButton)
        {
            Rectangle mouseRectangle = new Rectangle(InputManager.MouseState.Position.X, InputManager.MouseState.Position.Y, 0, 0);
            return i_MenuButton.Bounds.Intersects(mouseRectangle);
        }

        public void ButtonMousePressed()
        {
            if (InputManager.MouseState.LeftButton == ButtonState.Pressed && InputManager.PrevMouseState.LeftButton == ButtonState.Released)
            {
                m_CurrentMarkButton.ButtonMousePicked();
            }
        }

        private MenuButton getButtonAtIndex(int i_ButtonIndex)
        {
            return m_MenuButtons[i_ButtonIndex];
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            if (m_CurrentMarkButton != null)
            {
                if (m_CurrentMarkButton is ToggleMenuButton)
                {
                    (m_CurrentMarkButton as ToggleMenuButton).UpdateScroll(InputManager);
                }
            }

            MarkMouseHoverButton();

            if (InputManager.KeyPressed(Keys.Enter) && InputManager.PrevKeyboardState.IsKeyUp(Keys.Enter))
            {
                m_CurrentMarkButton.ButtonKeyPicked(InputManager);
            }

            if (InputManager.KeyPressed(Keys.Up) && InputManager.PrevKeyboardState.IsKeyUp(Keys.Up))
            {
                ScrollUpMenu();
            }

            if (InputManager.KeyPressed(Keys.Down) && InputManager.PrevKeyboardState.IsKeyUp(Keys.Down))
            {
                ScrollDownMenu();
            }

            if (isMouseOverButton(m_CurrentMarkButton) && InputManager.MouseState.LeftButton == ButtonState.Pressed && InputManager.PrevMouseState.LeftButton == ButtonState.Released)
            {
                ButtonMousePressed();
            }
        }
    }
}
