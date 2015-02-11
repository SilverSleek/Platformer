using System.Collections.Generic;

using Microsoft.Xna.Framework;
using System;

using Microsoft.Xna.Framework.Graphics;

using Platformer.Entities.Events;
using Platformer.Interfaces;
using Platformer.Shared;

namespace Platformer.Entities
{
	class Lava : IEventListener
	{
		private const int NUM_SEGMENTS = 25;
		private const int NUM_SINE_WAVES = 2;
		private const int MIN_AMPLITUDE = 50;
		private const int MAX_AMPLITUDE = 60;
		private const int MIN_FREQUENCY = 1;
		private const int MAX_FREQUENCY = 2;
		private const int MIN_CYCLE_SPEED = 12000;
		private const int MAX_CYCLE_SPEED = 18000;
		private const int ASCENSION_SPEED = 0;//50;

		private Random random;
		private Texture2D whitePixel;
		private Vector2[] points;
		private SineWave[] sineWaves;

		private float totalAscension;
		private float increment;
		private float totalMilliseconds;

		public Lava()
		{
			random = new Random();
			whitePixel = ContentLoader.LoadTexture("WhitePixel");
			totalAscension = -Constants.SCREEN_HEIGHT;
			increment = (float)Constants.SCREEN_WIDTH / NUM_SEGMENTS;

			GeneratePoints();
			GenerateSineWaves();

			SimpleEvent.AddEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.RESET, this));
		}

		private void GeneratePoints()
		{
			points = new Vector2[NUM_SEGMENTS + 1];

			for (int i = 0; i < points.Length; i++)
			{
				points[i].X = (float)i / (NUM_SEGMENTS) * Constants.SCREEN_WIDTH;
			}
		}

		private void GenerateSineWaves()
		{
			sineWaves = new SineWave[NUM_SINE_WAVES];

			for (int i = 0; i < sineWaves.Length; i++)
			{
				float amplitude = GetRandomValue(MIN_AMPLITUDE, MAX_AMPLITUDE);
				float frequency = GetRandomValue(MIN_FREQUENCY, MAX_FREQUENCY);
				float cycleSpeed = GetRandomValue(MIN_CYCLE_SPEED, MAX_CYCLE_SPEED);

				sineWaves[i] = new SineWave(amplitude, frequency, cycleSpeed);
			}
		}

		private float GetRandomValue(int min, int max)
		{
			return (float)random.NextDouble() * (max - min) + min;
		}

		public bool CheckSubmerged(Vector2 point)
		{
			int index = (int)(point.X / increment);

			Vector2 a = points[index];
			Vector2 b = points[index + 1];

			// taken from http://stackoverflow.com/questions/1560492/how-to-tell-whether-a-point-is-to-the-right-or-left-side-of-a-line
			return Math.Sign((b.X - a.X) * (point.Y - a.Y) - (b.Y - a.Y) * (point.X - a.X)) == 1;
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			GenerateSineWaves();

			totalAscension = -Constants.SCREEN_HEIGHT;
		}

		public void Update(float dt)
		{
			totalAscension += ASCENSION_SPEED * dt;
			totalMilliseconds += dt * 1000;

			for (int i = 0; i < points.Length; i++)
			{
				float amount = (float)i / points.Length;
				float y = -totalAscension;

				foreach (SineWave wave in sineWaves)
				{
					float timeOffset = totalMilliseconds / wave.CycleSpeed * MathHelper.TwoPi;

					y -= (float)Math.Sin(amount * wave.Frequency * MathHelper.TwoPi + timeOffset) * wave.Amplitude;
				}

				points[i].Y = y;
			}
		}

		public void Draw(SpriteBatch sb)
		{
			for (int i = 0; i < points.Length - 1; i++)
			{
				DrawingFunctions.DrawLine(sb, points[i], points[i + 1], Color.Red);
			}
		}

		private class SineWave
		{
			public SineWave(float amplitude, float frequency, float cycleSpeed)
			{
				Amplitude = amplitude;
				Frequency = frequency;
				CycleSpeed = cycleSpeed;
			}

			public float Amplitude { get; private set; }
			public float Frequency { get; private set; }
			public float CycleSpeed { get; private set; }
		}
	}
}
