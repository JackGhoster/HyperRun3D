namespace PlayerLogic
{
    public interface IWithStats
    {
        Stats Stats { get; set; }
    }
    
    public class PlayerStatsBuilder
    {
        private Stats _stats = new Stats();

        public PlayerStatsBuilder WithSpeed(float speed)
        {
            _stats.speed = speed;
            return this;
        }
        
        public PlayerStatsBuilder WithHealth(float health)
        {
            _stats.health = health;
            return this;
        }

        public PlayerStatsBuilder WithJumpCount(int jumpCount)
        {
            _stats.maxJumpCount = jumpCount;
            return this;
        }

        public PlayerStatsBuilder WithJumpHeight(float height)
        {
            _stats.jumpHeight = height;
            return this;
        }
        
        public PlayerStatsBuilder WithGravity(float gravity)
        {
            _stats.gravityForce = gravity;
            return this;
        }
        
        public Stats Build()
        {
            return _stats;
        }
    }
    
    public class Stats
    {
        public float health = 0;
        public float speed = 0;
        public int maxJumpCount = 0;
        public float gravityForce = 0;
        public float jumpHeight = 0;

        public static Stats operator +(Stats a, Stats b)
        {
            var result = a;

            result.health += b.health;
            result.speed += b.speed;
            result.maxJumpCount += b.maxJumpCount;
            result.gravityForce += b.gravityForce;
            result.jumpHeight += b.jumpHeight;

            return result;
        }
        
        public static Stats operator -(Stats a, Stats b)
        {
            var result = a;

            result.health -= b.health;
            result.speed -= b.speed;
            result.maxJumpCount -= b.maxJumpCount;
            result.gravityForce -= b.gravityForce;
            result.jumpHeight -= b.jumpHeight;

            return result;
        }
        
        public static Stats operator *(Stats a, float b)
        {
            var result = a;

            result.health *= b;
            result.speed *= b;
            result.maxJumpCount *= (int) b;
            result.gravityForce *= b;
            result.jumpHeight *= b;

            return result;
        }
        
        public static Stats operator /(Stats a, float b)
        {
            var result = a;

            result.health /= b;
            result.speed /= b;
            result.maxJumpCount /= (int) b;
            result.gravityForce /= b;
            result.jumpHeight /= b;

            return result;
        }
    }
}
