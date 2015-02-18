using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Entities.Particles.Testing
{
	class SelectionList
	{
		private const int SPACING = 25;

		private ListItem[] items;

		public SelectionList(Type enumType)
		{
			string[] names = GetNames(enumType);

			items = new ListItem[names.Length];

			Vector2 localPosition = Vector2.Zero;

			for (int i = 0; i < names.Length; i++)
			{
				items[i] = new ListItem(names[i], localPosition);
				localPosition.Y += SPACING;
			}
		}

		public bool Active { get; set; }

		public BoundingBox2D BoundingBox { get; private set; }

		private string[] GetNames(Type enumType)
		{
			string[] names = Enum.GetNames(enumType);

			for (int i = 0; i < names.Length; i++)
			{
				string name = names[i].ToLower();
				name = char.ToUpper(name[0]) + name.Substring(1);
				names[i] = name;
			}

			return names;
		}

		public void Activate(Vector2 position)
		{
			Active = !Active;

			for (int i = 0; i < items.Length; i++)
			{
				if (Active)
				{
					items[i].Activate(position);
				}
				else
				{
					items[i].Active = false;
				}
			}
		}

		public void Update()
		{
		}

		public void Draw(SpriteBatch sb)
		{
			foreach (ListItem item in items)
			{
				item.Draw(sb);
			}
		}
	}
}
