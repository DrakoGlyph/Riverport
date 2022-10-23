using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ArkHive
{
    public class SpareNanobotCardController : ArkHiveBaseCardController
    {
        public SpareNanobotCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddIfTheTargetThatThisCardIsNextToLeavesPlayDestroyThisCardTrigger();

            AddStartOfTurnTrigger(tt => tt == Equipped.Owner, Nanoboost, TriggerType.Hidden);
        }

        private IEnumerator Nanoboost(PhaseChangeAction arg)
        {
            List<Function> modes = new List<Function>() {
                new Function(arg.DecisionMaker, "Play a card", SelectionType.PlayCard, () => this.GameController.SelectHeroToPlayCard(arg.DecisionMaker, additionalCriteria: new LinqTurnTakerCriteria(tt => tt == Equipped.Owner), cardSource: GetCardSource()), this.GameController.CanPlayCards(FindTurnTakerController(Equipped.Owner), GetCardSource())),
                new Function(arg.DecisionMaker, "Draw a card", SelectionType.DrawCard, () => DrawCard((HeroTurnTaker) Equipped.Owner), this.GameController.CanDrawCards((HeroTurnTakerController) FindTurnTakerController(Equipped.Owner), GetCardSource())),
                new Function(arg.DecisionMaker, "Use a power", SelectionType.UsePower, () => this.GameController.SelectHeroToUsePower(arg.DecisionMaker, additionalCriteria: new LinqTurnTakerCriteria(tt => tt == Equipped.Owner), cardSource: GetCardSource()), this.GameController.CanUsePowers((HeroTurnTakerController) FindTurnTakerController(Equipped.Owner), GetCardSource()))
            };
            var nanoboost = SelectAndPerformFunction(arg.DecisionMaker, modes, true, null, arg, "The Nanobots failed to activate!");
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(nanoboost); } else { this.GameController.ExhaustCoroutine(nanoboost); }
        }

        public Card Equipped
        {
            get
            {
                return GetCardThisCardIsNextTo();
            }
        }

        public override IEnumerator DeterminePlayLocation(List<MoveCardDestination> storedResults, bool isPutIntoPlay, List<IDecision> decisionSources, Location overridePlayArea = null, LinqTurnTakerCriteria additionalTurnTakerCriteria = null)
        {
            var equip = SelectCardThisCardWillMoveNextTo(new LinqCardCriteria(c => c.IsHeroCharacterCard && c != CharacterCard && !c.NextToLocation.Cards.Any(card=>card.Identifier == "SpareNanobot")), storedResults, isPutIntoPlay, decisionSources);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(equip); } else { this.GameController.ExhaustCoroutine(equip); }
        }
    }
}
