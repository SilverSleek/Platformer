using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Entities;
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
		DOWN,
		LEFT,
		RIGHT
	}

	public class Game1 : Game
	{
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		private InputManager inputManager;
		private EventManager eventManager;
		private TimerManager timerManager;

		private Player player;
		private Lava lava;
		private List<Platform> platforms;

		private CollisionHelper collisionHelper;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = Constants.SCREEN_WIDTH;
			graphics.PreferredBackBufferHeight = Constants.SCREEN_HEIGHT;

			Content.RootDirectory = "Content";
			Window.Title = "Platformer";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			ContentLoader.Initialize(Content);

			inputManager = new InputManager();
			eventManager = new EventManager();
			timerManager = new TimerManager();

			player = new Player();
			lava = new Lava();
			platforms = new List<Platform>();
			platforms.Add(new Platform(new Vector2(400, 400)));

			collisionHelper = new CollisionHelper(player, lava, platforms);

			base.Initialize();
		}
		
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		protected override void Update(GameTime gameTime)
		{
			float dt = (float)gameTime.ElapsedGameTime.Milliseconds / 1000;

			inputManager.Update();
			eventManager.Update();
			timerManager.Update(dt);

			player.Update(dt);
			lava.Update(dt);
			collisionHelper.Update();

			Camera.Instance.Update();
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.White);

			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default,
				RasterizerState.CullCounterClockwise, null, Camera.Instance.Transform);

			foreach (Platform platform in platforms)
			{
				platform.Draw(spriteBatch);
			}

			player.Draw(spriteBatch);
			lava.Draw(spriteBatch);

			spriteBatch.End();
		}
	}
}
