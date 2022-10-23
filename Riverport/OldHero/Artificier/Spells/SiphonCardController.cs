using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Artificier
{
    public class SiphonCardController : ArtificierBaseCardController
    {
        public SiphonCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            List<DealDamageAction> damages = new List<DealDamageAction>();
            var blast = this.GameController.SelectTargetsAndDealDamage(DecisionMaker, Artificier, 1, DamageType.Energy, 5, false, 0, addStatusEffect: dda=>this.GameController.AddTokensToPool(ManaPool, dda.DidDealDamage?1:0, GetCardSource()), cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(blast); } else { this.GameController.ExhaustCoroutine(blast); }
        }
    }
}
