using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
    [System.Serializable]
    public class DialogueChains
    {
        public GameConstants.DialogueTypes Type = GameConstants.DialogueTypes.Generic;
        public List<Dialogue> charDialogue = new List<Dialogue>();

        public DialogueChains()
        {
            
        }
    }
}

