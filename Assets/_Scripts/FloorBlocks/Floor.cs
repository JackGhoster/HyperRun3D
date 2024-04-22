using System;
using System.Runtime.CompilerServices;
using Managers;
using TriInspector;
using UnityEngine;

namespace Game.Floor
{
    public class Floor : MonoBehaviour
    {
        [field: SerializeField] public FloorType FloorType { get; private set; } = FloorType.None;
        public bool IsTurn => FloorType == FloorType.Left || FloorType == FloorType.Right;
        [field: SerializeField, ShowIf(nameof(IsTurn))] public Transform EndPoint { get; private set; }
        
        public bool FailedToPass { get; set; }
        

        public uint FloorIndex { get; private set; } = 0;
        public void SetIndex(uint newIndex) => FloorIndex = newIndex;

    }
}