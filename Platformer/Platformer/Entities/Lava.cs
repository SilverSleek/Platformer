using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Shared;

namespace Platformer.Entities
{
	class Lava
	{
		private const int ASCENSION_SPEED = 25;

		private Texture2D whitePixel;

		public Lava()
		{
			whitePixel = ContentLoader.LoadTexture("WhitePixel");
			Level = Constants.SCREEN_HEIGHT;
		}
		
		public float Level { get; private set; } 

		public void Update(float dt)
		{
			Level -= ASCENSION_SPEED * dt;
		}

		public void Draw(SpriteBatch sb)
		{
			Rectangle destinationRect = new Rectangle(0, (int)Level, Constants.SCREEN_WIDTH, 1);

			sb.Draw(whitePixel, destinationRect, Color.Red);
		}
	}
}
