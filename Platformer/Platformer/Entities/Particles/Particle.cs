using System;
using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Shared;

namespace Platformer.Entities.Particles
{
    public enum ParticleTypes
    {
		FIRE,
        EMBER,
		ASH,
		SPECK,
		INVALID
    }

	abstract class Particle
	{
		private static Texture2D spritesheet;
		private static ParticleAttributes[] attributeList;

		static Particle()
		{
			spritesheet = ContentLoader.LoadTexture("Particles");
			attributeList = new ParticleAttributes[Enum.GetNames(typeof(ParticleTypes)).Length - 1];

			foreach (XElement particleElement in XDocument.Load(Constants.XML_FILEPATH + "Particles.xml").Root.Elements("Particle"))
			{
				ParticleTypes particleType = ConvertToParticleType(particleElement.Attribute("Type").Value);

				string[] tokens = particleElement.Value.Split(' ');

				int x = int.Parse(tokens[0]);
				int y = int.Parse(tokens[1]);
				int width = int.Parse(tokens[2]);
				int height = int.Parse(tokens[3]);

				attributeList[(int)particleType] = new ParticleAttributes(new Rectangle(x, y, width, height));
			}
		}

		public static ParticleTypes ConvertToParticleType(string particleType)
		{
			switch (particleType)
			{
				case "Fire":
					return ParticleTypes.FIRE;

				case "Ember":
					return ParticleTypes.EMBER;

				case "Ash":
					return ParticleTypes.ASH;

				case "Speck":
					return ParticleTypes.SPECK;
			}

			return ParticleTypes.INVALID;
		}

        private float rotation;
        private float angularVelocity;

		private ParticleAttributes attributes;
        private Timer lifetimeTimer;

		public Particle(ParticleTypes type, Vector2 position, Vector2 velocity, float minAngularVelocity,
            float maxAngularVelocity, int minLifetime, int maxLifetime)
		{
			attributes = attributeList[(int)type];

			Position = position;
            Velocity = velocity;
            Scale = Vector2.One;
            Color = Color.White;
            angularVelocity = Functions.GetRandomFloat(minAngularVelocity, maxAngularVelocity);

            lifetimeTimer = new Timer(Functions.GetRandomInt(minLifetime, maxLifetime), Destroy, false);
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
            sb.Draw(spritesheet, Position, attributes.SourceRect, Color, rotation, attributes.Origin, Scale, SpriteEffects.None, 0);
        }

        protected class ParticleAttributes
        {
			public ParticleAttributes(Rectangle? sourceRect)
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
