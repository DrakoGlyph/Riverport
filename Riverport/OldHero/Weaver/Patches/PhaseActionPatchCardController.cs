using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Weaver
{
    public abstract class PhaseActionPatchCardController : PatchCardController
    {
        public PhaseActionPatchCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }
        protected override TriggerType TriggerType => TriggerType.IncreasePhaseActionCount;        protected virtual Phase IncreasedPhase { get; }
        protected override StatusEffect StatusEffect
        {
            get
            {
                IncreasePhaseActionCountStatusEffect ipacse = new IncreasePhaseActionCountStatusEffect(1);
                ipacse.UntilThisTurnIsOver(Game);
                ipacse.ActualToTurnPhaseCriteria.TurnTaker = EquippedTurnTaker;
                ipacse.ActualToTurnPhaseCriteria.Phase = IncreasedPhase;
                return ipacse;
            }
        }
    }
}
