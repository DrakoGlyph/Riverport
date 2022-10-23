using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Artificier
{
    public class SymbioticEvolutionCardController : ArtificierBaseCardController
    {
        public SymbioticEvolutionCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddStartOfTurnTrigger(tt => tt == TurnTaker, Evolve, TriggerType.IncreasePhaseActionCount);
        }

        private IEnumerator Evolve(PhaseChangeAction arg)
        {
            List<SelectTurnPhaseDecision> storedResults = new List<SelectTurnPhaseDecision>();
            var select = this.GameController.SelectPhase(DecisionMaker, new List<TurnPhase> { new TurnPhase(TurnTaker, Phase.PlayCard, Card), new TurnPhase(TurnTaker, Phase.DrawCard, Card) }, SelectionType.PlayExtraCard, storedResults, false, GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(select); } else { this.GameController.ExhaustCoroutine(select); }
            SelectTurnPhaseDecision stpd = storedResults.FirstOrDefault();
            IncreasePhaseActionCountStatusEffect ipacse = new IncreasePhaseActionCountStatusEffect(1);
            ipacse.ActualToTurnPhaseCriteria.Phase = stpd.SelectedPhase.Phase;
            ipacse.ActualToTurnPhaseCriteria.TurnTaker = stpd.SelectedTurnTaker;
            ipacse.UntilThisTurnIsOver(Game);
            var status = AddStatusEffect(ipacse);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(status); } else { this.GameController.ExhaustCoroutine(status); }
        }
    }
}
