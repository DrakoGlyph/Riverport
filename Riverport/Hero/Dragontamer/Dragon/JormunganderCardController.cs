using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public class JormunganderCardController : DragonCardController
    {
        public JormunganderCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator UsePower(int index = 0)
        {
            int targets = GetPowerNumeral(0, 1);
            int damage = GetPowerNumeral(1, 2);
            var burn = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, new DamageSource(GameController, Card), damage, DamageType.Fire, targets, false, 0, additionalCriteria: c => !c.IsHero, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(burn); } else { this.GameController.ExhaustCoroutine(burn); }


            if (HasCardsUnder)
            {
                List<YesNoCardDecision> decision = new List<YesNoCardDecision>();
                var decide = this.GameController.MakeYesNoCardDecision(HeroTurnTakerController, SelectionType.MoveCardFromUnderCard, Card, null, decision, null, GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(decide); } else { this.GameController.ExhaustCoroutine(decide); }
                if (DidPlayerAnswerYes(decision))
                {


                    var destroy = DestroyCardUnderThis();
                    if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }

                    var raze = this.GameController.SelectAndDestroyCard(HeroTurnTakerController, new LinqCardCriteria(c => c.IsEnvironment && c.IsInPlayAndNotUnderCard), true, null, Card, GetCardSource());
                    if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(raze); } else { this.GameController.ExhaustCoroutine(raze); }


                }
            }
        }

    }
}
