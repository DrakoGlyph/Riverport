using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    class DragonEggCardController : CardController
    {
        public DragonEggCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowNumberOfCardsAtLocation(TurnTaker.Deck, new LinqCardCriteria(c => c.DoKeywordsContain("dragon")));
            SpecialStringMaker.ShowNumberOfCardsAtLocation(TurnTaker.Trash, new LinqCardCriteria(c => c.DoKeywordsContain("dragon")));

        }

        public override void AddTriggers()
        {
            AddStartOfTurnTrigger(tt => tt == TurnTaker, DestroyThisCardResponse, TriggerType.DestroySelf);
            AddAfterDestroyedAction(Hatch);
        }

        private IEnumerator Hatch(GameAction arg)
        {
            var search = SearchForCards(HeroTurnTakerController, true, true, 1, 1, new LinqCardCriteria(c => c.DoKeywordsContain("dragon")), true, false, false);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(search); } else { this.GameController.ExhaustCoroutine(search); }
        }
    }
}
