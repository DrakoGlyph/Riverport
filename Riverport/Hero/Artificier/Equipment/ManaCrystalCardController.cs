using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Artificier
{
    public class ManaCrystalCardController : ArtificierBaseCardController
    {
        public ManaCrystalCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddWhenDestroyedTrigger(dca => this.GameController.AddTokensToPool(ManaPool, 3, GetCardSource()), TriggerType.AddTokensToPool);

            AddStartOfTurnTrigger(tt => tt == TurnTaker, DestroyThisCardResponse, TriggerType.DestroySelf);
        }
    }
}
