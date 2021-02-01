using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    class LearningExperienceCardController : CardController
    {
        public LearningExperienceCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddTrigger<UsePowerAction>((UsePowerAction upa) => upa.Power.CardController.Card.DoKeywordsContain("dragon"), upa => DrawCard(), TriggerType.DrawCard, TriggerTiming.After);
            AddTrigger<DrawCardAction>((DrawCardAction dca) => dca.HeroTurnTaker == HeroTurnTaker && !HasBeenSetToTrueThisTurn("LearningExperience"), Learn, TriggerType.HiddenLast, TriggerTiming.After);
        }

        private IEnumerator Learn(DrawCardAction arg)
        {
            List<YesNoCardDecision> cardDecisions = new List<YesNoCardDecision>();
            var doIt = this.GameController.MakeYesNoCardDecision(DecisionMaker, SelectionType.MoveCardToUnderCard, Card, arg, cardDecisions, null, GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(doIt); } else { this.GameController.ExhaustCoroutine(doIt); }
            if (DidPlayerAnswerYes(cardDecisions))
            {
                List<MoveCardDestination> UnderDragons = new List<MoveCardDestination>();
                foreach (Card dragon in FindCardsWhere(c => c.DoKeywordsContain("dragon") && c.IsInPlayAndHasGameText))
                {
                    UnderDragons.Add(new MoveCardDestination(dragon.UnderLocation));
                }
                var move = this.GameController.SelectCardFromLocationAndMoveIt(DecisionMaker, HeroTurnTaker.Hand, new LinqCardCriteria(c => true), UnderDragons, false, false, false, true, cardSource: GetCardSource());
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(move); } else { this.GameController.ExhaustCoroutine(move); }
                SetCardPropertyToTrueIfRealAction("LearningExperience");
            }
        }

    }
}
