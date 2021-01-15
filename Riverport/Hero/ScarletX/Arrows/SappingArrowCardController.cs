using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.ScarletX
{
    public class SappingArrowCardController : ArrowBaseCardController
    {
        public SappingArrowCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        protected override IEnumerator FireArrow(GameAction ga = null)
        {
            List<DestroyCardAction> result = new List<DestroyCardAction>();
            var dest = this.GameController.SelectAndDestroyCard(DecisionMaker, new LinqCardCriteria(c => (c.IsOngoing || c.IsDevice) && IsVillain(c) && c.IsInPlayAndHasGameText), true, result, CharacterCard, GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(dest); } else { this.GameController.ExhaustCoroutine(dest); }
            if(DidDestroyCard(result))
            {
                DestroyCardAction sapped = result.FirstOrDefault();
                var punish = DealDamage(CharacterCard, sapped.CardToDestroy.TurnTaker.CharacterCard, 2, DamageType.Projectile, cardSource: GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(punish); } else { this.GameController.ExhaustCoroutine(punish); }
            }
        }
    }
}
