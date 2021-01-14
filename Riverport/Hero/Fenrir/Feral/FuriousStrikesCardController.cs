using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Fenrir
{
    public class FuriousStrikesCardController : FenrirBaseCardController
    {
        public FuriousStrikesCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowIfSpecificCardIsInPlay("Frenzy");
        }

        public override IEnumerator Play()
        {
            var strike = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, Fenrir, 1, DamageType.Melee, 1, false, 0, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(strike); } else { this.GameController.ExhaustCoroutine(strike); }
            strike = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, Fenrir, 2, DamageType.Melee, 2, false, 0, cardSource: GetCardSource());
            if(ShouldActivate("wolf"))
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(strike); } else { this.GameController.ExhaustCoroutine(strike); }
            if(Frenzy.IsInPlayAndHasGameText)
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(strike); } else { this.GameController.ExhaustCoroutine(strike); }
        }


    }
}
