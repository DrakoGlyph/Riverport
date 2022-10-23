using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Fenrir
{
    public class FenrirTheOtherHandCardController : BaseFenrirCardController
    {
        public FenrirTheOtherHandCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController, true)
        {
            AddThisCardControllerToList(CardControllerListType.IncreasePhaseActionCount);
            
        }

        public override void AddTriggers()
        {
            /*
             * Human: You may play an additional card during your play phase
             * Wolf: When Fenrir deals Melee damage, he may discard a card to deal that same damage to a second target.
             */
            
            AddAdditionalPhaseActionTrigger(tt => IsHuman && tt == TurnTaker, Phase.PlayCard, 1);
            AddTrigger<DealDamageAction>(dda => IsWolf && dda.DamageSource.IsSameCard(CharacterCard), WolfOtherHand, TriggerType.DealDamage, TriggerTiming.After, isActionOptional: true);
        }

        public override bool AskIfIncreasingCurrentPhaseActionCount()
        {
            return IsHuman && Game.ActiveTurnPhase.Phase == Phase.PlayCard && Game.ActiveTurnTaker == TurnTaker;
        }

        private IEnumerator WolfOtherHand(DealDamageAction arg)
        {
            // Discard a card to deal that same damage to a second target

            List<DiscardCardAction> discards = new List<DiscardCardAction>();
            var e = SelectAndDiscardCards(HeroTurnTakerController, 1, true, null, discards);
            if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
            else this.GameController.ExhaustCoroutine(e);
            if (DidDiscardCards(discards))
            {
                e = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, Fenrir, arg.Amount, DamageType.Melee, 1, false, 1, additionalCriteria: c => c.IsTarget && c != arg.Target, cardSource: GetCardSource());
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
            }
        }
         
    }
}
