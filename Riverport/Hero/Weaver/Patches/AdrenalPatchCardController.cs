﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class AdrenalPatchCardController : PhaseActionPatchCardController
    {
        public AdrenalPatchCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }
        protected override Phase IncreasedPhase => Phase.UsePower;
    }
}
