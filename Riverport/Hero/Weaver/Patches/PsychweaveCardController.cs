using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Collections;

namespace Riverport.Weaver
{
    public class PsychweaveCardController : PhaseActionPatchCardController
    {
        public PsychweaveCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }
        protected override Phase IncreasedPhase => Phase.DrawCard;
    }
}
