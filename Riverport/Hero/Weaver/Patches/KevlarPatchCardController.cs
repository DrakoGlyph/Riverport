using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class KevlarPatchCardController : PatchCardController
    {
        public KevlarPatchCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        protected override TriggerType TriggerType => TriggerType.ReduceDamage; 

        protected override StatusEffect StatusEffect
        {
            get
            {
                ReduceDamageStatusEffect rdse = new ReduceDamageStatusEffect(1);
                rdse.SourceCriteria.IsSpecificCard = Equipped;
                rdse.UntilStartOfNextTurn(EquippedTurnTaker);
                return rdse;
            }
        }
    }
}
