using System;

namespace AssemblyCSharp
{
    public static class GameConstants
    {
        /*---------- Game State Management -------*/
        [Serializable]
        public enum GameState { Overworld, Dialogue, Combat, Cutscene };

        /*---------- Movement variables ----------*/
        public enum AnimDir { Up, Right, Left, Down };

        /*---------- Dialogue variables ----------*/
        [Serializable]
        public enum InteractionType { Normal, Scared };
        [Serializable]
        public enum DialogueTypes { Generic, Question, Item, QuestionItem };

        /*---------- Scene Management ----------*/
        [Serializable]
        public enum SceneIndices {GameScene, CombatScene, CutScene}; 

        /*---------- Combat Management ----------*/
        [Serializable]
        public enum TurnOrderType { IndividualSpeed, FastestTeammate };
        [Serializable]
        public enum ActionOptionIndices { Attack, Ability, Item, Flee, Default };
        [Serializable]
        public enum AbilityType { BasicAttack, Ability };
        [Serializable]
        public enum DamageType { AbilityDamage, PhysDamage };
        [Serializable]
        public enum TargetType { Self, Ally, Party, SingleEnemy, EnemyParty };
        [Serializable]
        public enum StatusType { None, Downed, Sleep, Poison };
        [Serializable]
        public enum RangeType { Melee, Ranged };

        //NPC Enemy Behavior types for action selection in combat
        [Serializable]
        public enum EnemyCombatBehaviorType{ Standard, Offensive, Defensive, Healer };

        //NPC Enemy Focus types for targeting in combat
        [Serializable]
        public enum EnemyFocusType{ Random, LowHealth, Weakened, PriorityTarget, Collaborate, HighestSplash };

        /// <summary>
        /// Combat end type. In the case of player vs. ai enemies, Party 1 is the player, Party 2 is the computer.
        /// </summary>
        [Serializable]
        public enum CombatEndType { Party1Win, Party2Win };


        public const int MAX_PARTY_SIZE = 3;

        /// <summary>
        /// The currently open combat menu page, e.g. the level to which the menu has been drilled down into
        /// </summary>
        [Serializable]
        public enum CombatMenuPage{ Unopened, Action, Attack, Ability, Item, Flee, AnimationAction, EndScreen };

        /*---------- Positioning Variables ----------*/

        //Party positions
        public static float CombatPartyStartPosX = -1f;
        public static float CombatPartyStartPosY =  2f;

        //Enemy positions
        public static float EnemyStartPosX = 1.55f;
        public static float EnemyStartPosY = 2f;

    }
}

