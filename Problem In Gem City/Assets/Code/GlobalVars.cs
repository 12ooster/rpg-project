using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
    public static class GlobalVars 
    {
        public enum DamageType {AbilityDamage,PhysDamage};

        public enum TargetType {Self, Ally, Party, SingleEnemy, EnemyParty};

        public const int MAX_PARTY_SIZE = 3;
    }
}
