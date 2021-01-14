using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Collections;

namespace Riverport.Fenrir
{
    public class MaintainControlCardController : FenrirBaseCardController
    {
        public MaintainControlCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddEndOfTurnTrigger(tt => tt == TurnTaker, Control, TriggerType.DiscardCard);
            AddTrigger<PlayCardAction>((PlayCardAction pca) => pca.IsSuccessful && pca.CardToPlay.DoKeywordsContain("feral") && IsFirstTimeCardPlayedThisTurn(pca.CardToPlay, c => c.DoKeywordsContain("feral"), TriggerTiming.After), WolfPlay, TriggerType.PlayCard, TriggerTiming.After);
        }

        private IEnumerator WolfPlay(PlayCardAction arg)
        {
            if(ShouldActivate("wolf"))
            {
                var play = SelectAndPlayCardFromHand(HeroTurnTakerController, true, null, new LinqCardCriteria(c => !c.DoKeywordsContain("feral")));
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(play); } else { this.GameController.ExhaustCoroutine(play); } 
            }
        }

        private IEnumerator Control(PhaseChangeAction arg)
        {
            if (ShouldActivate("human"))
            {
                List<DiscardCardAction> tossed = new List<DiscardCardAction>();
                var control = SelectAndDiscardCards(HeroTurnTakerController, null, false, 0, tossed, cardCriteria: new LinqCardCriteria(c => c.DoKeywordsContain("feral")));
                if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(control); } else { this.GameController.ExhaustCoroutine(control); }
                int replenish = GetNumberOfCardsDiscarded(tossed);
                if(replenish > 0)
                {
                    var draw = DrawCards(HeroTurnTakerController, replenish, false, true);
                    if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(draw); } else { this.GameController.ExhaustCoroutine(draw); }
                }
            }
        }

    }
}
