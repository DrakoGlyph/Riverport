using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class LoomOfFateCardController : CardController
    {
        public LoomOfFateCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowNumberOfCardsAtLocation(TurnTaker.Trash, PatchFiler);
        }

        
        protected readonly LinqCardCriteria PatchFiler = new LinqCardCriteria(c => c.DoKeywordsContain("patch") , "Patch", false, false, "Patch", "Patches");

        public override void AddTriggers()
        {
            AddStartOfTurnTrigger(tt => tt == TurnTaker, Reclaim, TriggerType.MoveCard);

        }

        private IEnumerator Reclaim(PhaseChangeAction arg)
        {
            var reclaim = this.GameController.SelectCardFromLocationAndMoveIt(HeroTurnTakerController, TurnTaker.Trash, PatchFiler, new List<MoveCardDestination>() { new MoveCardDestination(HeroTurnTaker.Hand) }, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(reclaim); } else { this.GameController.ExhaustCoroutine(reclaim); }
        }

        public override IEnumerator UsePower(int index = 0)
        {
            var play = SelectAndPlayCardFromHand(HeroTurnTakerController, true, null, PatchFiler);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(play); } else { this.GameController.ExhaustCoroutine(play); }
        }
    }
}
