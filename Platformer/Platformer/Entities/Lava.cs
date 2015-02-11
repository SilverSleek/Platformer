﻿using System.Collections.Generic;

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
		private const int NUM_POINTS = 50;
		private const int NUM_SINE_WAVES = 2;
		private const int MIN_AMPLITUDE = 50;
		private const int MAX_AMPLITUDE = 60;
		private const int MIN_FREQUENCY = 1;
		private const int MAX_FREQUENCY = 2;
		private const int MIN_CYCLE_SPEED = 12000;
		private const int MAX_CYCLE_SPEED = 18000;
		private const int ASCENSION_SPEED = 50;

		private Random random;
		private Texture2D whitePixel;
		private SineWave[] sineWaves;

		private float totalAscension;

		public Lava()
		{
			random = new Random();
			whitePixel = ContentLoader.LoadTexture("WhitePixel");
			totalAscension = -Constants.SCREEN_HEIGHT;

			GeneratePoints();
			GenerateSineWaves();

			SimpleEvent.AddEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.RESET, this));
		}

		public Vector2[] Points { get; private set; }

		private void GeneratePoints()
		{
			Points = new Vector2[NUM_POINTS];

			for (int i = 0; i < Points.Length; i++)
			{
				Vector2 point = Points[i];
				point.X = (float)i / (NUM_POINTS - 1) * Constants.SCREEN_WIDTH;
				Points[i] = point;
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

		public void EventResponse(SimpleEvent simpleEvent)
		{
			GenerateSineWaves();

			totalAscension = -Constants.SCREEN_HEIGHT;
		}

		public void Update(GameTime gameTime, float dt)
		{
			totalAscension += ASCENSION_SPEED * dt;

			float totalMilliseconds = (float)gameTime.TotalGameTime.TotalMilliseconds;

			for (int i = 0; i < NUM_POINTS; i++)
			{
				float amount = (float)i / NUM_POINTS;
				float y = -totalAscension;

				foreach (SineWave wave in sineWaves)
				{
					float timeOffset = totalMilliseconds / wave.CycleSpeed * MathHelper.TwoPi;

					y -= (float)Math.Sin(amount * wave.Frequency * MathHelper.TwoPi + timeOffset) * wave.Amplitude;
				}

				Vector2 point = Points[i];
				point.Y = y;
				Points[i] = point;
			}
		}

		public void Draw(SpriteBatch sb)
		{
			for (int i = 0; i < Points.Length - 1; i++)
			{
				DrawingFunctions.DrawLine(sb, Points[i], Points[i + 1], Color.Red);
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
