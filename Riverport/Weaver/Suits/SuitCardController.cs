using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public abstract class SuitCardController : CardController
    {
        public Card Equipped
        {
            get
            {
                return GetCardThisCardIsNextTo();
            }
        }

        protected SuitCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowListOfCards(new LinqCardCriteria(c => DetermineEligibility(c), "Heroes without suits", false, false, "Hero without a suit", "Heroes without suits"));
        }

        public override void AddTriggers()
        {
            //If Equipped hero is dealt 5 or more damage in 1 blow, destroy this card
            AddTrigger<DealDamageAction>((DealDamageAction dda) => dda.Amount >= 5 && dda.Target == Equipped, DestroyThisCardResponse, TriggerType.DestroySelf, TriggerTiming.After);
        }

        protected bool DetermineEligibility(Card c)
        {
            //Suits can only be played next to Hero Character Cards
            //If there are no cards next to this, it can have a suit
            return c.IsHeroCharacterCard && !c.IsIncapacitated;
        }

        public override IEnumerator DeterminePlayLocation(List<MoveCardDestination> storedResults, bool isPutIntoPlay, List<IDecision> decisionSources, Location overridePlayArea = null, LinqTurnTakerCriteria additionalTurnTakerCriteria = null)
        {
            var equip = SelectCardThisCardWillMoveNextTo(new LinqCardCriteria(DetermineEligibility), storedResults, isPutIntoPlay, decisionSources);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(equip); } else { this.GameController.ExhaustCoroutine(equip); }
        }

        public override IEnumerator Play()
        {
            // Move all but 1 Materials next to this Suit to Weaver's Hand

            int suits = 0;
            foreach (Card c in Equipped.NextToLocation.Cards)
            {
                if (c.DoKeywordsContain("suit")) suits++;
            }
            if (suits > 1)
            {
                var rtn = this.GameController.SelectCardsFromLocationAndMoveThem(HeroTurnTakerController, Equipped.NextToLocation, suits - 1, suits - 1, new LinqCardCriteria(c => c.DoKeywordsContain("material")), new List<MoveCardDestination>() { new MoveCardDestination(HeroTurnTaker.Hand) }, selectionType: SelectionType.MoveCardToHand, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(rtn); } else { this.GameController.ExhaustCoroutine(rtn); }
            }
        }
    }
}
