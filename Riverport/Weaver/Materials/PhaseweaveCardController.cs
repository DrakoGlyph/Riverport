using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class PhaseweaveCardController : MaterialCardController
    {
        public PhaseweaveCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            base.AddTriggers();

            AddMakeDamageIrreducibleTrigger(dda => dda.DamageSource.IsSameCard(Equipped));
        }
    }
}
