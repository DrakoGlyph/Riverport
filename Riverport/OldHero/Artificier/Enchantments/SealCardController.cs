using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Artificier
{
    public class SealCardController : EnchantmentBaseCardController
    {
        public SealCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        protected override TriggerType TriggerType
        {
            get { return TriggerType.DestroyCard; }
        }

        protected override Card Enchanted
        {
            get { return Card.UnderLocation.TopCard; }
        }

        protected override bool ShouldDestroySelfWhenEnchantedTargetLeaves
        {
            get { return false; }
        }

        protected override bool DetermineEligible(Card c)
        {
            return false;
        }

        

        public override IEnumerator DeterminePlayLocation(List<MoveCardDestination> storedResults, bool isPutIntoPlay, List<IDecision> decisionSources, Location overridePlayArea = null, LinqTurnTakerCriteria additionalTurnTakerCriteria = null)
        {
            var place = (this as CardController).DeterminePlayLocation(storedResults, isPutIntoPlay, decisionSources, overridePlayArea, additionalTurnTakerCriteria);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(place); } else { this.GameController.ExhaustCoroutine(place); }
        }

        public override IEnumerator Play()
        {
            List<SelectCardDecision> storedResults = new List<SelectCardDecision>();
            var select = this.GameController.SelectCardAndStoreResults(DecisionMaker, SelectionType.MoveCardToUnderCard, new LinqCardCriteria(c => c.IsInPlayAndHasGameText && (c.DoKeywordsContain("relic") || c.DoKeywordsContain("ongoing")) && !this.GameController.IsCardIndestructible(c)), storedResults, false);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(select); } else { this.GameController.ExhaustCoroutine(select); }
            if(DidSelectCard(storedResults))
            {
                Card toMove = GetSelectedCard(storedResults);
                var move = this.GameController.MoveCard(TurnTakerController, toMove, Card.UnderLocation, cardSource: GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(move); } else { this.GameController.ExhaustCoroutine(move); }
            }
        }

        protected override IEnumerator WhenDestroyed(DestroyCardAction dca)
        {
            IEnumerator remove = null;
            if(ManaPool.CurrentValue >= 2)
            {
                List<YesNoCardDecision> decision = new List<YesNoCardDecision>();
                var decide = this.GameController.MakeYesNoCardDecision(DecisionMaker, SelectionType.DestroyCard, Enchanted, null, decision, null, GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(decide); } else { this.GameController.ExhaustCoroutine(decide); }
                if(DidPlayerAnswerYes(decision))
                {
                    var pay = this.GameController.RemoveTokensFromPool(ManaPool, 2, cardSource: GetCardSource());
                    if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(pay); } else { this.GameController.ExhaustCoroutine(pay); }
                    remove = this.GameController.DestroyCard(DecisionMaker, Enchanted, cardSource: GetCardSource());
                }
            }
            if(remove == null)
            {
                remove = this.GameController.PlayCard(TurnTakerController, Enchanted, true, cardSource: GetCardSource());
            }
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(remove); } else { this.GameController.ExhaustCoroutine(remove); }

        }
    }
}
