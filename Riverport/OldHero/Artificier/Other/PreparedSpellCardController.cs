using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Artificier
{
    public class PreparedSpellCardController : ArtificierBaseCardController
    {
        public PreparedSpellCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            TrackMana();
        }
        public override IEnumerator Play()
        {
            var search = SearchForCards(DecisionMaker, true, false, 1, 1, new LinqCardCriteria(c => c.DoKeywordsContain("spell")), false, true, false, shuffleAfterwards: true);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(search); } else { this.GameController.ExhaustCoroutine(search); }

            if (ManaPool.CurrentValue >= 2)
            {
                List<YesNoCardDecision> decision = new List<YesNoCardDecision>();
                var decide = this.GameController.MakeYesNoCardDecision(DecisionMaker, SelectionType.RemoveTokens, Card, null, decision, null, GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(decide); } else { this.GameController.ExhaustCoroutine(decide); }
                if (DidPlayerAnswerYes(decision))
                {
                    var pay = this.GameController.RemoveTokensFromPool(ManaPool, 2, cardSource: GetCardSource());
                    if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(pay); } else { this.GameController.ExhaustCoroutine(pay); }
                    var play = SelectAndPlayCardFromHand(DecisionMaker, false, null, new LinqCardCriteria(c => c.DoKeywordsContain("spell")), false, true);
                    if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(play); } else { this.GameController.ExhaustCoroutine(play); }
                }
            }
        }
    }
}
