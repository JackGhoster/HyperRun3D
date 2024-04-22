using System;
using Managers;
using UnityEngine;

namespace PlayerLogic
{
    [RequireComponent(typeof(CharacterController))]
    public class Movement : MonoBehaviour, IMovable
    {
        [field: SerializeField] private CharacterController _controller;
        public IWithStats StatHolder { get; private set; }
        
        public CharacterController Controller => _controller;
        public float Speed => StatHolder.Stats.speed;
        public int MaxJumpCount => StatHolder.Stats.maxJumpCount;
        public float JumpHeight => StatHolder.Stats.jumpHeight;
        private int _currentJumpCount = 2;
        public int CurrentJumpCount => _currentJumpCount;
        public float GravityForce => StatHolder.Stats.gravityForce;

        private Vector3 _velocity = Vector3.zero;
        public Vector3 Velocity => _velocity;

        [field: SerializeField] public bool IsMoving { get; private set; } = false; 
        public event Action StartedMovement;
        public event Action StoppedMovement;
        public event Action Jumped;

        private void Awake()
        {
            StatHolder ??= GetComponent<IWithStats>();
        }

        public void StartMovement()
        {
            IsMoving = true;
            StartedMovement?.Invoke();
        }

        public void StopMovement()
        {
            IsMoving = false;
            StoppedMovement?.Invoke();
        }
        
        public void Move()
        {
            bool grounded = Controller.isGrounded;
            
            if (!IsMoving)
            {
                if (grounded && Velocity.y < 0)
                {
                    _velocity.y = 0;
                }

                _velocity.y += GravityForce * Time.deltaTime;
                Controller.Move(Velocity * Time.deltaTime);
                return;
            }

            TryResetJumps();
            
            if (grounded && Velocity.y < 0)
            {
                _velocity.y = 0;
            }

            var direction = gameObject.transform.forward;
            Controller.Move(direction * (Speed * Time.deltaTime));

            if (direction != Vector3.zero)
            {
                gameObject.transform.forward = direction;
            }

            var inputCondition = Input.GetMouseButtonDown(0) ||
                                 Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began; 
            
            if (inputCondition && CurrentJumpCount > 0)
            {
                _velocity.y += Mathf.Sqrt(JumpHeight * -3.0f * GravityForce);
                HandleJump();
            }

            
            _velocity.y += GravityForce * Time.deltaTime;
            Controller.Move(Velocity * Time.deltaTime);
        }


        public void HandleJump()
        {
            _currentJumpCount--;
            Jumped?.Invoke();
        }
        
        public void TryResetJumps()
        {
            if (Controller.isGrounded) _currentJumpCount = MaxJumpCount;
        }


        private void OnValidate()
        {
            _controller ??= GetComponent<CharacterController>();
            StatHolder ??= GetComponent<IWithStats>();
        }
                       
    }
}