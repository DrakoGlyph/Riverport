using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ArkHive
{
    public class NovasteelChassisCardController : ArkHiveBaseCardController
    {
        public NovasteelChassisCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowNumberOfCardsAtLocation(TurnTaker.Trash, new LinqCardCriteria(c => c.DoKeywordsContain("nanobot"), "Nanobots", false, false, "Nanobot", "Nanobots"));
        }

        public override void AddTriggers()
        {
            AddReduceDamageTrigger(c => c == CharacterCard, 1);
            AddEndOfTurnTrigger(tt => tt == TurnTaker && TurnTaker.Trash.Cards.Any(c => c.DoKeywordsContain("nanobot")), pca => this.GameController.SelectCardFromLocationAndMoveIt(DecisionMaker, TurnTaker.Trash, new LinqCardCriteria(c => c.DoKeywordsContain("nanobot")), new List<MoveCardDestination>() { new MoveCardDestination(HeroTurnTaker.Hand) }, cardSource: GetCardSource()), TriggerType.MoveCard);
        }
    }
}
