using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ScarletX
{
    public class WasteNotWantNotCardController : CardController
    {
        public WasteNotWantNotCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowNumberOfCardsAtLocation(TurnTaker.Trash, new LinqCardCriteria(c => c.DoKeywordsContain("arrow")));
        }

        public override void AddTriggers()
        {
            AddTrigger<DestroyCardAction>((DestroyCardAction dca) => dca.CardToDestroy.Card.IsTarget && dca.ResponsibleCard == CharacterCard, Reclaim, TriggerType.PutIntoPlay, TriggerTiming.After);
        }

        private IEnumerator Reclaim(DestroyCardAction arg)
        {
            var search = SearchForCards(HeroTurnTakerController, false, true, 1, 0, new LinqCardCriteria(c => c.DoKeywordsContain("arrow")), true, false, false);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(search); } else { this.GameController.ExhaustCoroutine(search); }
        }
    }
}
