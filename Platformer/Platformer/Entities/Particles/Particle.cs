using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Shared;

namespace Platformer.Entities.Particles
{
    public enum ParticleTypes
    {
        ASH,
        EMBER
    }

	abstract class Particle
	{
		static Particle()
		{
			Spritesheet = ContentLoader.LoadTexture("Particles");

            SpriteInfoList = new ParticleSpriteInfo[2];
            SpriteInfoList[(int)ParticleTypes.ASH] = new ParticleSpriteInfo(new Rectangle(6, 0, 12, 8));
            SpriteInfoList[(int)ParticleTypes.EMBER] = new ParticleSpriteInfo(new Rectangle(0, 0, 6, 6));
		}

		protected static Texture2D Spritesheet { get; private set; }
        protected static ParticleSpriteInfo[] SpriteInfoList { get; private set; }

        private float rotation;
        private float angularVelocity;

        private ParticleTypes type;
        private Timer lifetimeTimer;

		public Particle(ParticleTypes type, Vector2 position, Vector2 velocity, float minAngularVelocity,
            float maxAngularVelocity, int minLifetime, int maxLifetime)
		{
            this.type = type;

			Position = position;
            Velocity = velocity;
            Scale = Vector2.One;
            Color = Color.White;
            angularVelocity = Functions.GetRandomValue(minAngularVelocity, maxAngularVelocity);

            lifetimeTimer = new Timer((int)Functions.GetRandomValue(minLifetime, maxLifetime), Destroy, false);
		}

		protected Vector2 Position { get; private set; }
        protected Vector2 Velocity { get; set; }
        protected Vector2 Scale { get; set; }
        protected Color Color { get; set; }

		public bool Destroyed { get; private set; }

		public virtual void Destroy()
		{
			Destroyed = true;
            lifetimeTimer.Destroy = true;
		}

		public virtual void Update(float dt)
		{
			Position += Velocity * dt;
            rotation += angularVelocity * dt;
		}

        public void Draw(SpriteBatch sb)
        {
            int index = (int)type;

            sb.Draw(Spritesheet, Position, SpriteInfoList[index].SourceRect, Color, rotation,
                SpriteInfoList[index].Origin, Scale, SpriteEffects.None, 0);
        }

        protected class ParticleSpriteInfo
        {
            public ParticleSpriteInfo(Rectangle? sourceRect)
            {
                Rectangle rectangle = (Rectangle)sourceRect;

                SourceRect = sourceRect;
                Origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
            }

            public Rectangle? SourceRect { get; private set; }
            public Vector2 Origin { get; private set; }
        }
	}
}
