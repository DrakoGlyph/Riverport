using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class QuickPatchworkCardController : CardController
    {
        public QuickPatchworkCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddStartOfTurnTrigger(tt => tt == TurnTaker, pca => SelectAndPlayCardFromHand(DecisionMaker, cardCriteria: new LinqCardCriteria(c => c.DoKeywordsContain("patch"))), TriggerType.PlayCard);
        }

        public override IEnumerator UsePower(int index = 0)
        {
            var sac = DrawCard();
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(sac); } else { this.GameController.ExhaustCoroutine(sac); }

            var play = this.GameController.SelectCardFromLocationAndMoveIt(DecisionMaker, TurnTaker.Trash, new LinqCardCriteria(c => c.DoKeywordsContain("patch")), new List<MoveCardDestination> { new MoveCardDestination(TurnTaker.PlayArea) }, false, true, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(play); } else { this.GameController.ExhaustCoroutine(play); }
        }

    }
}
