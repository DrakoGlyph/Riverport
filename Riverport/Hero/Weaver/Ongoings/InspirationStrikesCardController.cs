using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class InspirationStrikesCardController : CardController
    {
        public InspirationStrikesCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator UsePower(int index = 0)
        {
            List<DealDamageAction> hit = new List<DealDamageAction>();
            var strike = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, new DamageSource(GameController, CharacterCard), 1, DamageType.Projectile, 3, false, 0, storedResultsDamage: hit, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(strike); } else { this.GameController.ExhaustCoroutine(strike); }
            int n = GetNumberOfTargetsDealtDamage(hit);
            if (n > 0)
            {
                var inspire = DrawCards(HeroTurnTakerController, n);
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(inspire); } else { this.GameController.ExhaustCoroutine(inspire); }
            }
        }
    }
}
