using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ArkHive
{
    public abstract class ArkHiveBaseCharacterCardController : HeroCharacterCardController
    {
        public ArkHiveBaseCharacterCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            
        }

        protected int SpareNanobots
        {
            get
            {
                return FindCardsWhere(c => c.IsInPlayAndHasGameText && c.Identifier == "SpareNanobot").Count();
            }
        }




    }
}
