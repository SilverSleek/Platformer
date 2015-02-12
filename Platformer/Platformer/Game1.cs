using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Entities;
using Platformer.Entities.Events;
using Platformer.Entities.Hazards;
using Platformer.Entities.Particles;
using Platformer.Entities.Screens;
using Platformer.Interfaces;
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

		private Player player;
		private Lava lava;
		private Background background;
		private SplashScreen splashScreen;

		private List<Ash> ashes;

		private PlatformHelper platformHelper;
		private CollisionHelper collisionHelper;

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

			SwitchGamestate(Gamestates.SPLASH);

			player = new Player();
			lava = new Lava(GraphicsDevice);
			background = new Background();

			ashes = new List<Ash>();

			for (int i = 0; i < 25; i++)
			{
				float x = Functions.GetRandomValue(0, Constants.SCREEN_WIDTH);
				float y = Functions.GetRandomValue(0, Constants.SCREEN_HEIGHT);
				float velocityX = Functions.GetRandomValue(-4, 4);
				float velocityY = Functions.GetRandomValue(10, 20);

				ashes.Add(new Ash(new Vector2(x, y), new Vector2(velocityX, velocityY)));
			}

			List<Platform> platforms = new List<Platform>();
			List<Hazard> hazards = new List<Hazard>();

			platformHelper = new PlatformHelper(platforms, lava);
			collisionHelper = new CollisionHelper(player, lava, platforms, hazards);

			Platform.Initialize(hazards);

			SimpleEvent.AddEvent(EventTypes.RESET, null);

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
			}
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
					foreach (Ash ash in ashes)
					{
						ash.Update(dt);
					}

					lava.Update(dt);
					platformHelper.Update(dt);
					player.Update(dt);
					collisionHelper.Update();

					Camera.Instance.Update();

					break;
			}
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default,
				RasterizerState.CullCounterClockwise, null, Camera.Instance.Transform);

			switch (gamestate)
			{
				case Gamestates.SPLASH:
					splashScreen.Draw(spriteBatch);
					break;

				case Gamestates.TITLE:
					break;

				case Gamestates.GAMEPLAY:
					//background.Draw(spriteBatch);
					platformHelper.Draw(spriteBatch);
					player.Draw(spriteBatch);
					lava.Draw(spriteBatch);

					foreach (Ash ash in ashes)
					{
						ash.Draw(spriteBatch);
					}

					break;
			}

			spriteBatch.End();
		}
	}
}
