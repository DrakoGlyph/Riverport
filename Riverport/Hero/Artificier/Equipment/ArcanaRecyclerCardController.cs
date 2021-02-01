using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Artificier
{
    public class ArcanaRecyclerCardController : ArtificierBaseCardController
    {
        public ArcanaRecyclerCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddEndOfTurnTrigger(tt => tt == TurnTaker, Recycle, TriggerType.MoveCard);
        }

        private IEnumerator Recycle(PhaseChangeAction arg)
        {
            var move = this.GameController.SelectCardFromLocationAndMoveIt(DecisionMaker, TurnTaker.Trash, new LinqCardCriteria(c => c.DoKeywordsContain("spell")), new List<MoveCardDestination>() { new MoveCardDestination(HeroTurnTaker.Hand) }, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(move); } else { this.GameController.ExhaustCoroutine(move); }
            if (ManaPool.CurrentValue >= 2)
            {
                List<YesNoCardDecision> decision = new List<YesNoCardDecision>();
                var decide = this.GameController.MakeYesNoCardDecision(DecisionMaker, SelectionType.RemoveTokens, Card, null, decision, null, GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(decide); } else { this.GameController.ExhaustCoroutine(decide); }
                if(DidPlayerAnswerYes(decision))
                {
                    var pay = this.GameController.RemoveTokensFromPool(ManaPool, 2, cardSource: GetCardSource());
                    if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(pay); } else { this.GameController.ExhaustCoroutine(pay); }
                    var play = SelectAndPlayCardFromHand(DecisionMaker, true, null, new LinqCardCriteria(c => c.DoKeywordsContain("spell")));
                    if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(play); } else { this.GameController.ExhaustCoroutine(play); }
                }
            }
        }
    }
}
