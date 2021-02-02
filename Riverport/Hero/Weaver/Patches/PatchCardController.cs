using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public abstract class PatchCardController : CardController
    {
        protected PatchCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        protected virtual TriggerType TriggerType { get; }
        protected virtual StatusEffect StatusEffect { get; }

        protected virtual IEnumerator Empower(GameAction ga = null)
        {
            List<YesNoCardDecision> decisions = new List<YesNoCardDecision>();
            var decide = this.GameController.MakeYesNoCardDecision(FindHeroTurnTakerController(EquippedTurnTaker as HeroTurnTaker), SelectionType.DestroySelf, Card, storedResults: decisions, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(decide); } else { this.GameController.ExhaustCoroutine(decide); }
            if (DidPlayerAnswerYes(decisions))
            {
                var status = AddStatusEffect(StatusEffect);
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(status); } else { this.GameController.ExhaustCoroutine(status); }
                var destroy = this.GameController.DestroyCard(DecisionMaker, Card, cardSource: GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
            }
        }

        public override IEnumerator DeterminePlayLocation(List<MoveCardDestination> storedResults, bool isPutIntoPlay, List<IDecision> decisionSources, Location overridePlayArea = null, LinqTurnTakerCriteria additionalTurnTakerCriteria = null)
        {
            var equip = SelectCardThisCardWillMoveNextTo(new LinqCardCriteria(c=>c.IsHeroCharacterCard), storedResults, isPutIntoPlay, decisionSources);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(equip); } else { this.GameController.ExhaustCoroutine(equip); }
        }


        public override void AddTriggers()
        {
            AddIfTheTargetThatThisCardIsNextToLeavesPlayDestroyThisCardTrigger();

            AddStartOfTurnTrigger(tt => tt == EquippedTurnTaker, Empower, TriggerType);
        }


        public Card Equipped {
            get
            {
                return GetCardThisCardIsNextTo();
            }
        }

        public TurnTaker EquippedTurnTaker
        {
            get
            {
                return FindCardController(Equipped).TurnTaker;
            }
        }

    }
}
