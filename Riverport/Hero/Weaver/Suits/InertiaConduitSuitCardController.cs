using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class InertiaConduitSuitCardController : SuitCardController
    {
        public InertiaConduitSuitCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            base.AddTriggers();
            //Increase Damage dealt by Equipped by 1
            AddIncreaseDamageTrigger(dda => dda.DamageSource.IsSameCard(Equipped) && !dda.Target.IsHero, 1);
        }
    }
}
