using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Fenrir
{
    class FenrirTurnTakeController : HeroTurnTakerController
    {
        public FenrirTurnTakeController(TurnTaker turnTaker, GameController gameController) : base(turnTaker, gameController)
        {
            
        }

    }
}
