using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Entities.Platforms;
using Platformer.Shared;

namespace Platformer.Entities.Hazards
{
	class RetractableSpikes : Hazard
	{
		private const int EXTENDED_DURATION = 1500;
		private const int PARTIALLY_EXTENDED_DURATION = 1000;
		private const int RETRACTED_DURATION = 3000;
		private const int PARTIAL_EXTENSION_DURATION = 100;
		private const int FULL_EXTENSION_DURATION = 100;
		private const int RETRACT_DURATION = 100;
		private const int PARTIAL_EXTENSION_LENGTH = 4;

		private enum ExtensionStates
		{
			PARTIAL,
			FULL,
			RETRACT,
			NONE
		}

		private static Texture2D texture;
		private static int textureWidth;

		static RetractableSpikes()
		{
			texture = ContentLoader.LoadTexture("Hazards/SpikeUp");
			textureWidth = texture.Width;
		}

		private Vector2 position;
		private Timer movementTimer;
		private Timer stateTimer;
		private ExtensionStates extensionState;

		private int numSpikes;
		private int platformTop;
		private int partialExtensionTarget;
		private int fullExtensionTarget;

		public RetractableSpikes(Platform platform) :
			base(HazardTypes.RETRACTABLE_SPIKES)
		{
			//Rectangle platformBox = platform.BoundingBox;

			//numSpikes = platformBox.Width / textureWidth;
			//platformTop = platformBox.Top;
			//partialExtensionTarget = platformTop - PARTIAL_EXTENSION_LENGTH;
			//fullExtensionTarget = platformTop - texture.Height;

			//position = new Vector2(platformBox.Left, platformTop);
			//BoundingBox = new Rectangle(platformBox.X, platformTop - texture.Height, numSpikes * textureWidth, texture.Height);
			//stateTimer = new Timer(RETRACTED_DURATION, PartiallyExtend, false);
			//extensionState = ExtensionStates.NONE;
		}

		private void PartiallyExtend()
		{
			movementTimer = new Timer(PARTIAL_EXTENSION_DURATION, EndPartialExtension, false);
			stateTimer = new Timer(PARTIALLY_EXTENDED_DURATION, FullyExtend, false);
			extensionState = ExtensionStates.PARTIAL;
		}

		private void FullyExtend()
		{
			movementTimer = new Timer(FULL_EXTENSION_DURATION, EndFullExtension, false);
			stateTimer = new Timer(EXTENDED_DURATION, Retract, false);
			extensionState = ExtensionStates.FULL;
		}

		private void Retract()
		{
			movementTimer = new Timer(RETRACT_DURATION, EndRetract, false);
			stateTimer = new Timer(RETRACTED_DURATION, PartiallyExtend, false);
			extensionState = ExtensionStates.RETRACT;
			Active = false;
		}

		private void EndPartialExtension()
		{
			position.Y = partialExtensionTarget;
			extensionState = ExtensionStates.NONE;
		}

		private void EndFullExtension()
		{
			position.Y = fullExtensionTarget;
			extensionState = ExtensionStates.NONE;
			Active = true;
		}

		private void EndRetract()
		{
			position.Y = platformTop;
			extensionState = ExtensionStates.NONE;
		}

		public override void Destroy()
		{
			if (movementTimer != null)
			{
				movementTimer.Destroy = true;
			}

			stateTimer.Destroy = true;
		}

		public override void Update(float dt)
		{
			switch (extensionState)
			{
				case ExtensionStates.PARTIAL:
					position.Y = MathHelper.Lerp(platformTop, partialExtensionTarget, movementTimer.Progress);
					break;

				case ExtensionStates.FULL:
					position.Y = MathHelper.Lerp(partialExtensionTarget, fullExtensionTarget, movementTimer.Progress);
					break;

				case ExtensionStates.RETRACT:
					position.Y = MathHelper.Lerp(fullExtensionTarget, platformTop, movementTimer.Progress);
					break;
			}
		}

		public override void Draw(SpriteBatch sb)
		{
			Vector2 drawPosition = position;

			for (int i = 0; i < numSpikes; i++)
			{
				sb.Draw(texture, drawPosition, Color.White);

				drawPosition.X += textureWidth;
			}
		}
	}
}
