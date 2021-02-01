using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ArkHive
{
    public class DeploySparesCardController : ArkHiveBaseCardController
    {
        public DeploySparesCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            List<SelectCardDecision> storedResults = new List<SelectCardDecision>();
            var search = SearchForCards(DecisionMaker, true, false, 0, H - 1, new LinqCardCriteria(c => c.Identifier == "SpareNanobot"), true, false, false, storedResults: storedResults, shuffleAfterwards: true);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(search); } else { this.GameController.ExhaustCoroutine(search); }
            int selected = GetNumberOfCardsSelected(storedResults);
            Console.WriteLine("Selected = " + selected);
            var damage = DealDamage(CharacterCard, CharacterCard, selected * 2, DamageType.Energy, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(damage); } else { this.GameController.ExhaustCoroutine(damage); }
        }
    }
}
