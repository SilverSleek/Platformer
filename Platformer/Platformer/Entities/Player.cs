using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Platformer.Entities.Events;
using Platformer.Interfaces;
using Platformer.Shared;

namespace Platformer.Entities
{
	class Player : ISimpleEventListener
	{
		private const int ACCELERATION = 2000;
		private const int DECELERATION = 1700;
		private const int MAX_SPEED = 300;

		private Vector2 position;
		private Vector2 velocity;
		private Vector2 acceleration;
		private Sprite sprite;

		public Player()
		{
			sprite = new Sprite(ContentLoader.LoadTexture("Player"), null, Vector2.Zero, Color.White);

			SimpleEvent.AddListener(EventTypes.KEYBOARD, this);
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			KeyboardEventData data = (KeyboardEventData)simpleEvent.Data;

			bool aDown = false;
			bool dDown = false;

			foreach (Keys key in data.KeysDown)
			{
				if (key == Keys.A)
				{
					aDown = true;
				}
				else if (key == Keys.D)
				{
					dDown = true;
				}
			}

			if (aDown && !dDown)
			{
				acceleration.X = -ACCELERATION;
			}
			else if (dDown && !aDown)
			{
				acceleration.X = ACCELERATION;
			}
			else
			{
				acceleration.X = 0;
			}
		}

		private void HandleRunning(KeyboardEventData data)
		{
		}

		private void HandleJumping(KeyboardEventData data)
		{
		}

		public void Update(float dt)
		{
			velocity += acceleration * dt;

			CorrectVelocity(dt);

			position += velocity * dt;
			sprite.Position = position;
		}

		private void CorrectVelocity(float dt)
		{
			if (velocity.X > MAX_SPEED)
			{
				velocity.X = MAX_SPEED;
			}
			else if (velocity.X < -MAX_SPEED)
			{
				velocity.X = -MAX_SPEED;
			}
			else if (acceleration.X == 0)
			{
				if (velocity.X > 0)
				{
					velocity.X -= DECELERATION * dt;

					if (velocity.X < 0)
					{
						velocity.X = 0;
					}
				}
				else if (velocity.X < 0)
				{
					velocity.X += DECELERATION * dt;

					if (velocity.X > 0)
					{
						velocity.X = 0;
					}
				}
			}
		}

		public void Draw(SpriteBatch sb)
		{
			sprite.Draw(sb);
		}
	}
}
