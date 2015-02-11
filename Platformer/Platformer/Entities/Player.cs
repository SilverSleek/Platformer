using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Platformer.Entities.Events;
using Platformer.Entities.Hazards;
using Platformer.Interfaces;
using Platformer.Shared;

namespace Platformer.Entities
{
	class Player : IEventListener
	{
		private const int ACCELERATION = 2000;
		private const int DECELERATION = 1700;
		private const int MAX_SPEED = 300;
		private const int JUMP_SPEED_INITIAL = 400;
		private const int JUMP_SPEED_LIMITED = 150;
		private const int DOUBLE_JUMP_SPEED_INITIAL = 550;
		private const int DOUBLE_JUMP_SPEED_LIMITED = 200;
		private const int COLLISION_LAUNCH_SPEED_VERTICAL = 750;
		private const int COLLISION_LAUNCH_SPEED_HORIZONTAL = 300;
		private const int GROUND_TESTING_OFFSET = 2;
		private const int STARTING_HEALTH = 3;

		private Vector2 position;
		private Vector2 velocity;
		private Vector2 acceleration;
		private Vector2 halfBounds;
		private Vector2 groundTestingPointLeft;
		private Vector2 groundTestingPointRight;

		private Platform platform;
		private Sprite sprite;

		private bool airborne;
		private bool doubleJumpEnabled;
		private bool doubleJumpActive;

		private int health;

		public Player()
		{
			Texture2D texture = ContentLoader.LoadTexture("Player");

			sprite = new Sprite(texture, position);
			halfBounds = new Vector2(texture.Width, texture.Height) / 2;
			NewBoundingBox = new Rectangle(0, 0, texture.Width, texture.Height);

			ResetValues();

			SimpleEvent.AddEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.KEYBOARD, this));
			SimpleEvent.AddEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.RESET, this));
		}

		public Rectangle OldBoundingBox { get; private set; }
		public Rectangle NewBoundingBox { get; private set; }

		public void RegisterPlatformCollision(Platform platform, CollisionDirections direction, float value)
		{
			this.platform = platform;

			switch (direction)
			{
				case CollisionDirections.DOWN:
					position.Y = value - halfBounds.Y;
					velocity.Y = 0;
					airborne = false;
					doubleJumpEnabled = true;
					doubleJumpActive = false;

					break;

				case CollisionDirections.LEFT:
					break;

				case CollisionDirections.RIGHT:
					break;
			}

			UpdateValues();
		}

		public void RegisterDamage(CollisionDirections direction)
		{
			health--;

			if (health == 0)
			{
				SimpleEvent.AddEvent(EventTypes.RESET, null);
			}
			else
			{
				switch (direction)
				{
					case CollisionDirections.UP:
						if (velocity.Y < COLLISION_LAUNCH_SPEED_VERTICAL)
						{
							velocity.Y = COLLISION_LAUNCH_SPEED_VERTICAL;
						}

						break;

					case CollisionDirections.DOWN:
						if (velocity.Y > -COLLISION_LAUNCH_SPEED_VERTICAL)
						{
							velocity.Y = -COLLISION_LAUNCH_SPEED_VERTICAL;
						}

						break;

					case CollisionDirections.LEFT:
						velocity.X = COLLISION_LAUNCH_SPEED_HORIZONTAL;
						launched = true;

						break;

					case CollisionDirections.RIGHT:
						velocity.X = -COLLISION_LAUNCH_SPEED_HORIZONTAL;
						launched = true;

						break;
				}

				if (doubleJumpActive)
				{
					doubleJumpEnabled = true;
					doubleJumpActive = false;
				}
			}
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
				ResetValues();
			}
		}

		private void ResetValues()
		{
			position = new Vector2(Constants.SCREEN_WIDTH / 2, 300);
			velocity = Vector2.Zero;
			acceleration = Vector2.Zero;
			airborne = true;
			doubleJumpEnabled = true;
			doubleJumpActive = false;
			health = STARTING_HEALTH;

			UpdateValues();
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
			bool jumpPressedThisFrame = CheckJumpKey(data.KeysPressedThisFrame);
			bool jumpReleasedThisFrame = CheckJumpKey(data.KeysReleasedThisFrame);

			if (!airborne)
			{
				if (jumpPressedThisFrame)
				{
					velocity.Y = -JUMP_SPEED_INITIAL;
					airborne = true;
				}
			}
			else
			{
				if (doubleJumpEnabled && jumpPressedThisFrame)
				{
					velocity.Y = -DOUBLE_JUMP_SPEED_INITIAL;
					doubleJumpEnabled = false;
					doubleJumpActive = true;
				}
				else if (doubleJumpActive && jumpReleasedThisFrame && velocity.Y < -DOUBLE_JUMP_SPEED_LIMITED)
				{
					velocity.Y = -DOUBLE_JUMP_SPEED_LIMITED;
				}
				else if (jumpReleasedThisFrame && velocity.Y < -JUMP_SPEED_LIMITED)
				{
					velocity.Y = -JUMP_SPEED_LIMITED;
				}
			}
		}

		private bool CheckJumpKey(List<Keys> keys)
		{
			foreach (Keys key in keys)
			{
				if (key == Keys.W || key == Keys.Space)
				{
					return true;
				}
			}

			return false;
		}

		public void Update(float dt)
		{
			velocity += acceleration * dt;

			if (airborne)
			{
				velocity.Y += Constants.GRAVITY * dt;
			}
			else if (OffPlatform())
			{
				airborne = true;
			}

			UpdateVelocity(dt);

			position += velocity * dt;

			UpdateValues();
		}

		private bool OffPlatform()
		{
			return !platform.BoundingBox.Contains((int)groundTestingPointLeft.X, (int)groundTestingPointLeft.Y) &&
				!platform.BoundingBox.Contains((int)groundTestingPointRight.X, (int)groundTestingPointRight.Y);
		}

		private void UpdateValues()
		{
			sprite.Position = position;
			OldBoundingBox = NewBoundingBox;

			Rectangle newBoundingBox = NewBoundingBox;
			newBoundingBox.X = (int)(position.X - halfBounds.X);
			newBoundingBox.Y = (int)(position.Y - halfBounds.Y);
			NewBoundingBox = newBoundingBox;

			groundTestingPointLeft = new Vector2(newBoundingBox.Left, newBoundingBox.Bottom + GROUND_TESTING_OFFSET);
			groundTestingPointRight = new Vector2(newBoundingBox.Right, newBoundingBox.Bottom + GROUND_TESTING_OFFSET);

			Vector2 cameraPosition = Camera.Instance.Position;
			cameraPosition.Y = position.Y;
			Camera.Instance.Position = cameraPosition;
		}

		private void UpdateVelocity(float dt)
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
