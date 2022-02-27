using System;
using Infrastructure.GamesObjects.ObjectModel;
using Infrastructure.GamesObjects.ObjectModel.Animators;
using Infrastructure.GamesObjects.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using SpaceInvadersGame.SpaceInvadersUtils;

namespace SpaceInvadersGame.SpaceInvadersObjects
{
    public class SpaceInvaderPlayer : GameComponent
    {
        public event GameEvents.GameEnd.GameEndEventHandler GameEnded = null;

        private const int k_FirstPlayerId = 1;
        private const int k_SecondPlayerId = 2;
        private const string k_FirstSpaceShipAssetName = @"Sprites\Ship01_32x32";
        private const string k_SecondSpaceShipAssetName = @"Sprites\Ship02_32x32";
        private const string k_AnimationManagerName = "AnimatorManager";
        private const string k_FirstPlayerName = "P1";
        private const string k_SecondPlayerName = "P2";
        private const byte k_InitSpaceShipLife = 3;
        private const float k_AnimationLength = 2.6f;
        private const float k_AmountOfSpinPerSec = 6;
        private GameScreen m_GameScreen;
        private Player m_Player;
        private Spaceship m_Spaceship;
        private PlayerLifesBoard m_LifeBoard;
        private PlayerScoreBoard m_ScoreBoard;

        internal SpaceInvaderPlayer(GameScreen i_GameScreen, SpaceInvadersEnums.ePlayerType i_PlayerType) : base(i_GameScreen.Game)
        {
            string assetName = null;
            switch (i_PlayerType)
            {
                case SpaceInvadersEnums.ePlayerType.FirstPlayer:
                    assetName = k_FirstSpaceShipAssetName;
                    this.m_Player = new Player(k_FirstPlayerId, k_FirstPlayerName, Color.DeepSkyBlue, k_InitSpaceShipLife);
                    break;

                case SpaceInvadersEnums.ePlayerType.SecondPlayer:
                    assetName = k_SecondSpaceShipAssetName;
                    this.m_Player = new Player(k_SecondPlayerId, k_SecondPlayerName, Color.Green, k_InitSpaceShipLife);
                    break;
            }

            this.m_Spaceship = new Spaceship(i_GameScreen, m_Player, i_PlayerType);
            this.m_GameScreen = i_GameScreen;
            createPlayerBoards(assetName);
        }

        private void onGameEnd()
        {
            if (GameEnded != null)
            {
                GameEnded.Invoke();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            m_Spaceship.Initialize();
            this.Spaceship.ScoreUpdate += scoreUpdate;
            this.Spaceship.SpaceshipGotHit += spaceshipHitted;
        }
       
        private void createPlayerBoards(string i_AssetName)
        {
            this.m_ScoreBoard = new PlayerScoreBoard(m_GameScreen, m_Player);
            this.m_ScoreBoard.Initialize();
            this.m_LifeBoard = new PlayerLifesBoard(m_GameScreen, i_AssetName, m_Player);
        }

        internal int SpaceShipScore
        {
            get
            {
                return m_Player.PlayerScore;
            }

            set
            {
                m_Player.PlayerScore = value;
                m_ScoreBoard.BoardScoreChange();
            }
        }

        internal Spaceship Spaceship
        {
            get
            {
                return m_Spaceship;
            }
        }

        private void spaceshipHitted()
        {
            m_Player.AmountOfLives--;
            m_LifeBoard.RemoveLife();

            if (m_Player.AmountOfLives == 0)
            {
                m_Spaceship.Dispose();
                animationSpaceshipDying();
                onGameEnd();
            }
        }

        internal void IsEnemiesArriveToSpaceship(Enemy i_HighestYLiveEnemy)
        {
            if (i_HighestYLiveEnemy != null && i_HighestYLiveEnemy.Position.Y + i_HighestYLiveEnemy.Height > Game.GraphicsDevice.Viewport.Height - m_Spaceship.GetYPositionOfSpaceShipFromBottom)
            {
                onGameEnd();
            }
        }

        private void animationSpaceshipDying()
        {
            m_Spaceship.Animations.Pause();
            m_Spaceship.Animations.Remove(k_AnimationManagerName);
            CompositeAnimator compositeAnimator = new CompositeAnimator(m_Spaceship);
            FaderAnimator faderAnimator = new FaderAnimator(TimeSpan.FromSeconds(k_AnimationLength));
            RotatorAnimator rotatorAnimator = new RotatorAnimator(TimeSpan.FromSeconds(k_AnimationLength), TimeSpan.FromSeconds(k_AmountOfSpinPerSec));
            rotatorAnimator.Finished += new EventHandler(disposeSpaceshipComponent);
            compositeAnimator.Add(faderAnimator);
            compositeAnimator.Add(rotatorAnimator);
            m_Spaceship.Animations.Add(compositeAnimator);
            this.m_Spaceship.Animations.Restart();
        }

        private void disposeSpaceshipComponent(object i_Sender, EventArgs i_EventArgs)
        {            
            m_GameScreen.Remove(m_Spaceship);
            m_Spaceship.Dispose();
        }

        private void scoreUpdate(int i_ScoreUpdate)
        {
            m_Player.PlayerScore += i_ScoreUpdate;
            if (m_Player.PlayerScore < 0)
            {
                m_Player.PlayerScore = 0;
            }

            this.m_ScoreBoard.BoardScoreChange();
        }

        internal string PlayerScoreString()
        {
            return m_Player.ScoreString();
        }

        internal string PlayerName
        {
            get
            {
                return m_Player.PlayerName;
            }
        }

        protected override void Dispose(bool i_Disposing)
        {
            base.Dispose(i_Disposing);
            m_Spaceship.CleanSpaceShipComponents();
            m_Spaceship.Dispose();
            m_ScoreBoard.CleanScore();
            m_LifeBoard.CleanLifeBorad();
        }
    }
}
