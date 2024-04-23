using UnityEngine;

namespace Project._Scripts.Runtime.ScriptableObjects
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "UnitData")]
    public class UnitData : ScriptableObject
    {
        public int Health;
        public int Damage;
        public int TargetDetectRange;
        public float DamageRadius;
        public float AttackCooldown;
        public LayerMask TargetLayers;
        public float ForceStrength = 7.5f;
    }
}