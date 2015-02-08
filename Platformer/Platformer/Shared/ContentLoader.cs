using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Shared
{
	class ContentLoader
	{
		private static ContentManager content;

		public static void Initialize(ContentManager content)
		{
			ContentLoader.content = content;
		}

		public static Texture2D LoadTexture(string filename)
		{
			return content.Load<Texture2D>("Textures/" + filename);
		}
	}
}
