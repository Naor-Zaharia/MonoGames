using Infrastructure.GamesObjects.ObjectModel.MenuControls.MenuEvents;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.GamesObjects.ObjectModel.MenuControls
{
    public class ButtonFunctonality
    {
        public event MenuButtonPressed.MenuButtonPressedEventHandler MenuButtonPressed = null;

        private Keys m_FunctionalityKey;

        public ButtonFunctonality(Keys i_Key)
        {
            this.m_FunctionalityKey = i_Key;
        }

        public Keys FunctionalityKey
        {
            get
            {
                return m_FunctionalityKey;
            }
        }

        public void AddFunctonalityForPress(MenuButtonPressed.MenuButtonPressedEventHandler i_MenuButtonPressed)
        {
            this.MenuButtonPressed += i_MenuButtonPressed;
        }

        public void RemoveFunctonalityForPress(MenuButtonPressed.MenuButtonPressedEventHandler i_MenuButtonPressed)
        {
            this.MenuButtonPressed -= i_MenuButtonPressed;
        }
        
        public void OnMenuButtonPressed()
        {
            if (MenuButtonPressed != null)
            {
                MenuButtonPressed.Invoke();
            }
        }

        public Keys Key
        {
            get
            {
                return m_FunctionalityKey;
            }
        }
    }
}
