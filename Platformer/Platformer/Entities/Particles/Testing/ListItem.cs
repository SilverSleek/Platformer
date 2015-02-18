using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Shared;

namespace Platformer.Entities.Particles.Testing
{
	class ListItem
	{
		private static Texture2D backgroundTexture;
		private static SpriteFont font;

		private static int boxWidth;
		private static int boxHeight;
		private static float padding;

		static ListItem()
		{
			backgroundTexture = ContentLoader.LoadTexture("SelectionBackground");
			font = ContentLoader.LoadFont("Selection");

			boxWidth = backgroundTexture.Width;
			boxHeight = backgroundTexture.Height;
			padding = (boxHeight - font.MeasureString("A").Y) / 2;
		}

		public static int BoxWidth { get { return boxWidth; } }
		public static int BoxHeight { get { return boxHeight; } }

		private Text text;
		private Vector2 localPosition;
		private Vector2 backgroundPosition;

		public ListItem(string textValue, Vector2 localPosition)
		{
			localPosition += new Vector2(padding);

			this.localPosition = localPosition;

			text = new Text(font, textValue, Vector2.Zero, OriginLocations.TOP_LEFT, Color.Black);
			BoundingBox = new BoundingBox2D(Vector2.Zero, boxWidth, boxHeight);
		}

		public bool Active { get; set; }

		public BoundingBox2D BoundingBox { get; private set; }

		public void Activate(Vector2 masterPosition)
		{
			backgroundPosition = masterPosition + localPosition;
			text.Position = backgroundPosition + new Vector2(padding);
		}

		public void Update()
		{
		}

		public void Draw(SpriteBatch sb)
		{
			sb.Draw(backgroundTexture, backgroundPosition, Color.White);
			text.Draw(sb);
		}
	}
}
