using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Artificier
{
    public class DispelCardController : ArtificierBaseCardController
    {
        public DispelCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {

        }

        public override IEnumerator Play()
        {
            List<SelectCardDecision> target = new List<SelectCardDecision>();
            var select = this.GameController.SelectCardAndStoreResults(DecisionMaker, SelectionType.MoveCard, new LinqCardCriteria(c => IsEnchanted(c)), target, true, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(select); } else { this.GameController.ExhaustCoroutine(select); }
            if(DidSelectCard(target))
            {
                Card c = GetSelectedCard(target);
                int gain = 0;
                foreach (Card card in c.NextToLocation.Cards)
                {
                    if (card.DoKeywordsContain("enchantment"))
                    {
                        gain++;
                        var destroy = this.GameController.DestroyCard(DecisionMaker, card, responsibleCard: CharacterCard, cardSource: GetCardSource());
                        if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(destroy); } else { this.GameController.ExhaustCoroutine(destroy); }
                    }
                }
                if (gain > 0)
                {
                    var damage = DealDamage(CharacterCard, c, 2*gain, DamageType.Energy);
                    if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(damage); } else { this.GameController.ExhaustCoroutine(damage); }
                }
            }
        }
    }
}
