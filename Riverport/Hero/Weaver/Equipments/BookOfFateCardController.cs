using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class BookOfFateCardController : CardController
    {
        public BookOfFateCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddTrigger<PlayCardAction>((PlayCardAction pca) => pca.CardToPlay.DoKeywordsContain("weave"), Weave, TriggerType.UsePower, TriggerTiming.After);
        }

        private IEnumerator Weave(PlayCardAction arg)
        {
            if(IsFirstTimeCardPlayedThisTurn(arg.CardToPlay, c => c.DoKeywordsContain("weave"), TriggerTiming.After))
            {
                var weave = SelectAndUsePower(this);
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(weave); } else { this.GameController.ExhaustCoroutine(weave); }
            }
        }

        public override IEnumerator UsePower(int index = 0)
        {
            List<SelectLocationDecision> storedResults = new List<SelectLocationDecision>();
            var pick = SelectDecks(HeroTurnTakerController, 1, SelectionType.RevealCardsFromDeck, l => true, storedResults);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(pick); } else { this.GameController.ExhaustCoroutine(pick); }
            var read = RevealCardsFromTopOfDeck_PutOnTopAndOnBottom(HeroTurnTakerController, TurnTakerController, GetSelectedLocation(storedResults), 3, 3, 0);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(read); } else { this.GameController.ExhaustCoroutine(read); }
        }
    }
}
