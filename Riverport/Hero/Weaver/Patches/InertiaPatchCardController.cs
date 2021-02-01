using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class InertiaPatchCardController : PatchCardController
    {
        public InertiaPatchCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        protected override TriggerType TriggerType => TriggerType.IncreaseDamage;

        protected override StatusEffect StatusEffect
        {
            get
            {
                IncreaseDamageStatusEffect idse = new IncreaseDamageStatusEffect(1);
                idse.SourceCriteria.IsSpecificCard = Equipped;
                idse.UntilStartOfNextTurn(EquippedTurnTaker);
                return idse;
            }
        }

    }
}
