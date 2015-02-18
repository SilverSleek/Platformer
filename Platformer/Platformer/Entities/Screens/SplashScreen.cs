using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Entities.Events;
using Platformer.Shared;

namespace Platformer.Entities.Screens
{
	class SplashScreen
	{
		private const int FADE_DELAY = 500;
		private const int FADE_DURATION = 500;
		private const int VISIBLE_DURATION = 3000;
		private const int GAMESTATE_DELAY = 500;
		private const int VERTICAL_OFFSET = 250;
		private const int LOGO_OFFSET = 25;

		private enum FadeStates
		{
			FADE_IN,
			FADE_OUT,
			NONE
		}

		private Text text;
		private Sprite logoSprite;
		private FadeStates fadeState;
		private Timer timer;

		public SplashScreen()
		{
			SpriteFont font = ContentLoader.LoadFont("Splash");

			string textValue = "Silver Sleek Studios";

			Vector2 textPosition = new Vector2(Constants.SCREEN_WIDTH / 2, VERTICAL_OFFSET);
			Vector2 spritePosition = textPosition - new Vector2(font.MeasureString(textValue).X / 2 + LOGO_OFFSET, 0);

			logoSprite = new Sprite(ContentLoader.LoadTexture("Logo"), spritePosition);
			text = new Text(font, textValue, textPosition, OriginLocations.CENTER, Color.White); 

			ChangeFadeState(FadeStates.NONE, Color.Transparent, FADE_DELAY, BeginFadeIn);
		}

		private void BeginFadeIn()
		{
			ChangeFadeState(FadeStates.FADE_IN, Color.Transparent, FADE_DURATION, EndFadeIn);
		}

		private void EndFadeIn()
		{
			ChangeFadeState(FadeStates.NONE, Color.White, VISIBLE_DURATION, BeginFadeOut);
		}

		private void BeginFadeOut()
		{
			ChangeFadeState(FadeStates.FADE_OUT, Color.White, FADE_DURATION, EndFadeOut);
		}

		private void EndFadeOut()
		{
			ChangeFadeState(FadeStates.NONE, Color.Transparent, GAMESTATE_DELAY, () => {
				SimpleEvent.AddEvent(EventTypes.GAMESTATE, Gamestates.TITLE); });
		}

		private void ChangeFadeState(FadeStates fadeState, Color color, int duration, Action trigger)
		{
			this.fadeState = fadeState;

			logoSprite.Color = color;
			text.Color = color;
			timer = new Timer(duration, trigger, false);
		}

		public void Update(float dt)
		{
			if (fadeState != FadeStates.NONE)
			{
				Color color = Color.White;

				if (fadeState == FadeStates.FADE_IN)
				{
					color = Color.Lerp(Color.Transparent, Color.White, timer.Progress);
				}
				else if (fadeState == FadeStates.FADE_OUT)
				{
					color = Color.Lerp(Color.White, Color.Transparent, timer.Progress);
				}

				logoSprite.Color = color;
				text.Color = color;
			}
		}

		public void Draw(SpriteBatch sb)
		{
			logoSprite.Draw(sb);
			text.Draw(sb);
		}
	}
}
