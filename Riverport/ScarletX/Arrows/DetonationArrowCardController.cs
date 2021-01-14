using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ScarletX
{
    public class DetonationArrowCardController : ArrowBaseCardController
    {
        public DetonationArrowCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            Type = new TriggerType[] { TriggerType.DealDamage };
        }

        protected override IEnumerator FireArrow(DestroyCardAction dca)
        {
            var stick = this.GameController.SelectTargetsAndDealDamage(DecisionMaker, new DamageSource(GameController, CharacterCard), 2, DamageType.Projectile, 1, false, 0, false, true, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(stick); } else { this.GameController.ExhaustCoroutine(stick); }
            var detonate = this.GameController.SelectTargetsAndDealDamage(DecisionMaker, new DamageSource(GameController, CharacterCard), 1, DamageType.Fire, 3, false, 0, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(detonate); } else { this.GameController.ExhaustCoroutine(detonate); }
        }
    }
}
