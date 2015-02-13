using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Entities.Events;
using Platformer.Entities.Particles;
using Platformer.Interfaces;
using Platformer.Factories;
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
		private const int ASCENSION_SPEED = 50;

		private const int NUM_VERTICES_PER_SEGMENT = 30;
		private const int NUM_TRIANGLES_PER_SEGMENT = 10;
		private const int NUM_VERTICES_PER_SUBSEGMENT = 6;
		private const int WHITE_HEIGHT = 25;
		private const int YELLOW_HEIGHT = 25;
		private const int ORANGE_HEIGHT = 100;
		private const int RED_HEIGHT = 525;

        private const int MIN_EMBER_SPEED = 75;
        private const int MAX_EMBER_SPEED = 125;
        private const int EMBER_SPAWN_DEPTH = 10;
        private const float AVERAGE_NUM_EMBERS_PER_SECOND = 1.5f;

		private GraphicsDevice graphicsDevice;
		private BasicEffect basicEffect;
		private Texture2D whitePixel;
        private Random random;

		private Vector2[] points;
		private SineWave[] sineWaves;
		private VertexPositionColor[] vertexData;

		private float totalAscension;
		private float increment;
		private float totalMilliseconds;

		private int numPoints;
		private int primitiveCount;

		public Lava(GraphicsDevice graphicsDevice)
		{
			this.graphicsDevice = graphicsDevice;

			whitePixel = ContentLoader.LoadTexture("WhitePixel");
			totalAscension = -Constants.SCREEN_HEIGHT;
            increment = (float)Constants.SCREEN_WIDTH / NUM_SEGMENTS;
            random = new Random();

			basicEffect = new BasicEffect(graphicsDevice);
			basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, 0, 0, 1);
			basicEffect.VertexColorEnabled = true;

			GeneratePoints();
			GenerateSineWaves();
			ComputeVertexData();

			SimpleEvent.AddEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.RESET, this));
		}

		private void GeneratePoints()
		{
			numPoints = NUM_SEGMENTS + 1;
			points = new Vector2[numPoints];

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
				float amplitude = Functions.GetRandomValue(MIN_AMPLITUDE, MAX_AMPLITUDE);
				float frequency = Functions.GetRandomValue(MIN_FREQUENCY, MAX_FREQUENCY);
				float cycleSpeed = Functions.GetRandomValue(MIN_CYCLE_SPEED, MAX_CYCLE_SPEED);

				sineWaves[i] = new SineWave(amplitude, frequency, cycleSpeed);
			}
		}

		private void ComputeVertexData()
		{
			// Each vertical "line" from each point goes white -> yellow -> orange -> red -> bottom, or five sections. Each section needs
			// 6 vertices to form two triangles, for a total of 10 triangles and 30 vertices per segment.
			vertexData = new VertexPositionColor[NUM_SEGMENTS * NUM_VERTICES_PER_SEGMENT];
			primitiveCount = NUM_SEGMENTS * NUM_TRIANGLES_PER_SEGMENT;

			VertexPositionColor[] whiteVertices = new VertexPositionColor[numPoints];
			VertexPositionColor[] yellowVertices = new VertexPositionColor[numPoints];
			VertexPositionColor[] orangeVertices = new VertexPositionColor[numPoints];
			VertexPositionColor[] redVertices = new VertexPositionColor[numPoints];
			VertexPositionColor[] bottomVertices = new VertexPositionColor[numPoints];

			for (int i = 0; i < numPoints; i++)
			{
				Vector3 whitePoint = new Vector3(points[i], 0);
				Vector3 yellowPoint = new Vector3(whitePoint.X, whitePoint.Y + WHITE_HEIGHT, 0);
				Vector3 orangePoint = new Vector3(yellowPoint.X, yellowPoint.Y + YELLOW_HEIGHT, 0);
				Vector3 redPoint = new Vector3(orangePoint.X, orangePoint.Y + ORANGE_HEIGHT, 0);
				Vector3 bottomPoint = new Vector3(orangePoint.X, orangePoint.Y + RED_HEIGHT, 0);

				whiteVertices[i] = new VertexPositionColor(whitePoint, Color.White);
				yellowVertices[i] = new VertexPositionColor(yellowPoint, Color.Yellow);
				orangeVertices[i] = new VertexPositionColor(orangePoint, Color.Orange);
				redVertices[i] = new VertexPositionColor(redPoint, Color.Red);
				bottomVertices[i] = new VertexPositionColor(bottomPoint, Color.Black);
			}

			for (int i = 0; i < NUM_SEGMENTS; i++)
			{
				int whiteIndex = i * NUM_VERTICES_PER_SEGMENT;
				int yellowIndex = whiteIndex + NUM_VERTICES_PER_SUBSEGMENT;
				int orangeIndex = yellowIndex + NUM_VERTICES_PER_SUBSEGMENT;
				int redIndex = orangeIndex + NUM_VERTICES_PER_SUBSEGMENT;

				AddVertices(whiteVertices, yellowVertices, whiteIndex, i);
				AddVertices(yellowVertices, orangeVertices, yellowIndex, i);
				AddVertices(orangeVertices, redVertices, orangeIndex, i);
				AddVertices(redVertices, bottomVertices, redIndex, i);
			}
		}

		private void AddVertices(VertexPositionColor[] vertices1, VertexPositionColor[] vertices2, int dataIndex, int i)
		{
			// Triangles for each subsegment are constructed as follows:
			//
			// 0,4 ___ 5
			//    |\  |
			//    | \ |
			//    |__\|
			//   2     1,3
			//
			vertexData[dataIndex] = vertices1[i];
			vertexData[dataIndex + 1] = vertices2[i + 1];
			vertexData[dataIndex + 2] = vertices2[i];
			vertexData[dataIndex + 3] = vertices2[i + 1];
			vertexData[dataIndex + 4] = vertices1[i];
			vertexData[dataIndex + 5] = vertices1[i + 1];
		}

		public bool CheckSubmerged(Vector2 point)
		{
			int index = (int)(point.X / increment);

			// this can happen with linear set pieces
			if (index < 0 || index > points.Length - 2)
			{
				return false;
			}

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

			//UpdateVertices();

            CheckGenerateEmber(dt);
		}

        private void CheckGenerateEmber(float dt)
        {
            if ((float)random.NextDouble() <= AVERAGE_NUM_EMBERS_PER_SECOND * dt)
            {
                Vector2 position = points[(int)Functions.GetRandomValue(0, points.Length)];
                Vector2 velocity = new Vector2(0, -Functions.GetRandomValue(MIN_EMBER_SPEED, MAX_EMBER_SPEED));

                position.Y += EMBER_SPAWN_DEPTH;

                ParticleFactory.CreateParticle(ParticleTypes.EMBER, position, velocity);
            }
        }

		private void UpdateVertices()
		{
			float[] whiteYValues = new float[numPoints];
			float[] yellowYValues = new float[numPoints];
			float[] orangeYValues = new float[numPoints];
			float[] redYValues = new float[numPoints];
			float[] bottomYValues = new float[numPoints];

			ComputeYValues(whiteYValues, 0);
			ComputeYValues(yellowYValues, WHITE_HEIGHT);
			ComputeYValues(orangeYValues, WHITE_HEIGHT + YELLOW_HEIGHT);
			ComputeYValues(redYValues, WHITE_HEIGHT + YELLOW_HEIGHT + ORANGE_HEIGHT);
			ComputeYValues(bottomYValues, WHITE_HEIGHT + YELLOW_HEIGHT + ORANGE_HEIGHT + RED_HEIGHT);

			for (int i = 0; i < NUM_SEGMENTS; i++)
			{
				int whiteIndex = i * NUM_VERTICES_PER_SEGMENT;;
				int yellowIndex = whiteIndex + NUM_VERTICES_PER_SUBSEGMENT;
				int orangeIndex = yellowIndex + NUM_VERTICES_PER_SUBSEGMENT;
				int redIndex = orangeIndex + NUM_VERTICES_PER_SUBSEGMENT;
				int bottomIndex = redIndex + NUM_VERTICES_PER_SUBSEGMENT;

				UpdateTriangles(whiteIndex, whiteYValues[i], whiteYValues[i + 1], yellowYValues[i], yellowYValues[i + 1]);
				UpdateTriangles(yellowIndex, yellowYValues[i], yellowYValues[i + 1], orangeYValues[i], orangeYValues[i + 1]);
				UpdateTriangles(orangeIndex, orangeYValues[i], orangeYValues[i + 1], redYValues[i], redYValues[i + 1]);
				UpdateTriangles(redIndex, redYValues[i], redYValues[i + 1], bottomYValues[i], bottomYValues[i + 1]);
			}
		}

		private void ComputeYValues(float[] yValues, int verticalOffset)
		{
			for (int i = 0; i < points.Length; i++)
			{
				yValues[i] = points[i].Y + verticalOffset;
			}
		}

		private void UpdateTriangles(int startingIndex, float leftY1, float rightY1, float leftY2, float rightY2)
		{
			// Same picture copied from above.
			//
			// 0,4 ___ 5
			//    |\  |
			//    | \ |
			//    |__\|
			//   2     1,3
			//
			vertexData[startingIndex].Position.Y = leftY1;
			vertexData[startingIndex + 1].Position.Y = rightY2;
			vertexData[startingIndex + 2].Position.Y = leftY2;
			vertexData[startingIndex + 3].Position.Y = rightY2;
			vertexData[startingIndex + 4].Position.Y = leftY1;
			vertexData[startingIndex + 5].Position.Y = rightY1;
		}

		public void Draw(SpriteBatch sb)
		{
            for (int i = 0; i < points.Length - 1; i++)
            {
                DrawingFunctions.DrawLine(sb, points[i], points[i + 1], Color.Red);
            }

			//basicEffect.CurrentTechnique.Passes[0].Apply();
			//graphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertexData, 0, primitiveCount);
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
