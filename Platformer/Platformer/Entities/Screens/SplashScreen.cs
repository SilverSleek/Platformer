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

		private enum FadeStates
		{
			FADE_IN,
			FADE_OUT,
			NONE
		}

		private Sprite logoSprite;
		private FadeStates fadeState;
		private Timer timer;

		public SplashScreen()
		{
			logoSprite = new Sprite(ContentLoader.LoadTexture("Logo"), Vector2.Zero);

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

		private void ChangeFadeState(FadeStates fadeState, Color spriteColor, int duration, Action trigger)
		{
			this.fadeState = fadeState;

			logoSprite.Color = spriteColor;
			timer = new Timer(duration, trigger, false);
		}

		public void Update(float dt)
		{
			if (fadeState == FadeStates.FADE_IN)
			{
				logoSprite.Color = Color.Lerp(Color.Transparent, Color.White, timer.Progress);
			}
			else if (fadeState == FadeStates.FADE_OUT)
			{
				logoSprite.Color = Color.Lerp(Color.White, Color.Transparent, timer.Progress);
			}
		}

		public void Draw(SpriteBatch sb)
		{
			logoSprite.Draw(sb);
		}
	}
}
