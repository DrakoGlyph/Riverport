using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class ResourceReclamationCardController : CardController
    {
        public ResourceReclamationCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddTrigger<DestroyCardAction>((DestroyCardAction dca) => dca.CardToDestroy.Card.Owner == TurnTaker && dca.CardToDestroy.Card.DoKeywordsContain("patch"), dca => DrawCard(), TriggerType.DrawCard, TriggerTiming.After);
        }

    }
}
