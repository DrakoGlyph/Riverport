using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public abstract class CommandCardController : CardController
    {
        public CommandCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowSpecialString(CommandPresenceString);
        }

        public int CommandPresence
        {
            get
            {
                return 1 + (FindCardsWhere(c => c.Title.Contains("Command Presence") && c.IsInPlayAndNotUnderCard)as List<Card>).Count;
            }
        }

        private string CommandPresenceString()
        {
            string rtn = "You can control ";
            rtn += CommandPresence;
            rtn += " dragon" + (CommandPresence == 1 ? "." : "s.");
            return rtn;
        }

        protected IEnumerator SelectDragonsAndDoThing(Func<Card, IEnumerator> thing)
        {
            var command = this.GameController.SelectCardsAndDoAction(HeroTurnTakerController, new LinqCardCriteria(c => c.DoKeywordsContain("dragon") && c.IsInPlayAndNotUnderCard), SelectionType.DealDamage, thing, CommandPresence, false, 0, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(command); } else { this.GameController.ExhaustCoroutine(command); }
        }


        protected IEnumerator SelectDragonToMoveUnder()
        {
            var empower = this.GameController.SelectCardsAndDoAction(HeroTurnTakerController, new LinqCardCriteria(c => c.DoKeywordsContain("dragon") && c.IsInPlayAndNotUnderCard), SelectionType.MoveCard, MoveUnder, 1, false, 0, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(empower); } else { this.GameController.ExhaustCoroutine(empower); }
        }

        private IEnumerator MoveUnder(Card c)
        {
            var move = this.GameController.MoveCard(TurnTakerController, Card, c.UnderLocation, doesNotEnterPlay: true, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(move); } else { this.GameController.ExhaustCoroutine(move); }
        }
    }
}
