using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Shared;

namespace Platformer.Entities.Particles
{
    class Ember : Particle
    {
        private const int MIN_LIFETIME = 5000;
        private const int MAX_LIFETIME = 15000;

        private const float MIN_ANGULAR_VELOCITY = -0.75f;
        private const float MAX_ANGULAR_VELOCITY = 0.75f;
        private const float MIN_DRIFT_CHANGE = 0.1f;
        private const float MAX_DRIFT_CHANGE = 1;
        private const float MAX_DRIFT_SPEED = 5;

        public Ember(Vector2 position, Vector2 velocity) :
            base(ParticleTypes.EMBER, position, velocity, MIN_ANGULAR_VELOCITY, MAX_ANGULAR_VELOCITY, MIN_LIFETIME,
            MAX_LIFETIME)
        {
        }

        public override void Update(float dt)
        {
            Vector2 velocity = Velocity;
            velocity.X += Functions.GetRandomValue(MIN_DRIFT_CHANGE, MAX_DRIFT_CHANGE);
            velocity.X = MathHelper.Clamp(velocity.X, -MAX_DRIFT_SPEED, MAX_DRIFT_SPEED);
            Velocity = velocity;

            base.Update(dt);
        }
    }
}
