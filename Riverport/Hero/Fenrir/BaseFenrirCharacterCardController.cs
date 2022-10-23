using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Fenrir
{
    public abstract class BaseFenrirCharacterCardController : HeroCharacterCardController
    {
        public BaseFenrirCharacterCardController(Card card, TurnTakerController ttc) : base(card, ttc) {
            SpecialStringMaker.ShowSpecialString(ShowForm, () => false, () => FindCardsWhere(DoesFormMatter));
            
        }

        public virtual bool IsHuman
        {
            get
            {
                IEnumerable<Card> character = FindCardsWhere(c => c.IsCharacter && c.IsInPlay && c.DoKeywordsContain("human") && c.SharedIdentifier == Card.SharedIdentifier);
                return DidFindCard(character);
            }
        }
        public virtual bool IsWolf {
            get {

                IEnumerable<Card> character = FindCardsWhere(c => c.IsCharacter && c.IsInPlay && c.DoKeywordsContain("wolf") && c.SharedIdentifier == Card.SharedIdentifier);
                return DidFindCard(character);
            }
        }

        public virtual Card Human { get => FindCard("FenrirHumanCharacter", false); }
        public virtual Card Wolf { get => FindCard("FenrirWolfCharacter", false); }

        public override bool CanBeMovedOutOfGame => true;

        public string ShowForm()
        {
            string rtn = "Fenrir can use ";
            if (IsHuman && IsWolf) rtn += "both {Human} and {Wolf}";
            else if (IsHuman) rtn += "{Human}";
            else rtn += "{Wolf}";
            rtn += " abilities.";
            return rtn;
        }

        private bool DoesFormMatter(Card c)
        {
            if (c.Owner != TurnTaker || c.IsCharacter) return false;
            CardController cc = FindCardController(c);
            if (!(cc is BaseFenrirCardController)) return false;
            return (cc as BaseFenrirCardController).DoesFormMatter;
        }

        /**
         * Virtual because some Promos use different rules for transforming
         * i.e. Twin Soul Fenrir
         */

        public virtual IEnumerator Transform()
        {
            if (!IsWolf)
            {
                if (Wolf == null) Console.WriteLine("Whoopsie! There is no Wolf Within!");
                var e = this.GameController.SwitchCards(CharacterCardWithoutReplacements, Wolf, cardSource: GetCardSource());
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
            }
        }

        public virtual IEnumerator Detransform()
        {
            if (IsHuman) yield break;
            var e = this.GameController.SwitchCards(CharacterCardWithoutReplacements, Human, cardSource: GetCardSource());
            if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
            else this.GameController.ExhaustCoroutine(e);
        }

        protected override IEnumerator RemoveCardsFromGame(IEnumerable<Card> cards)
        {
            if (CharacterCardWithoutReplacements.IsInPlayAndHasGameText) yield break;
            IEnumerator e;
            IEnumerable<Card> other = FindCardsWhere(new LinqCardCriteria(c => c != base.Card && c.SharedIdentifier != null && c.SharedIdentifier == Card.SharedIdentifier));
            foreach (Card c in other) {
                if (c.IsIncapacitated) continue;
                e = this.GameController.FlipCard(FindCardController(c), cardSource: GetCardSource());
                if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                else this.GameController.ExhaustCoroutine(e);
            }
            e = base.RemoveCardsFromGame(cards);
            if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
            else this.GameController.ExhaustCoroutine(e);
        }
    }
}
