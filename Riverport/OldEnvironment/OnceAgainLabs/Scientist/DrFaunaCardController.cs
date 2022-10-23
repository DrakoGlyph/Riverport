using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.OnceAgainLabs
{
    public class DrFaunaCardController : CardController
    {
        readonly LinqCardCriteria NonScientist = new LinqCardCriteria(c => !c.DoKeywordsContain("scientist"), "non-Scientist");

        public DrFaunaCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowLowestHP(cardCriteria: NonScientist);
        }

        public override void AddTriggers()
        {
            AddDealDamageAtStartOfTurnTrigger(TurnTaker, Card, NonScientist.Criteria, TargetType.LowestHP, 2, DamageType.Psychic);
        }
    }
}
