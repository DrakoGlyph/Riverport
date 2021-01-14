using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Collections;

namespace Riverport.Fenrir
{
    public class PaleMoonPendantCardController : FenrirBaseCardController
    {
        public PaleMoonPendantCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddEndOfTurnTrigger(tt => tt == TurnTaker, Recover, TriggerType.GainHP);
            AddReduceDamageTrigger(dda => IsFenrir(dda.Target), dda => ShouldActivate("wolf")?1:0);
        }

        private IEnumerator Recover(PhaseChangeAction arg)
        {
            if(ShouldActivate("human")) {
                var regen = this.GameController.GainHP(CharacterCard, 1, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(regen); } else { this.GameController.ExhaustCoroutine(regen); }
            }
        }

        public override IEnumerator UsePower(int index = 0)
        {
            List<DealDamageAction> hits = new List<DealDamageAction> { };
            var shine = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, new DamageSource(GameController, CharacterCard), 2, DamageType.Radiant, 1, false, 0, false, storedResultsDamage: hits, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(shine); } else { this.GameController.ExhaustCoroutine(shine); }
            if(DidDealDamage(hits))
            {
                var play = SelectAndPlayCardFromHand(HeroTurnTakerController);
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(play); } else { this.GameController.ExhaustCoroutine(play); }
            }
        }
    }
}
