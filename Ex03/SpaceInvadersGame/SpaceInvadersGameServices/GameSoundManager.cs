using Microsoft.Xna.Framework;
using Infrastructure.GamesObjects.Managers;
using Infrastructure.GamesObjects.ServiceInterfaces;

namespace SpaceInvadersGame.SpaceInvadersGameServices
{
    public static class GameSoundManager
    {
        private const string k_BackgroundMusicAsset = @"Sounds\BGMusic";
        private const string k_BarrierHitAsset = @"Sounds\BarrierHit";
        private const string k_EnemyKillAsset = @"Sounds\EnemyKill";
        private const string k_GameOverAsset = @"Sounds\GameOver";
        private const string k_LevelWinAsset = @"Sounds\LevelWin";
        private const string k_LifeDieAsset = @"Sounds\LifeDie";
        private const string k_MenuMoveAsset = @"Sounds\MenuMove";
        private const string k_MotherShipKillAsset = @"Sounds\MotherShipKill";
        private const string k_SSGunShotAsset = @"Sounds\SSGunShot";
        private const string k_EnemyGunShotAsset = @"Sounds\EnemyGunShot";

        public static void LoadGameSounds(Game i_Game)
        {
            SoundManager soundManager = (SoundManager)i_Game.Services.GetService(typeof(ISoundManager));
            soundManager.SetBackgroundMusic(k_BackgroundMusicAsset);
            soundManager.AddEffect("BarrierHit", k_BarrierHitAsset);
            soundManager.AddEffect("EnemyKill", k_EnemyKillAsset);
            soundManager.AddEffect("GameOver", k_GameOverAsset);
            soundManager.AddEffect("LevelWin", k_LevelWinAsset);
            soundManager.AddEffect("LifeDie", k_LifeDieAsset);
            soundManager.AddEffect("MenuMove", k_MenuMoveAsset);
            soundManager.AddEffect("MotherShipKill", k_MotherShipKillAsset);
            soundManager.AddEffect("SSGunShot", k_SSGunShotAsset);
            soundManager.AddEffect("EnemyGunShot", k_EnemyGunShotAsset);
        }
    }
}
