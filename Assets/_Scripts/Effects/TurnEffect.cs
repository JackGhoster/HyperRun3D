using System;
using UnityEngine;

namespace Game.Effects
{
    [RequireComponent(typeof(Collider))]
    public class TurnEffect : MonoBehaviour, IPlayerEffect
    {
        [field: SerializeField] public TurnType TurnType { get; private set; } 
    
        public void ApplyEffect()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            ApplyEffect();
        }
    }
}

public enum TurnType
{
    Left,
    Right
}
