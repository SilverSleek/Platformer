using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Platformer.Entities.Events;
using Platformer.Entities.Hazards;
using Platformer.Entities.Platforms;
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
		private Vector2 platformOffset;

		private Platform platform;
		private Sprite sprite;
		private HeightDisplay heightDisplay;

		private bool airborne;
		private bool doubleJumpEnabled;
		private bool doubleJumpActive;
		private bool heightOffsetRecorded;

		private int health;
		private int heightOffset;

		public Player(HeightDisplay heightDisplay)
		{
			this.heightDisplay = heightDisplay;

			Texture2D texture = ContentLoader.LoadTexture("Player");

			int width = texture.Width;
			int height = texture.Height;

			sprite = new Sprite(texture, position);
			halfBounds = new Vector2(width, height) / 2;
			OldBoundingBox = new BoundingBox2D(position, width, height);
			NewBoundingBox = new BoundingBox2D(position, width, height);
			heightOffset = int.MinValue;

			ResetValues();

			SimpleEvent.AddEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.KEYBOARD, this));
			SimpleEvent.AddEvent(EventTypes.LISTENER, new ListenerEventData(EventTypes.RESET, this));
		}

		public BoundingBox2D OldBoundingBox { get; private set; }
		public BoundingBox2D NewBoundingBox { get; private set; }

		public void RegisterPlatformCollision(Platform platform)
		{
			this.platform = platform;

			position.Y = platform.BoundingBox.Top - halfBounds.Y;
			velocity.Y = 0;
			airborne = false;
			doubleJumpEnabled = true;
			doubleJumpActive = false;

			if (platform.Moving)
			{
				platformOffset = position - platform.BoundingBox.Center;
			}
			
			if (!heightOffsetRecorded)
			{
				heightOffset = (int)position.Y;
				heightOffsetRecorded = true;
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

						break;

					case CollisionDirections.RIGHT:
						velocity.X = -COLLISION_LAUNCH_SPEED_HORIZONTAL;

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
			heightOffsetRecorded = false;
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
			else if (platform != null)
			{
				if (platform.Destroyed || OffPlatform())
				{
					airborne = true;
					platform = null;
				}
				else if (platform.Moving)
				{
					//platformOffset += velocity * dt;
					position = platform.BoundingBox.Center + platformOffset;
				}
			}

			CorrectVelocity(dt);

			position += velocity * dt;

			if (heightOffsetRecorded)
			{
				heightDisplay.SetValue(heightOffset - (int)position.Y);
			}

			UpdateValues();
		}

		private bool OffPlatform()
		{
			BoundingBox2D platformBox = platform.BoundingBox;

			return !platformBox.Contains(groundTestingPointLeft) && !platformBox.Contains(groundTestingPointRight);
		}

		private void UpdateValues()
		{
			sprite.Position = position;
			OldBoundingBox.SetCenter(NewBoundingBox.Center);

			BoundingBox2D newBoundingBox = NewBoundingBox;
			newBoundingBox.SetCenter(position);

			groundTestingPointLeft = new Vector2(newBoundingBox.Left, newBoundingBox.Bottom + GROUND_TESTING_OFFSET);
			groundTestingPointRight = new Vector2(newBoundingBox.Right, newBoundingBox.Bottom + GROUND_TESTING_OFFSET);

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
