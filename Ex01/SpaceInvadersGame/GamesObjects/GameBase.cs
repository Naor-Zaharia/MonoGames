using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GamesObjects
{
   public class GameBase : Game
    {
        protected SpriteBatch m_SpriteBatch;

        public SpriteBatch SpriteBatch
        {
            get
            {
                return m_SpriteBatch;
            }

            set
            {
                m_SpriteBatch = value;
            }
        }
    }
}
