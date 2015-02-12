using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Entities
{
	class Text
	{
		private SpriteFont font;
		private Vector2 origin;

		public Text(SpriteFont font, string value, Vector2 position, Color color)
		{
			this.font = font;

			Position = position;
			Color = color;

			SetValue(value);
		}

		public string Value { get; private set; }

		public Vector2 Position { get; set; }
		public Color Color { get; set; }

		public void SetValue(string value)
		{
			Value = value;
			origin = font.MeasureString(value) / 2;
		}

		public void Draw(SpriteBatch sb)
		{
			sb.DrawString(font, Value, Position, Color, 0, origin, 1, SpriteEffects.None, 0);
		}
	}
}
