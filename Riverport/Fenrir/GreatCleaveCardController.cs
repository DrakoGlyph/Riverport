using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Fenrir
{
    public class GreatCleaveCardController : FenrirBaseCardController
    {
        public GreatCleaveCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            if(ShouldActivate("human"))
            {
                var reveal = RevealCard_PlayItOrDiscardIt(TurnTakerController, TurnTaker.Deck, true, false, new LinqCardCriteria(c => c.IsOneShot));
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(reveal); } else { this.GameController.ExhaustCoroutine(reveal); }
            }
            if(ShouldActivate("wolf"))
            {
                List<DealDamageAction> result = new List<DealDamageAction>();
                var damage = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, Fenrir, 5, DamageType.Melee, 1, false, 1, false, storedResultsDamage: result, cardSource: GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(damage); } else { this.GameController.ExhaustCoroutine(damage); }
                DealDamageAction dda = result.FirstOrDefault();
                if(dda.TargetHitPointsAfterBeingDealtDamage < 0)
                {
                    int cleave = Math.Abs((int) dda.TargetHitPointsAfterBeingDealtDamage);
                    var doCleave = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, Fenrir, cleave, DamageType.Melee, 1, false, 0, cardSource: GetCardSource());
                    if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(doCleave); } else { this.GameController.ExhaustCoroutine(doCleave); }
                }
            }
        }
    }
}
