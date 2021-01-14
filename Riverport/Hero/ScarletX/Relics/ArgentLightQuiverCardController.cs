using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ScarletX
{
    public class ArgentLightQuiverCardController : CardController
    {
        public ArgentLightQuiverCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddStartOfTurnTrigger(tt => tt == TurnTaker, StockArrow, TriggerType.MoveCard);
        }

        private IEnumerator StockArrow(PhaseChangeAction arg)
        {
            var stock = this.GameController.SelectCardFromLocationAndMoveIt(DecisionMaker, TurnTaker.Trash, new LinqCardCriteria(c => c.DoKeywordsContain("arrow")), new List<MoveCardDestination>() { new MoveCardDestination(TurnTaker.PlayArea) }, true, false, false, true, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(stock); } else { this.GameController.ExhaustCoroutine(stock); }
        }

    }
}
