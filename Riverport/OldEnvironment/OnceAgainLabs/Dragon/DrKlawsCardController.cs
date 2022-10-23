using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.OnceAgainLabs
{
    public class DrKlawsCardController : CardController
    {
        public DrKlawsCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddDealDamageAtStartOfTurnTrigger(TurnTaker, Card, c => !c.IsEnvironment, TargetType.All, 1, DamageType.Sonic);
        }
    }
}
