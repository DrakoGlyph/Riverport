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
            List<MoveCardAction> storedResults = new List<MoveCardAction>();
            var search = this.GameController.SelectCardsFromLocationAndMoveThem(DecisionMaker, TurnTaker.Deck, 0, H - 1, new LinqCardCriteria(c => c.Identifier == "SpareNanobot"), new List<MoveCardDestination>() { new MoveCardDestination(TurnTaker.PlayArea) }, true, true, true, false, null, storedResultsMove: storedResults, selectionType: SelectionType.PutIntoPlay, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(search); } else { this.GameController.ExhaustCoroutine(search); }
            int deployed = GetNumberOfCardsMoved(storedResults);
            if (deployed > 0)
            {
                var pay = DealDamage(CharacterCard, CharacterCard, 2 * deployed, DamageType.Energy, cardSource: GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(pay); } else { this.GameController.ExhaustCoroutine(pay); }

            }
        }
    }
}
