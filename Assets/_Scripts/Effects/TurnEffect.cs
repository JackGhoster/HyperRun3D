using System;
using DG.Tweening;
using PlayerLogic;
using UnityEngine;

namespace Game.Effects
{
    [RequireComponent(typeof(Collider))]
    public class TurnEffect : MonoBehaviour, IPlayerEffect
    {
        [field: SerializeField] public TurnType TurnType { get; private set; } 
    
        public void ApplyEffect(Player player)
        {
            player.PlayerMovement.StopMovement();
            var rotation = player.gameObject.transform.rotation;
            switch (TurnType)
            {
                case TurnType.Left:
                    // player.gameObject.transform.rotation = Quaternion.Euler(rotation.eulerAngles + new Vector3(0, -90, 0));
                    player.gameObject.transform.DORotate(
                        rotation.eulerAngles + new Vector3(0, -90, 0), 0.5f).OnComplete(() =>
                    {
                        player.PlayerMovement.StartMovement();
                    });
                    break;
                case TurnType.Right:
                    player.gameObject.transform.DORotate(
                        rotation.eulerAngles + new Vector3(0, 90, 0), 0.5f).OnComplete(() =>
                    {
                        player.PlayerMovement.StartMovement();
                    });
                    // player.gameObject.transform.rotation =  Quaternion.Euler(rotation.eulerAngles + new Vector3(0, 90, 0));
                    break;
            }
            
            // player.PlayerMovement.StartMovement();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                ApplyEffect(player);
            }
        }
    }
}

public enum TurnType
{
    Left,
    Right
}
