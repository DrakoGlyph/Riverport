using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public abstract class MaterialCardController : CardController
    {
        protected MaterialCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowSpecialString(Inventory);
        }

        private string Inventory()
        {
            string rtn = "Suits in play:\n";
            foreach(HeroTurnTaker character in GameController.AllHeroes)
            {
                Card c = FindSuit(character);
                if (c == null) rtn += character.ShortName + " does not have a suit equipepd.";
                else rtn += character.ShortName + " has " + c.Title + " equipped.";
                rtn += "{BR}";
            }
            return rtn;
        }

        private Card FindSuit(HeroTurnTaker character)
        {
            foreach(Card c in character.CharacterCard.NextToLocation.Cards)
            {
                if (c.DoKeywordsContain("suit")) return c;
            }
            return null;
        }

        public override IEnumerator DeterminePlayLocation(List<MoveCardDestination> storedResults, bool isPutIntoPlay, List<IDecision> decisionSources, Location overridePlayArea = null, LinqTurnTakerCriteria additionalTurnTakerCriteria = null)
        {
            var equip = SelectCardThisCardWillMoveNextTo(new LinqCardCriteria(DetermineEligibility, "Suit", false, false, "Suit"), storedResults, isPutIntoPlay, decisionSources);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(equip); } else { this.GameController.ExhaustCoroutine(equip); }
        }

        protected bool DetermineEligibility(Card c)
        {
            return c.IsInPlayAndNotUnderCard && c.DoKeywordsContain("suit");
        }

        public override void AddTriggers()
        {
            AddIfTheTargetThatThisCardIsNextToLeavesPlayDestroyThisCardTrigger();
        }

        public Card Suit
        {
            get
            {
                return GetCardThisCardIsNextTo();
            }
        }

        public Card Equipped {
            get
            {
                SuitCardController scc = FindCardController(Suit) as SuitCardController;
                return scc.Equipped;
            }
        }

        public TurnTaker EquippedTurnTaker
        {
            get
            {
                return FindCardController(Equipped).TurnTaker;
            }
        }

        public override IEnumerator Play()
        {
            // Move all but 1 Materials next to this Suit to Weaver's Hand
            if (Suit.NextToLocation.NumberOfCards > 1)
            {
                int mats = 0;
                foreach (Card c in Suit.NextToLocation.Cards)
                {
                    if (c.DoKeywordsContain("material")) mats++;
                }
                if(mats > 1)
                {
                    var rtn = this.GameController.SelectCardsFromLocationAndMoveThem(HeroTurnTakerController, Suit.NextToLocation, mats - 1, mats - 1, new LinqCardCriteria(c => c.DoKeywordsContain("material")), new List<MoveCardDestination>() { new MoveCardDestination(HeroTurnTaker.Hand) }, selectionType: SelectionType.MoveCardToHand, cardSource: GetCardSource());
                    if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(rtn); } else { this.GameController.ExhaustCoroutine(rtn); }
                }
            }
        }
    }
}
