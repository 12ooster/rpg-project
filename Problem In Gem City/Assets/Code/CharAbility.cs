using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
    [System.Serializable]
    public class CharAbility
    {
     
        public CharAbility(){}

        [SerializeField]
        /// <summary>
        /// The name of the ability.
        /// </summary>
        private string _abilityName;


        /// <summary>
        /// Gets or sets the name of the ability.
        /// </summary>
        /// <value>The name of the ability.</value>
        public string AbilityName
        {
            get
            {
                return this._abilityName;
            }
            set
            {
                this._abilityName = value;
            }
        }

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

        [SerializeField]
        /// <summary>
        /// The type of combat move this is (basic attack, ability, etc.)
        /// </summary>
        private GameConstants.AbilityType _aType;

        public  GameConstants.AbilityType aType
        {
            get
            {
                return this._aType;
            }
            set
            {
                this._aType = value;
            }
        }

        [SerializeField]
        /// <summary>
        /// The type of damage this ability does.
        /// </summary>
        private GameConstants.DamageType _dType;

        public  GameConstants.DamageType DType
        {
            get
            {
                return this._dType;
            }
            set
            {
                this._dType = value;
            }
        }

        [SerializeField]
        /// <summary>
        /// The type of targeting this ability uses
        /// </summary>
        private GameConstants.TargetType _tType;

        public GameConstants.TargetType TType
        {
            get
            {
                return this._tType;
            }
            set
            {
                this._tType = value;
            }
        }

        [SerializeField]
        /// <summary>
        /// The base damage the ability deals before modification by equipment, etc.
        /// </summary>
        private int _baseDamage;

        public int BaseDamage
        {
            get
            {
                return this._baseDamage;
            }
            set
            {
                this._baseDamage = value;
            }
        }

        [SerializeField]
        /// <summary>
        /// The accuracy of the ability : likliness to hit
        /// </summary>
        private float _accuracy;

        public float Accuracy
        {
            get
            {
                return this._accuracy;
            }
            set
            {
                this._accuracy = value;
            }
        }

        [SerializeField]
        /// <summary>
        /// This ability's chance to crit.
        /// </summary>
        private float _crit;

        public float Crit
        {
            get
            {
                return this._crit;
            }
            set
            {
                this._crit = value;
            }
        }
    }

}
