using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ArkHive
{
    public abstract class ArkhivePlanBaseCardController : ArkHiveBaseCardController
    {
        protected ArkhivePlanBaseCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowSpecialString(ListArkhiveAndTheSpares);
            SpecialStringMaker.ShowNumberOfCardsAtLocation(Card.UnderLocation);
        }

        protected IEnumerator EnactTrigger(GameAction arg)
        {
            List<SelectCardDecision> selectCardsDecision = new List<SelectCardDecision>();
            var enact = this.GameController.SelectCardsAndDoAction(HeroTurnTakerController, new LinqCardCriteria(c => c.DoKeywordsContain("plan") && c.IsInPlayAndHasGameText), SelectionType.ActivateAbility, card => FindCardController(card).ActivateAbility("enact"), 1, false, 0, selectCardsDecision, false, () => "Enact a Plan and destroy this card", GetCardSource()); ;
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(enact); } else { this.GameController.ExhaustCoroutine(enact); }
            if (DidSelectCard(selectCardsDecision))
            {
                var destroy = this.GameController.DestroyCard(DecisionMaker, Card, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
            }
        }
    }
}
