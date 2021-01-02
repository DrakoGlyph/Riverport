using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public class BurnCommandCardController : CommandCardController
    {
        public BurnCommandCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            var damage = SelectDragonsAndDoThing(Bite);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(damage); } else { this.GameController.ExhaustCoroutine(damage); }
            var empower = SelectDragonToMoveUnder();
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(empower); } else { this.GameController.ExhaustCoroutine(empower); }
        }

        private IEnumerator Bite(Card arg)
        {
            var burn = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, new DamageSource(GameController, arg), 1, DamageType.Fire, 3, false, 0, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(burn); } else { this.GameController.ExhaustCoroutine(burn); }
        }
    }
}
