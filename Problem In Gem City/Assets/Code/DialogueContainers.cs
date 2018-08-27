using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
    [Serializable]
    public class DialogueContainers
    {
        
        public List<DialogueChain> dialogueChains;
        public GameConstants.InteractionType InteractionType;

        public DialogueContainers()
        {
        }
    }
}

