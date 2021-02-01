using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class PhaseweaveCardController : PatchCardController
    {
        public PhaseweaveCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }
        protected override TriggerType TriggerType => TriggerType.MakeDamageIrreducible;
        protected override StatusEffect StatusEffect
        {
            get
            {
                MakeDamageIrreducibleStatusEffect mdise = new MakeDamageIrreducibleStatusEffect();
                mdise.SourceCriteria.IsSpecificCard = Equipped;
                mdise.UntilEndOfNextTurn(EquippedTurnTaker);
                return mdise;
            }
        }
    }
}
