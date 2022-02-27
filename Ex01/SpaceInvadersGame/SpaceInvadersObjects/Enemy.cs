using Microsoft.Xna.Framework;
using GamesObjects;

namespace SpaceInvadersGame
{
    internal class Enemy : Sprite
    {
        private int m_EnemyValue;
        protected float m_DrawElapsedTime;

        public Enemy(Game i_Game, string i_AssetName, int i_DrawOrder, int i_UpdateOrder, Vector2 i_EnemyPosition, Color i_EnemyColor, int i_EnemyValue)
            : base(i_Game, i_AssetName, i_DrawOrder, i_UpdateOrder)
        {
            this.Position = i_EnemyPosition;
            this.m_Color = i_EnemyColor;
            this.m_EnemyValue = i_EnemyValue;
            this.m_DrawElapsedTime = 0;
        }

        internal int EnemyValue
        {
            get
            {
                return m_EnemyValue;
            }

            set
            {
                this.m_EnemyValue = value;
            }
        }       
    }
}
