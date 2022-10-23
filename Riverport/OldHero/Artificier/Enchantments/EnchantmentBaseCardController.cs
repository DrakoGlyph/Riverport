using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Artificier
{
    public abstract class EnchantmentBaseCardController : ArtificierBaseCardController
    {
        protected EnchantmentBaseCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            TrackMana();
        }

        protected virtual TriggerType TriggerType
        {
            get;
        }

        protected virtual Card Enchanted
        {
            get { return GetCardThisCardIsNextTo(); }
        }

        protected virtual bool ShouldDestroySelfWhenEnchantedTargetLeaves
        {
            get { return true; }
        }

        public override void AddTriggers()
        {
            AddEndOfTurnTrigger(tt => tt == TurnTaker, Upkeep, TriggerType.ModifyTokens);
            AddWhenDestroyedTrigger(WhenDestroyed, TriggerType);
            if(ShouldDestroySelfWhenEnchantedTargetLeaves) AddIfTheTargetThatThisCardIsNextToLeavesPlayDestroyThisCardTrigger();
        }

        private IEnumerator Upkeep(PhaseChangeAction arg)
        {
            List<YesNoCardDecision> spend = new List<YesNoCardDecision>();
            IEnumerator remove = null;
            if (ManaPool.CurrentValue > 0)
            {
                var decide = this.GameController.MakeYesNoCardDecision(DecisionMaker, SelectionType.RemoveTokens, Card, null, spend, null, GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(decide); } else { this.GameController.ExhaustCoroutine(decide); }
                if (DidPlayerAnswerYes(spend))
                {
                    remove = this.GameController.RemoveTokensFromPool(ManaPool, 1, cardSource: GetCardSource());
                }
            }
            if(remove == null)
            {
                remove = this.GameController.DestroyCard(DecisionMaker, Card, cardSource: GetCardSource());
            }
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(remove); } else { this.GameController.ExhaustCoroutine(remove); }

        }

        public override IEnumerator DeterminePlayLocation(List<MoveCardDestination> storedResults, bool isPutIntoPlay, List<IDecision> decisionSources, Location overridePlayArea = null, LinqTurnTakerCriteria additionalTurnTakerCriteria = null)
        {
            var place = SelectCardThisCardWillMoveNextTo(new LinqCardCriteria(DetermineEligible), storedResults, isPutIntoPlay, decisionSources);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(place); } else { this.GameController.ExhaustCoroutine(place); }
        }

        protected abstract bool DetermineEligible(Card c);
        protected abstract IEnumerator WhenDestroyed(DestroyCardAction dca);
    }
}
