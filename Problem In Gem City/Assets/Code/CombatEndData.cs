using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AssemblyCSharp
{
    public class CombatEndData
    {
        public GameConstants.CombatEndType combatEndType;
        public Scene ReturnScene;
        public List<InventoryItemScript> FoundItems;
        public int XP;
        public string EndMessage;

        public CombatEndData()
        {
            
        }

        public CombatEndData(GameConstants.CombatEndType c, Scene s,List<InventoryItemScript> items, int xp, string endMessage)
        {
            this.combatEndType = c;
            this.ReturnScene = s;
            this.FoundItems = items;
            this.XP = xp;
            this.EndMessage = endMessage;
        }
    }
}

