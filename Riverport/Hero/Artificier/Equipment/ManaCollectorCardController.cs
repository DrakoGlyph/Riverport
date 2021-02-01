using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Artificier
{
    public class ManaCollectorCardController : ArtificierBaseCardController
    {
        public ManaCollectorCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddStartOfTurnTrigger(tt => tt == TurnTaker, pca => this.GameController.AddTokensToPool(ManaPool, 1, GetCardSource()), TriggerType.AddTokensToPool);
        }
    }
}
