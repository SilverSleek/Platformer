using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Entities;
using Platformer.Entities.Events;
using Platformer.Entities.Hazards;
using Platformer.Interfaces;
using Platformer.Helpers;
using Platformer.Managers;
using Platformer.Shared;

namespace Platformer
{
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

		private PlatformHelper platformHelper;
		private CollisionHelper collisionHelper;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = Constants.SCREEN_WIDTH;
			graphics.PreferredBackBufferHeight = Constants.SCREEN_HEIGHT;

			Content.RootDirectory = "Content";
			Window.Title = "Platformer";
			IsMouseVisible = true;

			SimpleEvent.AddEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.EXIT, this));
		}

		protected override void Initialize()
		{
			ContentLoader.Initialize(Content);

			inputManager = new InputManager();
			eventManager = new EventManager();
			timerManager = new TimerManager();

			player = new Player();
			lava = new Lava();
			background = new Background();

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
			Exit();
		}

		protected override void Update(GameTime gameTime)
		{
			float dt = (float)gameTime.ElapsedGameTime.Milliseconds / 1000 / TIME_FACTOR;

			inputManager.Update();
			eventManager.Update();
			timerManager.Update(dt);

			lava.Update(dt);
			platformHelper.Update(dt);
			player.Update(dt);
			collisionHelper.Update();

			Camera.Instance.Update();
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.White);

			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default,
				RasterizerState.CullCounterClockwise, null, Camera.Instance.Transform);

			//background.Draw(spriteBatch);
			platformHelper.Draw(spriteBatch);
			player.Draw(spriteBatch);
			lava.Draw(spriteBatch);

			spriteBatch.End();
		}
	}
}
