using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.OnceAgainLabs
{
    public class ChameleonCardController : CardController
    {
        public ChameleonCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowLowestHP(1, null, new LinqCardCriteria(c => !c.DoKeywordsContain("dragon"), "non-Dragon", false, false));
        }

        public override void AddTriggers()
        {
            AddImmuneToDamageTrigger(dda => dda.Target == Card && GameController.ActiveTurnTaker.IsEnvironment);
            AddDealDamageAtEndOfTurnTrigger(TurnTaker, Card, c => !c.DoKeywordsContain("dragon"), TargetType.HighestHP, 3, DamageType.Melee);
        }
    }
}
