using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Entities.Hazards
{
	abstract class Hazard
	{
		public Rectangle BoundingBox { get; protected set; }

		public abstract void Draw(SpriteBatch sb);
	}
}
