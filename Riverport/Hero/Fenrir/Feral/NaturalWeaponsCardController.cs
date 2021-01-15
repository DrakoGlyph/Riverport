using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Collections;

namespace Riverport.Fenrir
{
    public class NaturalWeaponsCardController : FenrirBaseCardController
    {
        public NaturalWeaponsCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            AllowFastCoroutinesDuringPretend = false;
        }

        public override void AddTriggers()
        {
            AddTrigger<DealDamageAction>((DealDamageAction dda) => dda.DamageType == DamageType.Psychic && IsFenrir(dda.DamageSource.Card), Fear, TriggerType.CreateStatusEffect, TriggerTiming.After);

        }

        private IEnumerator Fear(DealDamageAction arg)
        {
            ReduceDamageStatusEffect rdse = new ReduceDamageStatusEffect(1);

            rdse.Identifier = "Fear";
            rdse.SourceCriteria.IsSpecificCard = arg.Target;
            rdse.UntilStartOfNextTurn(TurnTaker);
            var fear = AddStatusEffect(rdse);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(fear); } else { this.GameController.ExhaustCoroutine(fear); }
        }

        public override IEnumerator UsePower(int index = 0)
        {
            if (ShouldActivate("human")) {
                var draw = this.GameController.SelectHeroToDrawCard(HeroTurnTakerController, cardSource: GetCardSource());
                if(UseUnityCoroutines) { this.GameController.StartCoroutine(draw); } else { this.GameController.ExhaustCoroutine(draw); }
            }
            if (ShouldActivate("wolf")) {
                List<DealDamageAction> damage = new List<DealDamageAction> {
                    new DealDamageAction(GetCardSource(), Fenrir, null, Frenzy.IsInPlay ? 3 : 1, DamageType.Melee),
                    new DealDamageAction(GetCardSource(), Fenrir, null, 1, DamageType.Psychic)
                };
                var hit = SelectTargetAndDealMultipleInstancesOfDamage(damage);
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(hit); } else { this.GameController.ExhaustCoroutine(hit); }
            };
        }
    }
}
