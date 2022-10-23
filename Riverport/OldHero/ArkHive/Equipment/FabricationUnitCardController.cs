using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ArkHive
{
    class FabricationUnitCardController : ArkHiveBaseCardController
    {
        public FabricationUnitCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowNumberOfCardsAtLocation(TurnTaker.Deck, new LinqCardCriteria(c=>IsEquipment(c)));
        }

        public override IEnumerator UsePower(int index = 0)
        {
            var search = SearchForCards(DecisionMaker, true, false, 1, 1, new LinqCardCriteria(c => IsEquipment(c)), true, false, false);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(search); } else { this.GameController.ExhaustCoroutine(search); }
        }
    }
}
