using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.OnceAgainLabs
{
    public class DestroyTheEvidenceCardController : CardController
    {
        public DestroyTheEvidenceCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            List<DestroyCardAction> results = new List<DestroyCardAction>();
            var destroy = this.GameController.DestroyCards(DecisionMaker, new LinqCardCriteria(c => c.Identifier == "Replic8Vat"), true, results, true, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
            int burn = 2 + GetNumberOfCardsDestroyed(results);
            var damage = DealDamage(Card, c => c.IsTarget, burn, DamageType.Fire);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(damage); } else { this.GameController.ExhaustCoroutine(damage); }
            destroy = this.GameController.DestroyCard(DecisionMaker, Card, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }

        }
    }
}
