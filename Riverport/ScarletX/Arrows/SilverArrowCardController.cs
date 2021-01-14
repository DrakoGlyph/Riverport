using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.ScarletX
{
    public class SilverArrowCardController : ArrowBaseCardController
    {
        public SilverArrowCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            Type = new TriggerType[] { TriggerType.DealDamage };
        }

        protected override IEnumerator FireArrow(DestroyCardAction dca)
        {
            var damage = this.GameController.SelectTargetsAndDealDamage(DecisionMaker, new DamageSource(GameController, CharacterCard), 3, DamageType.Projectile, 1, false, 0, false, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(damage); } else { this.GameController.ExhaustCoroutine(damage); }

        }
    }
}
