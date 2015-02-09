using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Platformer.Entities.Events;
using Platformer.Interfaces;
using Platformer.Shared;

namespace Platformer.Entities
{
	class Player : IEventListener
	{
		private const int ACCELERATION = 2000;
		private const int DECELERATION = 1700;
		private const int MAX_SPEED = 300;
		private const int JUMP_SPEED_INITIAL = 600;
		private const int JUMP_SPEED_LIMITED = 200;

		private Vector2 position;
		private Vector2 velocity;
		private Vector2 acceleration;
		private Vector2 halfBounds;
		private Sprite sprite;

		private bool airborne;

		public Player()
		{
			Texture2D texture = ContentLoader.LoadTexture("Player");

			position = new Vector2(Constants.SCREEN_WIDTH / 2, 100);
			sprite = new Sprite(texture, position);
			halfBounds = new Vector2(texture.Width, texture.Height) / 2;
			NewBoundingBox = new Rectangle(0, 0, texture.Width, texture.Height);
			airborne = true;

			SimpleEvent.AddEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.KEYBOARD, this));
			SimpleEvent.AddEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.RESET, this));
		}

		public Rectangle OldBoundingBox { get; private set; }
		public Rectangle NewBoundingBox { get; private set; }

		public void RegisterCollision(CollisionDirections direction, float value)
		{
			switch (direction)
			{
				case CollisionDirections.DOWN:
					airborne = false;
					position.Y = value - halfBounds.Y;
					velocity.Y = 0;

					break;

				case CollisionDirections.LEFT:
					break;

				case CollisionDirections.RIGHT:
					break;
			}

			UpdateValues();
		}

		public void EventResponse(SimpleEvent simpleEvent)
		{
			EventTypes eventType = simpleEvent.Type;

			if (eventType == EventTypes.KEYBOARD)
			{
				KeyboardEventData data = (KeyboardEventData)simpleEvent.Data;

				HandleRunning(data);
				HandleJumping(data);
			}
			else
			{
				position = new Vector2(Constants.SCREEN_WIDTH / 2, 100);
				velocity = Vector2.Zero;
				acceleration = Vector2.Zero;
				airborne = true;

				UpdateValues();
			}
		}

		private void HandleRunning(KeyboardEventData data)
		{
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

		private void HandleJumping(KeyboardEventData data)
		{
			if (!airborne)
			{
				bool wPressedThisFrame = false;

				foreach (Keys key in data.KeysPressedThisFrame)
				{
					if (key == Keys.W)
					{
						wPressedThisFrame = true;

						break;
					}
				}

				if (wPressedThisFrame)
				{
					velocity.Y = -JUMP_SPEED_INITIAL;
					airborne = true;
				}
			}
			else
			{
				bool wReleasedThisFrame = false;

				foreach (Keys key in data.KeysReleasedThisFrame)
				{
					if (key == Keys.W)
					{
						wReleasedThisFrame = true;

						break;
					}
				}

				if (wReleasedThisFrame && velocity.Y < -JUMP_SPEED_LIMITED)
				{
					velocity.Y = -JUMP_SPEED_LIMITED;
				}
			}
		}

		public void Update(float dt)
		{
			velocity += acceleration * dt;

			if (airborne)
			{
				velocity.Y += Constants.GRAVITY * dt;
			}

			CorrectVelocity(dt);

			position += velocity * dt;

			UpdateValues();
		}

		private void UpdateValues()
		{
			sprite.Position = position;
			OldBoundingBox = NewBoundingBox;

			Rectangle newBoundingBox = NewBoundingBox;
			newBoundingBox.X = (int)(position.X - halfBounds.X);
			newBoundingBox.Y = (int)(position.Y - halfBounds.Y);
			NewBoundingBox = newBoundingBox;

			Vector2 cameraPosition = Camera.Instance.Position;
			cameraPosition.Y = position.Y;
			Camera.Instance.Position = cameraPosition;
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
