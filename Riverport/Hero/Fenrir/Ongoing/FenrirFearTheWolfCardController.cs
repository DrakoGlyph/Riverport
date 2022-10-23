using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Fenrir
{
    public class FenrirFearTheWolfCardController : BaseFenrirCardController
    {
        public FenrirFearTheWolfCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController, true)
        {
        }

        public override void AddTriggers()
        {
            /*
             * Increase Psychic damage dealt by Fenrir by 1
             * Human: When you draw a card, Fenrir may deal 1 target 1 Psychic damage
             * Wolf: When Fenrir deals Melee damage to a target, he may discard a card to deal that target 2 Psychic damage.
             */
            AddIncreaseDamageTrigger(dda => dda.DamageSource.IsSameCard(CharacterCard) && dda.DamageType == DamageType.Psychic, 1);
            AddTrigger<DrawCardAction>(dca => IsHuman && dca.DrawnCard.Owner == TurnTaker, dca => this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, Fenrir, 1, DamageType.Psychic, 1, true, null, cardSource: GetCardSource()), TriggerType.DealDamage, TriggerTiming.After, isActionOptional: true);
            AddTrigger<DealDamageAction>(dda => IsWolf && dda.DamageSource.IsSameCard(CharacterCard), FearTheWolf, TriggerType.DiscardCard, TriggerTiming.After, isActionOptional: true);
        }

        private IEnumerator FearTheWolf(DealDamageAction dda)
        {
            List<DiscardCardAction> discarded = new List<DiscardCardAction>();
            IEnumerator e = this.GameController.SelectAndDiscardCard(HeroTurnTakerController, true, storedResults: discarded, cardSource: GetCardSource());
            if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
            else this.GameController.ExhaustCoroutine(e);
            if(GetNumberOfCardsDiscarded(discarded) > 0)
            {
                e = DealDamage(CharacterCard, dda.Target, 2, DamageType.Psychic, cardSource: GetCardSource());
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
            }
        }
    }
}
