namespace PlayerLogic
{
    public interface IKillable
    {
        bool IsDead { get; set; }
        void Die();
    }
}