using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Entities.Platforms
{
	abstract class SetPiece
	{
		public static void Initialize(List<Platform> platforms)
		{
			PlatformMasterList = platforms;
		}

		protected static List<Platform> PlatformMasterList { get; private set; }

		protected SetPiece(int numPlatforms)
		{
			Platforms = new List<Platform>(numPlatforms);

			for (int i = 0; i < numPlatforms; i++)
			{
				Platforms.Add(new Platform());
			}

			OriginalNumPlatforms = numPlatforms;
			PlatformMasterList.AddRange(Platforms);
		}

		protected List<Platform> Platforms { get; set; }
		protected int OriginalNumPlatforms { get; private set; }

		public bool Destroyed { get; private set; }

		protected abstract void InitializePlatforms();

		public virtual void Update(float dt)
		{
			if (Platforms.Count == 0)
			{
				Destroyed = true;
			}
		}

		public abstract void Draw(SpriteBatch sb);
	}
}
