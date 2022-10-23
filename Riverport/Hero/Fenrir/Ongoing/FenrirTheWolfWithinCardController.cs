using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Fenrir
{
    public class FenrirTheWolfWithinCardController : BaseFenrirCardController
    {
        public FenrirTheWolfWithinCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController, true)
        {
        }

        public override void AddTriggers()
        {
            /*
             * Increase Melee damage dealt by Fenrir by 1
             * Human: When Fenrir takes damage, draw a card. If you have more then 7 cards in hand, Transform.
             * Wolf: When Fenrir takes damage, he may deal the source of that damage 1 Melee damage
             */

            AddIncreaseDamageTrigger(dda => dda.DamageSource.IsSameCard(CharacterCard) && dda.DamageType == DamageType.Psychic, 1);
            AddTrigger<DealDamageAction>(dda => IsHuman && dda.Target == CharacterCard, ContainWolf, TriggerType.DrawCard, TriggerTiming.After, isActionOptional: false);
            AddTrigger<DealDamageAction>(dda => IsWolf && dda.Target == CharacterCard, WolfWithin, TriggerType.DealDamage, TriggerTiming.After, isActionOptional: true);
        }

        private IEnumerator WolfWithin(DealDamageAction arg)
        {
            // When Fenrir takes damage, he may deal the source of that damage 1 Melee damage
            var e = DealDamage(arg.Target, arg.DamageSource.Card, 1, DamageType.Melee, optional: true, isCounterDamage: true, cardSource: GetCardSource());
            if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
            else this.GameController.ExhaustCoroutine(e);
        }

        private IEnumerator ContainWolf(DealDamageAction arg)
        {
            // When Fenrir takes damage, draw a card. If you have more than 7 cards in hand, Transform.
            IEnumerator e = DrawCard();
            if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
            else this.GameController.ExhaustCoroutine(e);

            if(HeroTurnTaker.NumberOfCardsInHand > 7)
            {
                e = Transform();
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
            }
        }
    }
}
