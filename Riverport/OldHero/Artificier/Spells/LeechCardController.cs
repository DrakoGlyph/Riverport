using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Artificier
{
    public class LeechCardController : ArtificierBaseCardController
    {
        public LeechCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            TrackMana();
        }

        public override IEnumerator Play()
        {
            List<int?> removed = new List<int?>();
            var remove = RemoveAnyNumberOfTokensFromTokenPool(ManaPool, removed);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(remove); } else { this.GameController.ExhaustCoroutine(remove); }
            int? mana = removed.FirstOrDefault();
            if(mana != null)
            {
                List<DealDamageAction> storedResultsDamage = new List<DealDamageAction>();
                var damage = this.GameController.SelectTargetsAndDealDamage(DecisionMaker, Artificier, 1, DamageType.Energy, mana.Value, false, 0, false, false, false, c => !c.IsHero, storedResultsDamage: storedResultsDamage, cardSource: GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(damage); } else { this.GameController.ExhaustCoroutine(damage); }
                int damaged = GetNumberOfTargetsDealtDamage(storedResultsDamage);
                if(damaged > 0)
                {
                    var heal = this.GameController.GainHP(CharacterCard, damaged, cardSource: GetCardSource());
                    if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(heal); } else { this.GameController.ExhaustCoroutine(heal); }
                }
            }
        }
    }
}
