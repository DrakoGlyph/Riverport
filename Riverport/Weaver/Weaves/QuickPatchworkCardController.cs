using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Weaver
{
    public class QuickPatchworkCardController : CardController
    {
        public QuickPatchworkCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowNumberOfCardsAtLocation(TurnTaker.Trash, new LinqCardCriteria(c => c.DoKeywordsContain("suit"), "Suits", false, false, "Suit", "Suits"));
        }

        public override void AddTriggers()
        {
            AddTrigger<DestroyCardAction>((DestroyCardAction dca) => dca.CardToDestroy.Card.Owner == TurnTaker && dca.CardToDestroy.Card.DoKeywordsContain("equipment"), Patch, TriggerType.CancelAction, TriggerTiming.Before);
        }

        private IEnumerator Patch(DestroyCardAction arg)
        {
            List<DiscardCardAction> mat = new List<DiscardCardAction>();
            var sac = SelectAndDiscardCards(HeroTurnTakerController, 1, true, 0, mat, extraInfo: PatchText, cardCriteria: new LinqCardCriteria(c => c.DoKeywordsContain("material")));
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(sac); } else { this.GameController.ExhaustCoroutine(sac); }
            if(DidDiscardCards(mat))
            {
                var patch = CancelAction(arg);
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(patch); } else { this.GameController.ExhaustCoroutine(patch); }
            }
        }

        public override IEnumerator UsePower(int index = 0)
        {
            List<DiscardCardAction> mat = new List<DiscardCardAction>();
            var sac = SelectAndDiscardCards(HeroTurnTakerController, 1, false, 0, mat);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(sac); } else { this.GameController.ExhaustCoroutine(sac); }
            if(DidDiscardCards(mat))
            {
                var remake = SearchForCards(HeroTurnTakerController, false, true, 0, 1, new LinqCardCriteria(c => c.DoKeywordsContain("suit")), true, false, false);
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(remake); } else { this.GameController.ExhaustCoroutine(remake); }
            }
        }

        private string PatchText()
        {
            return "Select card to patch with:";
        }
    }
}
