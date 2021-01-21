using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.OnceAgainLabs
{
    public class RaptorCardController : CardController
    {
        public readonly LinqCardCriteria NonDragon = new LinqCardCriteria(c => !c.DoKeywordsContain("dragon"), "non-Dragon");
        public RaptorCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowNumberOfCardsInPlay(NonDragon);
            SpecialStringMaker.ShowLowestHP(cardCriteria: NonDragon);
        }

        public override void AddTriggers()
        {
            AddDealDamageAtStartOfTurnTrigger(TurnTaker, Card, NonDragon.Criteria, TargetType.LowestHP, FindCardsWhere(c => c.DoKeywordsContain("dragon")).Count(), DamageType.Melee);
        }
    }
}
