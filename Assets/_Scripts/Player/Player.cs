using System;
using System.Collections;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace PlayerLogic
{
    public class Player : MonoBehaviour, IWithStats, IKillable
    {
        [field: SerializeField] public Movement PlayerMovement {get; private set; }
        [field: SerializeField] public Animator PlayerAnimator {get; private set; }
        private static readonly int JumpAnim = Animator.StringToHash("Jump");
        private static readonly int TookDamageAnim = Animator.StringToHash("TookDamage");
        private static readonly int DiedAnim = Animator.StringToHash("Died");
        private static readonly int RevivedAnim = Animator.StringToHash("Revived");
        private static readonly int WonAnim = Animator.StringToHash("Won");

        public Stats Stats { get; set; } = new Stats();

        public bool Initialized { get; private set; } = false;
        public bool IsDead { get; set; } = false;
        public bool IsDamageable { get; private set; } = true;

        private float _defaultHealth = 0;
        
        
        public void Initialize()
        {
            Initialized = true;
            PlayerMovement.Jumped += OnJumped;
            _defaultHealth = Stats.health;
        }
        

        private void OnJumped()
        {
            PlayerAnimator.SetTrigger(JumpAnim);
            PlayerAnimator.SetTrigger(JumpAnim);
        }

        private void Update()
        {
            if(Initialized) PlayerMovement.Move();
        }


        public void ApplyDamage(float damage = 1, float time = 1, float speedMod = 1)
        {
            if(!IsDamageable) return;
            Stats -= new PlayerStatsBuilder().WithHealth(damage).Build();
            PlayerAnimator.SetTrigger(TookDamageAnim);
            if(CheckIfDead()) return;
            StartCoroutine(TemporarySpeedChange(speedMod: speedMod, time: time));
        }

        public void Won()
        {
            PlayerMovement.StopMovement();
            PlayerAnimator.SetTrigger(WonAnim);
        }
        
         public bool CheckIfDead()
        {
            if(IsDead) return true;
            
            if (Stats.health <= 0)
            {
                Die();
            }

            return IsDead;
        }
  
        public void Die()
        {
            IsDead = true;
            Stats.health = 0;
            PlayerMovement.StopMovement();
            GameManager.Instance.PlayerManager.InvokePlayerDied();
            PlayerAnimator.SetTrigger(DiedAnim);
        }


        public void Revive()
        {
            Stats.health = _defaultHealth;
            IsDead = false;
            PlayerAnimator.SetTrigger(RevivedAnim);
            var nextFloor = GameManager.Instance.FloorManager.GetNextFloor();
            this.gameObject.transform.DOMove(nextFloor.transform.position + Vector3.up, 0.1f);
            PlayerMovement.StartMovement();
            StartCoroutine(TemporarySpeedChange(0.5f, 3f));
        }
        
       public IEnumerator TemporarySpeedChange(float speedMod, float time)
        {
            var timer = 0f;
            
            Stats.speed *= speedMod;
            
            while (timer < time)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            Stats.speed /= speedMod;
        }

        public IEnumerator TemporaryInvincibility(float time)
        {
            IsDamageable = false;
            yield return new WaitForSeconds(time);
            IsDamageable = true;
        }
    }
}