using Microsoft.Xna.Framework;

namespace SpaceInvadersGame.SpaceInvadersObjects
{
    internal class EnemyBullet : Bullet
    {
        protected readonly Color r_Color = Color.Blue;

        public EnemyBullet(Game i_Game, string i_AssetName, int i_DrawOrder, int i_UpdateOrder)
           : base(i_Game, i_AssetName, i_DrawOrder, i_UpdateOrder)
        {
            this.m_Color = r_Color;
        }

        public override void Update(GameTime i_GameTime)
        {
            UpdateYPosition((float)i_GameTime.ElapsedGameTime.TotalSeconds * k_BulletVelocity);
        }
    }
}
