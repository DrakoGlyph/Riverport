using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Artificier
{
    public class ManaBurnCardController : ArtificierBaseCardController
    {
        public ManaBurnCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            TrackMana();
        }

        public override IEnumerator Play()
        {
            List<int?> moved = new List<int?>();
            var remove = RemoveAnyNumberOfTokensFromTokenPool(ManaPool, moved);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(remove); } else { this.GameController.ExhaustCoroutine(remove); }
            int? mana = moved.FirstOrDefault();
            if(mana != null)
            {
                var burn = this.GameController.SelectTargetsAndDealDamage(DecisionMaker, Artificier, mana.Value, DamageType.Energy, 1, false, 0, additionalCriteria: c => !c.IsHero, cardSource: GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(burn); } else { this.GameController.ExhaustCoroutine(burn); }
            }
        }
    }
}
