using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public class ElgorCardController : DragonCardController
    {
        public ElgorCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            // If Elgor deals damage to a Character, he may take an ongoing or device and put it under him
            AddTrigger<DealDamageAction>((DealDamageAction dda) => dda.DamageSource.IsSameCard(Card) && dda.Target.IsCharacter, Steal, TriggerType.MoveCard, TriggerTiming.After, isConditional: true, requireActionSuccess: true);
            // Reduce damage dealt by Elgor by 1 for each card under him.
            AddReduceDamageTrigger((DealDamageAction dda) => dda.DamageSource.IsSameCard(Card), dda => NumberOfCardsUnder);
        }

        private IEnumerator Steal(DealDamageAction arg)
        {
            if (arg.DidDealDamage)
            {
                // Put a Device or Ongoing under Elgor
                var steal = this.GameController.SelectAndMoveCard(HeroTurnTakerController, c => c.IsInPlayAndNotUnderCard && (c.IsOngoing || c.IsDevice) && c.Owner == arg.Target.Owner, Card.UnderLocation, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(steal); } else { this.GameController.ExhaustCoroutine(steal); }

            }
        }

        public override IEnumerator UsePower(int index = 0)
        {
            switch (index)
            {
                case 0:
                    {
                        var a = GetPowerNumeral(0, 1);
                        var b = GetPowerNumeral(1, 1);
                        // Deal 1 target 1 Fire damage
                        var burn = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, new DamageSource(GameController, Card), b, DamageType.Fire, a, false, 0, false, true, cardSource: GetCardSource());
                        if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(burn); } else { this.GameController.ExhaustCoroutine(burn); }
                        break;
                    }
                case 1:
                    {
                        // Destroy a card under Elgor
                        var destroy = DestroyCardUnderThis();
                        if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
                        break;
                    }
            }
        }
    }
}
