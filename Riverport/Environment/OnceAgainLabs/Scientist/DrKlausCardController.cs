using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.OnceAgainLabs
{
    public class DrKlausCardController : CardController
    {
        public DrKlausCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddIncreaseDamageTrigger(dda => dda.Target.IsVillain && dda.DamageSource.IsEnvironmentCard, 1);
            AddReduceDamageTrigger(dda => dda.Target.IsHero && dda.DamageSource.IsEnvironmentCard, dda => 1);
            AddEndOfTurnTrigger(tt => tt == TurnTaker, Regen, TriggerType.GainHP);
        }

        private IEnumerator Regen(PhaseChangeAction arg)
        {
            var heal = this.GameController.GainHP(Card, 1, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(heal); } else { this.GameController.ExhaustCoroutine(heal); }

            Card klaws = FindCard("DrKlaws");
            if (klaws.IsInPlayAndHasGameText)
            {
                heal = this.GameController.GainHP(klaws, 1);
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(heal); } else { this.GameController.ExhaustCoroutine(heal); }
            }
        }
    }

}
