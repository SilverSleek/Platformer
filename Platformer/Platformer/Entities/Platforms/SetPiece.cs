using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Entities.Platforms
{
	abstract class SetPiece
	{
		protected static List<Platform> platformMasterList;

		public static void Initialize(List<Platform> platforms)
		{
			platformMasterList = platforms;
		}

		protected SetPiece(int numPlatforms)
		{
			Platforms = new Platform[numPlatforms];

			for (int i = 0; i < numPlatforms; i++)
			{
				Platforms[i] = new Platform();
			}

			InitializePlatforms(numPlatforms);

			OriginalNumPlatforms = numPlatforms;
			platformMasterList.AddRange(Platforms);
		}

		protected Platform[] Platforms { get; set; }
		protected int OriginalNumPlatforms { get; private set; }

		public bool Destroyed { get; private set; }

		protected abstract void InitializePlatforms(int numPlatforms);

		public virtual void Update(float dt)
		{
			if (Platforms.Length == 0)
			{
				Destroyed = true;
			}
		}

		public abstract void Draw(SpriteBatch sb);
	}
}
