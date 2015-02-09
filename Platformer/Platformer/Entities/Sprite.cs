using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Entities
{
	class Sprite
	{
		private Texture2D texture;
		private Rectangle? sourceRect;
		private Vector2 origin;

		public Sprite(Texture2D texture, Rectangle? sourceRect, Vector2 position, Color color)
		{
			this.texture = texture;
			this.sourceRect = sourceRect;

			Position = position;
			origin = new Vector2(texture.Width, texture.Height) / 2;
			Color = color;
		}

		public Vector2 Position { get; set; }
		public Color Color { get; set; }

		public float Rotation { get; set; }

		public void Draw(SpriteBatch sb)
		{
			sb.Draw(texture, Position, sourceRect, Color, Rotation, origin, 1, SpriteEffects.None, 0);
		}
	}
}
