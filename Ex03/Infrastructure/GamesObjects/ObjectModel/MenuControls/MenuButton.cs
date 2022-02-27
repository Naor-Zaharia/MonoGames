using System.Collections.Generic;
using IInfrastructure.GamesObjects.ObjectModel.MenuControls.MenuEvents;
using Infrastructure.GamesObjects.ObjectModel.MenuControls.MenuEvents;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.GamesObjects.ObjectModel.MenuControls
{
    public class MenuButton : TextBlock
    {
        public event MenuButtonMarked.MenuButtonMarkedEventHandler MenuButtonMarked = null;

        public event MenuButtonUnMarked.MenuButtonUnMarkedEventHandler MenuButtonUnMarked = null;

        private List<ButtonFunctonality> m_ButtonFunctonalities;
        public Keys m_SubmitKey;
        private string m_ButtonName;

        public MenuButton(Game i_Game, string i_AssetName, string i_ButtonName) : base(i_AssetName, i_Game)
        {
            this.m_ButtonName = i_ButtonName;
            this.m_ButtonFunctonalities = new List<ButtonFunctonality>();
            this.m_SubmitKey = Keys.Enter;
        }

        public void AddFunctonalities(List<ButtonFunctonality> i_ButtonFunctonalities)
        {
            foreach (ButtonFunctonality currentButtonFunctonality in i_ButtonFunctonalities)
            {
                m_ButtonFunctonalities.Add(currentButtonFunctonality);
            }
        }

        public void AddFunctonality(ButtonFunctonality i_ButtonFunctonality)
        {
            m_ButtonFunctonalities.Add(i_ButtonFunctonality);
        }

        public void RemoveFunctonality(ButtonFunctonality i_ButtonFunctonality)
        {
            m_ButtonFunctonalities.Remove(i_ButtonFunctonality);
        }

        public void ButtonKeyPicked(IInputManager i_InputManager)
        {
            foreach (ButtonFunctonality currentMenuButton in m_ButtonFunctonalities)
            {
                if (i_InputManager.KeyPressed(currentMenuButton.FunctionalityKey))
                {
                    currentMenuButton.OnMenuButtonPressed();
                }
            }
        }

        public void ButtonMousePicked()
        {
            foreach (ButtonFunctonality currentMenuButton in m_ButtonFunctonalities)
            {
                if (currentMenuButton.FunctionalityKey == m_SubmitKey)
                {
                    currentMenuButton.OnMenuButtonPressed();
                }
            }
        }

        public void SetSubmitKey(Keys i_SubmitKey)
        {
            this.m_SubmitKey = i_SubmitKey;
        }

        public void OnMenuButtonMarked()
        {
            if (MenuButtonMarked != null)
            {
                MenuButtonMarked.Invoke(this);
            }
        }

        public void OnMenuButtonUnMarked()
        {
            if (MenuButtonUnMarked != null)
            {
                MenuButtonUnMarked.Invoke(this);
            }
        }

        public void AddFunctonalityForMark(MenuButtonMarked.MenuButtonMarkedEventHandler i_MenuButtonMarked)
        {
            this.MenuButtonMarked += i_MenuButtonMarked;
        }

        public void RemoveFunctonalityForMark(MenuButtonMarked.MenuButtonMarkedEventHandler i_MenuButtonMarked)
        {
            this.MenuButtonMarked -= i_MenuButtonMarked;
        }

        public string ButtonName
        {
            get
            {
                return m_ButtonName;
            }
        }
    }
}
