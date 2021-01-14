using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class KevlarShellSuitCardController : SuitCardController
    {
        public KevlarShellSuitCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            base.AddTriggers();

            AddReduceDamageTrigger(c => c == Equipped, 1);
        }
    }
}
