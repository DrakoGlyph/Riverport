using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Collections;

namespace Riverport.ScarletX
{
    public abstract class ArrowBaseCardController : CardController
    {
        protected ArrowBaseCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        protected TriggerType[] Type
        {
            get;
            set;
        }
        
        protected abstract IEnumerator FireArrow(DestroyCardAction dca);

        public override void AddTriggers()
        {
            AddWhenDestroyedTrigger(FireArrow, Type, dca => dca.ResponsibleCard == CharacterCard, TriggerPriority.High);
        }
    }
}
