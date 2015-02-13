using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Entities;
using Platformer.Entities.Events;
using Platformer.Entities.Hazards;
using Platformer.Entities.Particles;
using Platformer.Entities.Platforms;
using Platformer.Entities.Screens;
using Platformer.Interfaces;
using Platformer.Factories;
using Platformer.Helpers;
using Platformer.Managers;
using Platformer.Shared;

namespace Platformer
{
	public enum Gamestates
	{
		SPLASH,
		TITLE,
		GAMEPLAY
	}

	public enum ButtonStates
	{
		HELD,
		RELEASED,
		PRESSED_THIS_FRAME,
		RELEASED_THIS_FRAME
	}

	public enum CollisionDirections
	{
		UP,
		DOWN,
		LEFT,
		RIGHT,
		NONE
	}

	class Game1 : Game, IEventListener
	{
		private const int TIME_FACTOR = 1;

		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		private InputManager inputManager;
		private EventManager eventManager;
		private TimerManager timerManager;

		private SplashScreen splashScreen;

		private Player player;
		private Lava lava;
		private Background background;
		private HeightDisplay heightDisplay;

		private PlatformHelper platformHelper;
		private CollisionHelper collisionHelper;
        private ParticleHelper particleHelper;

		private Gamestates gamestate;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = Constants.SCREEN_WIDTH;
			graphics.PreferredBackBufferHeight = Constants.SCREEN_HEIGHT;

			Content.RootDirectory = "Content";
			Window.Title = "Platformer";
			IsMouseVisible = true;

			SimpleEvent.AddEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.EXIT, this));
			SimpleEvent.AddEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.GAMESTATE, this));
		}

		protected override void Initialize()
		{
			ContentLoader.Initialize(Content);

			inputManager = new InputManager();
			eventManager = new EventManager();
			timerManager = new TimerManager();

			SwitchGamestate(Gamestates.GAMEPLAY);

			base.Initialize();
		}
		
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			if (simpleEvent.Type == EventTypes.EXIT)
			{
				Exit();
			}
			else
			{
				SwitchGamestate((Gamestates)simpleEvent.Data);
			}
		}

		private void SwitchGamestate(Gamestates gamestate)
		{
			this.gamestate = gamestate;

			switch (gamestate)
			{
				case Gamestates.SPLASH:
					splashScreen = new SplashScreen();
					break;

				case Gamestates.GAMEPLAY:
					InitializeGameplay();
					break;
			}
		}

		private void InitializeGameplay()
		{
			heightDisplay = new HeightDisplay();
			player = new Player(heightDisplay);
			lava = new Lava(GraphicsDevice);
			background = new Background();

			List<Platform> platforms = new List<Platform>();
			List<Hazard> hazards = new List<Hazard>();
            List<Particle> particles = new List<Particle>();

			platformHelper = new PlatformHelper(platforms, lava);
			collisionHelper = new CollisionHelper(player, lava, platforms, hazards);
            particleHelper = new ParticleHelper(particles);                 

			Platform.Initialize(hazards);
            ParticleFactory.Initialize(particles);

			SimpleEvent.AddEvent(EventTypes.RESET, null);
		}

		protected override void Update(GameTime gameTime)
		{
			float dt = (float)gameTime.ElapsedGameTime.Milliseconds / 1000 / TIME_FACTOR;

			inputManager.Update();
			eventManager.Update();
			timerManager.Update(dt);

			switch (gamestate)
			{
				case Gamestates.SPLASH:
					splashScreen.Update(dt);
					break;

				case Gamestates.GAMEPLAY:
                    UpdateGameplay(dt);
					break;
			}
		}

        private void UpdateGameplay(float dt)
        {
            lava.Update(dt);
            platformHelper.Update(dt);
            player.Update(dt);
            collisionHelper.Update();
            particleHelper.Update(dt);

            Camera.Instance.Update();
        }

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.White);

			switch (gamestate)
			{
				case Gamestates.SPLASH:
					spriteBatch.Begin();
					splashScreen.Draw(spriteBatch);
					spriteBatch.End();

					break;

				case Gamestates.TITLE:
					break;

				case Gamestates.GAMEPLAY:
					DrawGameplay();
					break;
			}
		}

		private void DrawGameplay()
		{
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Camera.Instance.Transform);

			//background.Draw(spriteBatch);
            platformHelper.Draw(spriteBatch);
            particleHelper.Draw(spriteBatch);
			player.Draw(spriteBatch);
			lava.Draw(spriteBatch);
			heightDisplay.Draw(spriteBatch);

			spriteBatch.End();
		}
	}
}
