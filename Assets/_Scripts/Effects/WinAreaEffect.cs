using System;
using Managers;
using PlayerLogic;
using UnityEngine;

namespace Game.Floor
{
    public class WinAreaEffect : MonoBehaviour, IPlayerEffect 
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                ApplyEffect(player);
            }
        }

        public void ApplyEffect(Player player)
        {
            player.Won();
            GameManager.Instance.Win();
        }
        
        
    }
}