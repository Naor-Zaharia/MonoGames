using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceInvadersGame;
using SpaceInvadersGame.SpaceInvadersGameScreens;
using SpaceInvadersGame.SpaceInvadersObjects;
using SpaceInvadersGame.SpaceInvadersUtils;

namespace GameScreens.Screens
{
    public class PlayScreen : SpaceInvadersGameScreen
    {
        private const string k_GameOverSoundName = "GameOver";
        private const string k_LevelWinSoundName = "LevelWin";
        private readonly PauseScreen r_PauseScreen;
        private EnemiesMatrix m_EnemiesMatrix;
        private MotherShip m_MotherShip;
        private SpaceInvaderPlayer m_FirstPlayer;
        private SpaceInvaderPlayer m_SecondPlayer;
        private Barriers m_Barriers;
        private int m_CurrentLivePlayer;

        public PlayScreen(Game i_Game)
            : base(i_Game)
        {
            this.r_PauseScreen = new PauseScreen(Game);
            this.BlendState = BlendState.NonPremultiplied;
        }

        public override void Initialize()
        {
            if (r_GameManager.Level == 1)
            {
                base.Initialize();
                InitGame();
            }
            else
            {
                generateNewLevel();
            }
        }

        public void CreateGame()
        {
            this.m_EnemiesMatrix = new EnemiesMatrix(this);
            this.Add(m_EnemiesMatrix);
            this.m_MotherShip = new MotherShip(this, 0, 0, Vector2.Zero, SpaceInvadersEnums.eEnemyType.MotherShipEnemy);
            this.Add(m_MotherShip);
            this.m_FirstPlayer = new SpaceInvaderPlayer(this, SpaceInvadersEnums.ePlayerType.FirstPlayer);
            this.m_Barriers = new Barriers(this, m_FirstPlayer.Spaceship.Position.Y - m_FirstPlayer.Spaceship.GapSpaceShipFromBottom);
            this.m_FirstPlayer.Initialize();
            this.m_FirstPlayer.GameEnded += playerOutOfGame;
            this.m_EnemiesMatrix.GameEnded += levelEndedOnWin;
            this.m_EnemiesMatrix.EnemiesYUpdate += m_FirstPlayer.IsEnemiesArriveToSpaceship;
            if (r_GameManager.AmountOfPlayers == 2)
            {
                this.m_SecondPlayer = new SpaceInvaderPlayer(this, SpaceInvadersEnums.ePlayerType.SecondPlayer);
                this.m_SecondPlayer.Initialize();
                this.m_SecondPlayer.GameEnded += playerOutOfGame;
                this.m_EnemiesMatrix.EnemiesYUpdate += m_SecondPlayer.IsEnemiesArriveToSpaceship;
            }
            else
            {
                this.m_SecondPlayer = null;
            }

            this.m_CurrentLivePlayer = r_GameManager.AmountOfPlayers;
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            if (InputManager.KeyPressed(Keys.P))
            {
                m_ScreensManager.SetCurrentScreen(r_PauseScreen);
            }
        }

        public void InitGame()
        {
            r_GameManager.InitGameLevel();
            cleanOldComponents();
            CreateGame();
        }

        private void cleanOldComponents()
        {
            if (m_Barriers != null)
            {
                m_Barriers.Dispose();
            }

            if (m_FirstPlayer != null)
            {
                m_FirstPlayer.Dispose();
            }

            if (m_SecondPlayer != null)
            {
                m_SecondPlayer.Dispose();
            }

            if (m_EnemiesMatrix != null)
            {
                m_EnemiesMatrix.CleanEnemiesMatrixData();
                m_EnemiesMatrix.Dispose();
            }

            if (m_MotherShip != null)
            {
                m_MotherShip.CleanEnemy();
            }
        }

        private void initComponentsForNewLevel()
        {
            if (m_EnemiesMatrix != null)
            {
                m_EnemiesMatrix.CleanEnemiesMatrixData();
            }

            if (m_FirstPlayer != null)
            {
                m_FirstPlayer.Spaceship.Gun.CleanAllBullets();
                m_FirstPlayer.Spaceship.Gun.RebuildGunBullets();
            }

            if (m_SecondPlayer != null)
            {
                m_SecondPlayer.Spaceship.Gun.CleanAllBullets();
                m_SecondPlayer.Spaceship.Gun.RebuildGunBullets();
            }
        }

        private void generateNewLevel()
        {
            initComponentsForNewLevel();
            this.m_EnemiesMatrix.CreateEnemiesMatrix();
            this.m_Barriers.RebuildBarriers();
        }

        private bool isGameEndOnWin()
        {
            return m_EnemiesMatrix.AmountOfLivingEnemies == 0;
        }

        private void levelEndedOnWin()
        {
            r_GameManager.StepUpLevel();
            r_SoundManager.PlaySound(k_LevelWinSoundName);
            initSpaceshipPosition(new List<SpaceInvaderPlayer> { m_FirstPlayer, m_SecondPlayer });
            ExitScreen();
            ScreensManager.SetCurrentScreen(new LevelTransitionScreen(Game, r_GameManager.Level));
        }

        private void initSpaceshipPosition(List<SpaceInvaderPlayer> i_SpaceInvadersPlayers)
        {
            foreach (SpaceInvaderPlayer currentPlayer in i_SpaceInvadersPlayers)
            {
                if (currentPlayer != null)
                {
                    currentPlayer.Spaceship.InitSpaceshipPosition();
                }
            }
        }

        private void playerOutOfGame()
        {
            this.m_CurrentLivePlayer--;
            if (this.m_CurrentLivePlayer == 0)
            {
                r_SoundManager.PlaySound(k_GameOverSoundName);
                r_GameManager.InitGameLevel();
                generateScoreStringForGameOverScreen(new List<SpaceInvaderPlayer> { m_FirstPlayer, m_SecondPlayer });
                setWinnerNameStringForGameOverScreen();
                ExitScreen();
                m_ScreensManager.SetCurrentScreen(new GameOverScreen(Game));
            }
        }

        private void generateScoreStringForGameOverScreen(List<SpaceInvaderPlayer> i_SpaceInvadersPlayers)
        {
            StringBuilder gameScoreMsg = new StringBuilder();

            foreach (SpaceInvaderPlayer currentPlayer in i_SpaceInvadersPlayers)
            {
                if (currentPlayer != null)
                {
                    gameScoreMsg.AppendFormat(
                        @"{0}
",
                        currentPlayer.PlayerScoreString());
                }
            }

            r_GameManager.GameOverScoreString = gameScoreMsg.ToString();
        }

        private void setWinnerNameStringForGameOverScreen()
        {
            string gameWinnerName = string.Empty;

            if (r_GameManager.AmountOfPlayers == 1 || m_FirstPlayer.SpaceShipScore > m_SecondPlayer.SpaceShipScore)
            {
                gameWinnerName = m_FirstPlayer.PlayerName;
            }

            if (r_GameManager.AmountOfPlayers != 1 && m_FirstPlayer.SpaceShipScore < m_SecondPlayer.SpaceShipScore)
            {
                gameWinnerName = m_SecondPlayer.PlayerName;
            }

            r_GameManager.SetWinnerNameString(gameWinnerName);
        }
    }
}
