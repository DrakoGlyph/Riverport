using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ScarletX
{
    public class AnticipateTheNeedCardController : CardController
    {
        public AnticipateTheNeedCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddStartOfTurnTrigger(tt => tt == TurnTaker, Anticipate, TriggerType.DrawCard);
            AddEndOfTurnTrigger(tt => tt == TurnTaker, Need, TriggerType.DrawCard);
        }

        private IEnumerator Need(PhaseChangeAction arg)
        {
            List<YesNoCardDecision> decide = new List<YesNoCardDecision>();
            var decision = this.GameController.MakeYesNoCardDecision(DecisionMaker, SelectionType.DestroySelf, Card, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(decision); } else { this.GameController.ExhaustCoroutine(decision); }
            if(DidPlayerAnswerYes(decide))
            {
                var draw = DrawCard();
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(draw); } else { this.GameController.ExhaustCoroutine(draw); }
                var destroy = this.GameController.DestroyCard(HeroTurnTakerController, Card, false, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
            }
        }

        private IEnumerator Anticipate(PhaseChangeAction arg)
        {
            var draw = DrawCard();
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(draw); } else { this.GameController.ExhaustCoroutine(draw); }

        }
    }
}
