using Microsoft.Xna.Framework;
using GamesObjects;

namespace SpaceInvadersGame.SpaceInvadersObjects
{
    internal class Background : Sprite
    {
        public Background(Game i_Game, string i_AssetName, int i_DrawOrder, int i_UpdateOrder)
            : base(i_Game, i_AssetName, i_DrawOrder, i_UpdateOrder)
        {
            this.m_AssetName = i_AssetName;            
            this.m_Color = Color.White;           
        }      
    }
}
