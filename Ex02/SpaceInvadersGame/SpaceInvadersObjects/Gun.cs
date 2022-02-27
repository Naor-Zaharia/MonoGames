using System.Collections.Generic;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;

namespace SpaceInvadersGame.SpaceInvadersObjects
{
    internal class Gun
    {
        private const string k_BulletAssetName = @"Sprites\Bullet";
        private const float k_Half = 0.5f;
        private const float k_BulletInterval = 7f;
        private readonly List<Bullet> r_BulletsList;
        private Game m_Game;
        private Sprite m_OwnerOfShootMachine;
        private int m_MaximalAmountsOfBullets;

        internal Gun(Game i_Game, int i_MaximalAmountsOfBullets, Sprite i_OwnerOfShootMachine)
        {
            this.m_Game = i_Game;
            this.m_MaximalAmountsOfBullets = i_MaximalAmountsOfBullets;
            this.m_OwnerOfShootMachine = i_OwnerOfShootMachine;
            this.r_BulletsList = createBulletsList();
        }

        internal Sprite OwnerOfShootMachine
        {
            get
            {
                return m_OwnerOfShootMachine;
            }

            set
            {
                m_OwnerOfShootMachine = value;
            }
        }

        internal List<Bullet> BulletsList
        {
            get
            {
                return r_BulletsList;
            }
        }

        private List<Bullet> createBulletsList()
        {
            List<Bullet> bulletsList = new List<Bullet>();
            bool BulletDirecton = m_OwnerOfShootMachine is Spaceship;
            for (int i = 0; i < m_MaximalAmountsOfBullets; i++)
            {
                bulletsList.Add(new Bullet(m_Game, k_BulletAssetName, 0, 0, BulletDirecton));
                bulletsList[i].Visible = false;
                bulletsList[i].Enabled = false;
            }

            return bulletsList;
        }

        internal void ShootBullet()
        {
            Bullet bulletForShoot = getNextUnusedBullet();
            if (bulletForShoot != null && m_OwnerOfShootMachine != null)
            {
                setBulletPosition(bulletForShoot);
                setBulletOwner(bulletForShoot);
                bulletForShoot.Enabled = true;
                bulletForShoot.Visible = true;
            }
        }

        private void setBulletPosition(Bullet i_Bullet)
        {
            float xBulletPosition = ((m_OwnerOfShootMachine.Width * k_Half) + m_OwnerOfShootMachine.Position.X) - (i_Bullet.Width * k_Half);
            float yBulletPosition = m_OwnerOfShootMachine.Position.Y + k_BulletInterval;
            Vector2 bulletPosition = new Vector2(xBulletPosition, yBulletPosition);
            i_Bullet.Position = bulletPosition;
        }

        private void setBulletOwner(Bullet i_Bullet)
        {
            i_Bullet.BulletOwner = m_OwnerOfShootMachine;
        }

        private Bullet getNextUnusedBullet()
        {
            Bullet bullet = null;
            foreach (Bullet currentBullet in r_BulletsList)
            {
                if (!currentBullet.Visible && !currentBullet.Enabled)
                {
                    bullet = currentBullet;
                    break;
                }
            }

            return bullet;
        }
    }
}
