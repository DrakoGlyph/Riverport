using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Artificier
{
    public class ArchiveGauntletsCardController : ArtificierBaseCardController
    {
        public ArchiveGauntletsCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            AddThisCardControllerToList(CardControllerListType.MakesIndestructible);
        }

        public override bool AskIfCardIsIndestructible(Card card)
        {
            return this.Card == card;
        }

        public override void AddTriggers()
        {
            AddStartOfTurnTrigger(tt => tt == TurnTaker, pca => this.GameController.AddTokensToPool(Card.FindTokenPool("ManaPool"), 1, GetCardSource()), TriggerType.AddTokensToPool);
        }

        public override IEnumerator UsePower(int index = 0)
        {
            var gather = this.GameController.AddTokensToPool(Card.FindTokenPool("ManaPool"), 1, GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(gather);} else { this.GameController.ExhaustCoroutine(gather); }
        }
    }
}
