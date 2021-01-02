using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public abstract class DragonCardController : CardController
    {
        protected DragonCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowNumberOfCardsUnderCard(card);
        }

        public override void AddTriggers()
        {
            
        }


        protected IEnumerator DestroyCardUnderThis(PhaseChangeAction arg = null)
        {
            List<SelectCardDecision> chosen = new List<SelectCardDecision>();
            var decide = this.GameController.SelectCardAndStoreResults(HeroTurnTakerController, SelectionType.DestroyCard, GetCardsBelowThisCard(), chosen, false, true, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(decide); } else { this.GameController.ExhaustCoroutine(decide); }

        }

        public int NumberOfCardsUnder
        {
            get
            {
                return GetCardsBelowThisCard().Count<Card>();
            }
        }

        public bool HasCardsUnder
        {
            get
            {
                return GetCardsBelowThisCard().Count<Card>() > 0;
            }
        }
    }
}
