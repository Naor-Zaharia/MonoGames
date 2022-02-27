using GameScreens.Screens;
using SpaceInvadersGame.SpaceInvadersGameScreens.Menus;

namespace SpaceInvadersGame.SpaceInvadersGameServices.SpaceInvadersGameInterfaces
{
    public interface ISpaceInvaderGameManagerService
    {
        PlayScreen PlayScreen { get; set; }

        MainMenuScreen MainMenuScreen { get; set; }

        int Level { get; }

        int AmountOfPlayers { get; }

        int EnemyMatrixWidthLevel { get; }

        float XVelocityOfBarriersLevel { get; }

        int ExtraValueForEnemiesLevel { get; }

        string GameOverScoreString { get; set; }

        void StepUpLevel();

        void IncreaseAmountOfPlayers();

        void InitGameManager();

        void GameManagerRematch();
    }
}
