using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.ArkHive
{
    public class RedundantArrayCardController : ArkHiveBaseCardController
    {
        public RedundantArrayCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddTrigger<DestroyCardAction>((DestroyCardAction dca) => dca.CardToDestroy.Card.DoKeywordsContain("plan"), dca => DrawCard(), TriggerType.DrawCard, TriggerTiming.After);
        }

    }
}
