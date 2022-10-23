using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Fenrir
{
    public class FenrirControlAndRageCardController : BaseFenrirCardController
    {
        public FenrirControlAndRageCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController, true)
        {
            SpecialStringMaker.ShowNumberOfCardsUnderCard(card);
        }

        public override void AddTriggers()
        {
            /**
             * At the end of your turn, if this card has 7 or more cards under it, destroy it and Fenrir deals 1 target X Melee damage, where X is the number of cards under this one
             * Human: At the start of your turn, you may destroy a card under this one and to draw a card.
             * Wolf: When you discard a card, you may then put it under this card
             * 
             */
            AddEndOfTurnTrigger(tt => tt == TurnTaker && Card.UnderLocation.NumberOfCards > 7, Frenzy, TriggerType.DealDamage);
            AddStartOfTurnTrigger(tt => IsHuman && tt == TurnTaker, Control, TriggerType.DrawCard);
            AddTrigger<DiscardCardAction>(dca => IsWolf && dca.CardToDiscard.Owner == TurnTaker, Rage, TriggerType.MoveCard, TriggerTiming.After);
        }

        private IEnumerator Control(PhaseChangeAction arg)
        {
            // ... destroy a card under this one and draw a card.
            if (!Card.UnderLocation.HasCards) yield break;
            SelectCardDecision scd = new SelectCardDecision(GameController, HeroTurnTakerController, SelectionType.DestroyCard, Card.UnderLocation.Cards, true, cardSource: GetCardSource());
            var e = this.GameController.SelectCardAndDoAction(scd, sel => DrawCard());
            if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
            else this.GameController.ExhaustCoroutine(e);
        }

        private IEnumerator Rage(DiscardCardAction arg)
        {
            // ... you may put the discarded card under this one
            List<YesNoCardDecision> answer = new List<YesNoCardDecision>();
            var e = this.GameController.MakeYesNoCardDecision(HeroTurnTakerController, SelectionType.MoveCardToUnderCard, arg.CardToDiscard, storedResults: answer, cardSource: GetCardSource());
            if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
            else this.GameController.ExhaustCoroutine(e);
            if(DidPlayerAnswerYes(answer))
            {
                e = this.GameController.MoveCard(TurnTakerController, arg.CardToDiscard, Card.UnderLocation, false, false, false, cardSource: GetCardSource());
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
            }
        }

        private IEnumerator Frenzy(PhaseChangeAction arg)
        {
            //... this card has 7 or more cards under it
            /*
             * Destroy this card
             * Deal 1 target X Melee damage where X is the number of cards under this one.
             */
            int x = Card.UnderLocation.NumberOfCards;
            IEnumerator e = this.GameController.DestroyCard(HeroTurnTakerController, Card, cardSource: GetCardSource());
            if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
            else this.GameController.ExhaustCoroutine(e);

            e = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, Fenrir, x, DamageType.Melee, 1, false, 1, cardSource: GetCardSource());
            if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
            else this.GameController.ExhaustCoroutine(e);


        }
    }
}
