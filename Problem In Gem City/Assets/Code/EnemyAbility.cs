using System;
using UnityEngine;

namespace AssemblyCSharp
{
    public class EnemyAbility:CharAbility
    {
        [SerializeField]
        /// <summary>
        /// The type of behavior this move this is (basic attack, ability, etc.)
        /// </summary>
        private GameConstants.EnemyCombatBehaviorType _behaviorType;

        public  GameConstants.EnemyCombatBehaviorType BehaviorType
        {
            get
            {
                return this._behaviorType;
            }
            set
            {
                this._behaviorType = value;
            }
        }
    }
}

