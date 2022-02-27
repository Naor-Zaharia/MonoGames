using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Infrastructure.GamesObjects.Managers;
using Infrastructure.GamesObjects.ServiceInterfaces;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using SpaceInvadersGame.SpaceInvadersUtils;

namespace SpaceInvadersGame.SpaceInvadersObjects
{
    internal class Gun
    {
        private const string k_BulletAssetName = @"Sprites\Bullet";
        private const string k_EnemyShootingSoundName = "EnemyGunShot";
        private const string k_SpaceshipShootingSoundName = "SSGunShot";
        private const string k_SSGunShotAsset = @"Sounds\SSGunShot";
        private const string k_EnemyGunShotAsset = @"Sounds\EnemyGunShot";
        private const float k_Half = 0.5f;
        private const float k_BulletInterval = 7f;
        private readonly SoundManager r_SoundManager;
        private GameScreen m_GameScreen;
        private List<Bullet> m_BulletsList;
        private Sprite m_OwnerOfShootMachine;
        private int m_MaximalAmountsOfBullets;

        internal Gun(GameScreen i_GameScreen, int i_MaximalAmountsOfBullets, Sprite i_OwnerOfShootMachine)
        {
            this.m_GameScreen = i_GameScreen;
            this.r_SoundManager = (SoundManager) i_GameScreen.Game.Services.GetService(typeof(ISoundManager));
            this.m_MaximalAmountsOfBullets = i_MaximalAmountsOfBullets;
            this.m_OwnerOfShootMachine = i_OwnerOfShootMachine;
            this.m_BulletsList = createBulletsList();
        }

        public void RebuildGunBullets()
        {
            this.m_BulletsList = createBulletsList();
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
                return m_BulletsList;
            }
        }

        private List<Bullet> createBulletsList()
        {
            List<Bullet> bulletsList = new List<Bullet>();

            for (int i = 0; i < m_MaximalAmountsOfBullets; i++)
            {
                bulletsList.Add(new Bullet(m_GameScreen, k_BulletAssetName, 0, 0, getBulletTypeForGun()));
                bulletsList[i].Visible = false;
                bulletsList[i].Enabled = false;
            }

            return bulletsList;
        }

        private SpaceInvadersEnums.eBulletType getBulletTypeForGun()
        {
            SpaceInvadersEnums.eBulletType bulletType;

            if (m_OwnerOfShootMachine is Spaceship)
            {
                bulletType = SpaceInvadersEnums.eBulletType.SpaceshipBullet;
            }
            else
            {
                bulletType = SpaceInvadersEnums.eBulletType.EnemyBullet;
            }

            return bulletType;
        }

        public void CleanAllBullets()
        {
            foreach (Bullet currentBullet in m_BulletsList)
            {
                m_GameScreen.Remove(currentBullet);
                currentBullet.CleanBullet();
            }
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

                if(OwnerOfShootMachine is Spaceship)
                {
                    r_SoundManager.PlaySound(k_SpaceshipShootingSoundName);                  
                }

                if(OwnerOfShootMachine is Enemy)
                {
                    r_SoundManager.PlaySound(k_EnemyShootingSoundName);                    
                }
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
            foreach (Bullet currentBullet in m_BulletsList)
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