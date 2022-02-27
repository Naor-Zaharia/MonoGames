using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GamesObjects;
using SpaceInvadersGame.SpaceInvadersObjects;

namespace SpaceInvadersGame
{
    internal class SpaceShip : Sprite
    {
        private const Keys k_ShootKeyboardKey = Keys.Enter;
        private const float k_SpaceShipVelocity = 130;
        private const byte k_InitRemainingLife = 3;
        private const int k_MaximalAmountOfSpaceShipBulletsAtOnce = 2;        
        private const int k_GapSpaceShipFromBottom = 15;
        private const string k_BulletAssetName = @"Sprites\Bullet";

        private KeyboardState m_PreviousKeyboardState;
        private MouseState m_PreviousMouseState;
        private List<SpaceShipBullet> m_SpaceShipBulletList;
        private byte m_RemainingLife;

        public SpaceShip(Game i_Game, string i_AssetName, int i_DrawOrder, int i_UpdateOrder)
            : base(i_Game, i_AssetName, i_DrawOrder, i_UpdateOrder)
        {
            this.m_Color = Color.White;
            this.m_RemainingLife = k_InitRemainingLife;
            this.m_SpaceShipBulletList = new List<SpaceShipBullet>();
            i_Game.Components.Add(this);
        }

        protected override void InitBounds()
        {
            base.InitBounds();
            Position = new Vector2(Game.GraphicsDevice.Viewport.Width - m_Width, Game.GraphicsDevice.Viewport.Height - m_Height - k_GapSpaceShipFromBottom);
        }

        public override void Update(GameTime i_Game)
        {
            MouseState currentMouseState = Mouse.GetState();
            KeyboardState currentKeyboardState = Keyboard.GetState();

            shipBulletsUpdate(i_Game);
            moveSpaceShip(currentKeyboardState, currentMouseState, i_Game);
            shootBullet(currentKeyboardState, currentMouseState, i_Game);
            this.m_PreviousMouseState = currentMouseState;
        }

        internal byte RemainingLife
        {
            get
            {
                return m_RemainingLife;
            }
        }

        internal void SpaceShipGetHit()
        {
            m_RemainingLife--;
            InitBounds();
        }

        internal List<SpaceShipBullet> SpaceShipBulletList
        {
            get
            {
                return m_SpaceShipBulletList;
            }
        }

        private void moveSpaceShip(KeyboardState i_KeyboardState, MouseState i_MouseState, GameTime i_GameTime)
        {
            if (i_KeyboardState.IsKeyDown(Keys.Left))
            {
                UpdateXPosition((float)i_GameTime.ElapsedGameTime.TotalSeconds * k_SpaceShipVelocity * -1);
            }

            if (i_KeyboardState.IsKeyDown(Keys.Right))
            {
                UpdateXPosition((float)i_GameTime.ElapsedGameTime.TotalSeconds * k_SpaceShipVelocity);
            }

            UpdateXPosition(getMousePositionDelta(i_MouseState).X);
        }

        private Vector2 getMousePositionDelta(MouseState i_MouseState)
        {
            Vector2 newPosition;

            newPosition.X = i_MouseState.X - m_PreviousMouseState.X;
            newPosition.Y = i_MouseState.Y - m_PreviousMouseState.Y;

            return newPosition;
        }

        private void shootBullet(KeyboardState i_KeyboardState, MouseState i_MouseState, GameTime i_GameTime)
        {
            if (((i_KeyboardState.IsKeyDown(k_ShootKeyboardKey) && m_PreviousKeyboardState.IsKeyUp(k_ShootKeyboardKey)) ||
                (i_MouseState.LeftButton == ButtonState.Pressed && m_PreviousMouseState.LeftButton == ButtonState.Released)) && m_SpaceShipBulletList.Count < k_MaximalAmountOfSpaceShipBulletsAtOnce)
            {
                SpaceShipBullet currentSpaceShipBullet = new SpaceShipBullet(this.Game, k_BulletAssetName, 0, 0);
                currentSpaceShipBullet.Initialize();

                // Find bullet position according to the spaceship
                currentSpaceShipBullet.Position = new Vector2(Position.X + (Texture.Width / 2.0f) - (currentSpaceShipBullet.Width / 2.0f), Position.Y - k_GapSpaceShipFromBottom);
                m_SpaceShipBulletList.Add(currentSpaceShipBullet);
                Game.Components.Add(currentSpaceShipBullet);
            }

            m_PreviousKeyboardState = i_KeyboardState;
            m_PreviousMouseState = i_MouseState;
        }

        private void shipBulletsUpdate(GameTime i_GameTime)
        {
            for (int i = 0; i < m_SpaceShipBulletList.Count; i++)
            {
                SpaceShipBullet currentSpaceShipBullet = m_SpaceShipBulletList[i];
                if (currentSpaceShipBullet.Position.Y < 0 || !currentSpaceShipBullet.Visible)
                {
                    m_SpaceShipBulletList.Remove(currentSpaceShipBullet);
                    Game.Components.Remove(currentSpaceShipBullet);
                    currentSpaceShipBullet.Visible = false;
                }
            }
        }
    }
}
