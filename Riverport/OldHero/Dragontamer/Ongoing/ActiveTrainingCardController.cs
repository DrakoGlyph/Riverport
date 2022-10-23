using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public class ActiveTrainingCardController : CardController
    {
        public ActiveTrainingCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            //Add Extra Draw
            GameController.AddCardControllerToList(CardControllerListType.IncreasePhaseActionCount, this);
        }

        public override void AddTriggers()
        {
            AddAdditionalPhaseActionTrigger(tt=> tt==TurnTaker, Phase.UsePower, 1);
            AddStartOfTurnTrigger(tt => tt == TurnTaker && FindCardsWhere(c=>c.DoKeywordsContain("dragon") && c.IsInPlayAndHasGameText).Count()>0, Empower, TriggerType.MoveCard);
        }

        private IEnumerator Empower(PhaseChangeAction arg)
        {
            List<YesNoCardDecision> results = new List<YesNoCardDecision>();
            var doIt = this.GameController.MakeYesNoCardDecision(HeroTurnTakerController, SelectionType.CardFromHand, Card, storedResults: results, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(doIt); } else { this.GameController.ExhaustCoroutine(doIt); }
            if (DidPlayerAnswerYes(results))
            {
                var select = this.GameController.SelectCardsAndDoAction(HeroTurnTakerController, new LinqCardCriteria(c => c.DoKeywordsContain("dragon") && c.IsInPlayAndNotUnderCard), SelectionType.MoveCardBelowCard, Move, 1, false, 1, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(select); } else { this.GameController.ExhaustCoroutine(select); }
            }
        }

        private IEnumerator Move(Card c)
        {
            var move = this.GameController.MoveCard(TurnTakerController, TurnTaker.Deck.TopCard, c.UnderLocation, doesNotEnterPlay: true, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(move); } else { this.GameController.ExhaustCoroutine(move); }
        }

        public override bool AskIfIncreasingCurrentPhaseActionCount()
        {
            return this.GameController.ActiveTurnPhase.IsUsePower && this.GameController.ActiveTurnTaker == TurnTaker;
        }

        public override IEnumerator Play()
        {
            var drawExtra = IncreasePhaseActionCountIfInPhase(tt => tt == TurnTaker, Phase.UsePower, 1);
            if(UseUnityCoroutines) {yield return this.GameController.StartCoroutine(drawExtra);} else {this.GameController.ExhaustCoroutine(drawExtra);}
        }
    }
}
