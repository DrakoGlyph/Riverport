using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Collections;

namespace Riverport.ArkHive
{
    public class PreemptiveActionCardController : ArkHiveBaseCardController
    {
        public PreemptiveActionCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            var draw = DrawCard(optional: true);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(draw); } else { this.GameController.ExhaustCoroutine(draw); }

            var play = SelectAndPlayCardFromHand(DecisionMaker, cardCriteria: PlanFilter, associateCardSource: true);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(play); } else { this.GameController.ExhaustCoroutine(play); }

            List<ActivateAbilityDecision> storedResults = new List<ActivateAbilityDecision>();
            var enact = this.GameController.SelectAndActivateAbility(DecisionMaker, "enact", null, storedResults, true, GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(enact); } else { this.GameController.ExhaustCoroutine(enact); }
            ActivateAbilityDecision aad = storedResults.FirstOrDefault();
            if (aad != null)
            {
                Card used = aad.SelectedCard;
                var destroy = this.GameController.DestroyCard(DecisionMaker, used, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
            }
        }
    }
}
