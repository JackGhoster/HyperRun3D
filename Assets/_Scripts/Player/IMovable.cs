using Managers;
using UnityEngine;

namespace PlayerLogic
{
    public interface IMovable
    {
        CharacterController Controller { get; }
        float Speed { get; }
        int MaxJumpCount { get; }
        float JumpHeight { get; }
        int CurrentJumpCount { get; }
        float GravityForce { get; }

        Vector3 Velocity { get; }
        
        void Move();
    }
}