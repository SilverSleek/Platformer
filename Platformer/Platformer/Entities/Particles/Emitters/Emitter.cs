using System;
using System.Diagnostics;
using System.Xml.Linq;

using Microsoft.Xna.Framework;

using Platformer.Shared;

namespace Platformer.Entities.Particles.Emitters
{
	public enum EmitterTypes
	{
		FIRE,
		INVALID
	}

	abstract class Emitter
	{
		static Emitter()
		{
			AttributeList = new EmitterAttributes[Enum.GetNames(typeof(EmitterTypes)).Length - 1];

			foreach (XElement emitterElement in XDocument.Load(Constants.XML_FILEPATH + "Emitters.xml").Root.Elements("Emitter"))
			{
				EmitterTypes emitterType = ConvertToEmitterType(emitterElement.Attribute("Type").Value);

				Debug.Assert(emitterType != EmitterTypes.INVALID);

				ParticleTypes particleType = Particle.ConvertToParticleType(emitterElement.Element("ParticleType").Value);

				int minDelay = int.Parse(emitterElement.Element("MinDelay").Value);
				int maxDelay = int.Parse(emitterElement.Element("MaxDelay").Value);
				int minParticles = int.Parse(emitterElement.Element("MinParticles").Value);
				int maxParticles = int.Parse(emitterElement.Element("MaxParticles").Value);
				int minParticleSpeedX = int.Parse(emitterElement.Element("MinParticleSpeedX").Value);
				int maxParticleSpeedX = int.Parse(emitterElement.Element("MaxParticleSpeedX").Value);
				int minParticleSpeedY = int.Parse(emitterElement.Element("MinParticleSpeedY").Value);
				int maxParticleSpeedY = int.Parse(emitterElement.Element("MaxParticleSpeedY").Value);

				AttributeList[(int)emitterType] = new EmitterAttributes(particleType, minDelay, maxDelay, minParticles, maxParticles,
					minParticleSpeedX, maxParticleSpeedX, minParticleSpeedY, maxParticleSpeedY);
			}
		}

		private static EmitterTypes ConvertToEmitterType(string emitterType)
		{
			switch (emitterType)
			{
				case "Fire":
					return EmitterTypes.FIRE;
			}

			return EmitterTypes.INVALID;
		}

		protected static EmitterAttributes[] AttributeList { get; private set; }

		public Emitter(EmitterTypes type)
		{
			Attributes = AttributeList[(int)type];
		}

		protected EmitterAttributes Attributes { get; private set; }

		public abstract void Emit();

		protected class EmitterAttributes
		{
			public EmitterAttributes(ParticleTypes particleType, int minDelay, int maxDelay, int minParticles, int maxParticles,
				int minParticleSpeedX, int maxParticleSpeedX, int minParticleSpeedY, int maxParticleSpeedY)
			{
				ParticleType = particleType;
				MinDelay = minDelay;
				MaxDelay = maxDelay;
				MinParticles = minParticles;
				MaxParticles = maxParticles;
				MinParticleSpeedX = minParticleSpeedX;
				MaxParticleSpeedX = maxParticleSpeedX;
				MinParticleSpeedY = minParticleSpeedY;
				MaxParticleSpeedY = maxParticleSpeedY;
			}

			public ParticleTypes ParticleType { get; private set; }

			public int MinDelay { get; private set; }
			public int MaxDelay { get; private set; }
			public int MinParticles { get; private set; }
			public int MaxParticles { get; private set; }
			public int MinParticleSpeedX { get; private set; }
			public int MaxParticleSpeedX { get; private set; }
			public int MinParticleSpeedY { get; private set; }
			public int MaxParticleSpeedY { get; private set; }
		}
	}
}
