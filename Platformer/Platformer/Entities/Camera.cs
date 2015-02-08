using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Shared;

namespace Platformer.Entities
{
	class Camera
	{
		public static Camera Instance { get; private set; }

		static Camera()
		{
			Instance = new Camera();
		}

		private Vector2 screenPosition;

		private Camera()
		{
			screenPosition = Vector2.Zero;
			VisibleArea = new Rectangle(-(int)screenPosition.X, -(int)screenPosition.Y, Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT); 

			Zoom = 1;
			Transform = Matrix.Identity;
			InverseTransform = Matrix.Identity;
		}

		public Vector2 Position { get; set; }
		public Rectangle VisibleArea { get; private set; }

		public float Rotation { get; set; }
		public float Zoom { get; set; }

		public Matrix Transform { get; private set; }
		public Matrix InverseTransform { get; private set; }

		public void Update()
		{
			Vector2 correctedPosition = new Vector2((int)Position.X, (int)Position.Y);

			Rectangle visibleArea = VisibleArea;
			visibleArea.X = (int)(correctedPosition.X - screenPosition.X);
			visibleArea.Y = (int)(correctedPosition.Y - screenPosition.Y);
			VisibleArea = visibleArea;

			Transform = Matrix.CreateTranslation(new Vector3(correctedPosition, 0)) * Matrix.CreateRotationZ(Rotation) * 
				Matrix.CreateScale(Zoom) * Matrix.CreateTranslation(new Vector3(screenPosition, 0));
			InverseTransform = Matrix.Invert(Transform);
		}
	}
}
