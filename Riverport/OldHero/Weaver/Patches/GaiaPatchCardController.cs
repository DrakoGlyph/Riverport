using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class GaiaPatchCardController : PatchCardController
    {
        public GaiaPatchCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }
        protected override TriggerType TriggerType { get { return TriggerType.GainHP; } }

        protected override IEnumerator Empower(GameAction ga = null)
        {
            List<YesNoCardDecision> decisions = new List<YesNoCardDecision>();
            var decide = this.GameController.MakeYesNoCardDecision(FindHeroTurnTakerController(EquippedTurnTaker as HeroTurnTaker), SelectionType.DestroySelf, Card, storedResults: decisions, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(decide); } else { this.GameController.ExhaustCoroutine(decide); }
            if (DidPlayerAnswerYes(decisions))
            {
                var heal = this.GameController.GainHP(Equipped, 3, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(heal); } else { this.GameController.ExhaustCoroutine(heal); }
                var destroy = this.GameController.DestroyCard(DecisionMaker, Card, false, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
            }
        }
    }
}
