using System;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.GamesObjects.ObjectModel.MenuControls
{
    public class ToggleMenuButton : MenuButton
    {
        public event EventHandler<EventArgs> ToggleMenuButtonChanged = null;

        public event EventHandler<EventArgs> ToggleMenuButtonUpChanged = null;

        public event EventHandler<EventArgs> ToggleMenuButtonDownChanged = null;

        private Keys m_ScrollUpKey;
        private Keys m_ScrollDownKey;
        private string m_ToggleString;
        private string m_CurrentOption;

        public ToggleMenuButton(Game i_Game, string i_AssetName, string i_ButtonName, string i_ToggleString, string i_CurrentOption, Keys i_ScrollUpKey, Keys i_ScrollDownKey) : base(i_Game, i_AssetName, i_ButtonName)
        {
            this.m_ToggleString = i_ToggleString;
            this.m_ScrollUpKey = i_ScrollUpKey;
            this.m_ScrollDownKey = i_ScrollDownKey;
            this.m_CurrentOption = i_CurrentOption;
        }

        public void ScrollUpMenu()
        {
            onToggleUpOptionChange();
        }

        public void ScrollDownMenu()
        {
            onToggleDownOptionChange();
        }

        public void InitButton()
        {
            this.Text = string.Format(@"{0} {1}", m_ToggleString, m_CurrentOption);
        }

        private void onToggleUpOptionChange()
        {
            if (ToggleMenuButtonUpChanged != null)
            {
                ToggleMenuButtonUpChanged.Invoke(this, EventArgs.Empty);
            }

            onToggleOptionChange();
        }

        private void onToggleDownOptionChange()
        {
            if (ToggleMenuButtonDownChanged != null)
            {
                ToggleMenuButtonDownChanged.Invoke(this, EventArgs.Empty);
            }

            onToggleOptionChange();
        }

        private void onToggleOptionChange()
        {
            if (ToggleMenuButtonChanged != null)
            {
                ToggleMenuButtonChanged.Invoke(this, EventArgs.Empty);
            }

            this.Text = string.Format(@"{0} {1}", m_ToggleString, m_CurrentOption);
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Text = string.Format(@"{0} {1}", m_ToggleString, m_CurrentOption);
        }

        public string CurrentOption
        {
            get
            {
                return m_CurrentOption;
            }

            set
            {
                m_CurrentOption = value;
            }
        }

        public void UpdateScroll(IInputManager i_InputManager)
        {
            bool scrollUpWheelValue = i_InputManager.MouseState.ScrollWheelValue > i_InputManager.PrevMouseState.ScrollWheelValue;
            bool scrollDownWheelValue = i_InputManager.MouseState.ScrollWheelValue < i_InputManager.PrevMouseState.ScrollWheelValue;
            bool scrollRightClick = i_InputManager.MouseState.RightButton == ButtonState.Pressed && i_InputManager.PrevMouseState.RightButton == ButtonState.Released;

            if (i_InputManager.KeyPressed(m_ScrollUpKey) || scrollUpWheelValue || scrollRightClick)
            {
                ScrollUpMenu();
            }

            if (i_InputManager.KeyPressed(m_ScrollDownKey) || scrollDownWheelValue)
            {
                ScrollDownMenu();
            }
        }
    }
}
