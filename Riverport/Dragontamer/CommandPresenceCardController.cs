using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public class CommandPresenceCardController : CommandCardController
    {
        public CommandPresenceCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator UsePower(int index = 0)
        {
            Func<Card, IEnumerator> doThis = Bite;
            if (index == 1) doThis = Burn;
            var command = SelectDragonsAndDoThing(doThis);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(command);  } else { this.GameController.ExhaustCoroutine(command); }

        }

        private IEnumerator Burn(Card arg)
        {
            var a = GetPowerNumeral(2, 3);
            var b = GetPowerNumeral(3, 1);
            var burn = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, new DamageSource(GameController, arg), b, DamageType.Fire, a, false, 0, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(burn); } else { this.GameController.ExhaustCoroutine(burn); }
        }

        private IEnumerator Bite(Card arg)
        {
            var a = GetPowerNumeral(0, 1);
            var b = GetPowerNumeral(1, 2);
            var bite = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, new DamageSource(GameController, arg), b, DamageType.Melee, 2, false, 0, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(bite); } else { this.GameController.ExhaustCoroutine(bite); }
        }

    }
}
