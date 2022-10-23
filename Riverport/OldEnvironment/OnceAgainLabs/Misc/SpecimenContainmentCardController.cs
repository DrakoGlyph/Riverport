using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.OnceAgainLabs
{
    public class SpecimenContainmentCardController : CardController
    {
        public SpecimenContainmentCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddWhenDestroyedTrigger(FreeCaptive, TriggerType.MoveCard);
        }

        protected IEnumerator FreeCaptive(DestroyCardAction arg)
        {
            var play = PlayCardsFromLocation(Card.UnderLocation, new LinqCardCriteria(c => true));
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(play); } else { this.GameController.ExhaustCoroutine(play); }

        }

        public override IEnumerator Play()
        {
            var targets = FindAllTargetsWithLowestHitPoints(c => !c.IsCharacter && c.Identifier != Card.Identifier, 1);
            Card contained = null;
            if (targets.Count() == 0)
            {
                //There is nothing to put this on
                var destroy = this.GameController.DestroyCard(DecisionMaker, Card, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
                var play = this.GameController.PlayTopCard(DecisionMaker, TurnTakerController, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(play); } else { this.GameController.ExhaustCoroutine(play); }

            }
            else if (targets.Count() > 1)
            {
                List<SelectCardDecision> results = new List<SelectCardDecision>();
                var select = this.GameController.SelectCardAndStoreResults(DecisionMaker, SelectionType.LowestHP, targets, results, false, true, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(select); } else { this.GameController.ExhaustCoroutine(select); }
                contained = results.FirstOrDefault().SelectedCard;
            }
            else
            {
                //Targets == 1
                contained = targets.FirstOrDefault();
            }
            if (contained != null)
            {
                var move = this.GameController.MoveCard(TurnTakerController, contained, Card.UnderLocation, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(move); } else { this.GameController.ExhaustCoroutine(move); }
            }
        }
    }
}
