using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Fenrir
{
    public class FenrirBaseCharacterCardController : HeroCharacterCardController
    {
        public FenrirBaseCharacterCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }


		protected Card Human
		{
			get
			{
				return FindCard("FenrirHumanCharacter");
			}
		}

		protected Card Wolf
		{
			get
			{
				return FindCard("FenrirWolfCharacter");
			}
		}

		protected DamageSource Fenrir
		{
			get
			{
				return new DamageSource(GameController, CharacterCard);
			}
		}

		protected override IEnumerator RemoveCardsFromGame(IEnumerable<Card> cards)
		{
			if (!base.Card.IsInPlayAndHasGameText)
			{
				yield break;
			}
			IEnumerable<Card> enumerable = FindCardsWhere((Card c) => c != base.Card && c.SharedIdentifier != null && c.SharedIdentifier == base.Card.SharedIdentifier);
			foreach (Card item in enumerable)
			{
				if (!item.IsIncapacitated)
				{
					IEnumerator coroutine = base.GameController.FlipCard(FindCardController(item), treatAsPlayed: false, treatAsPutIntoPlay: false, null, null, GetCardSource());
					if (base.UseUnityCoroutines)
					{
						yield return base.GameController.StartCoroutine(coroutine);
					}
					else
					{
						base.GameController.ExhaustCoroutine(coroutine);
					}
				}
			}
			IEnumerator coroutine2 = base.RemoveCardsFromGame(cards);
			if (base.UseUnityCoroutines)
			{
				yield return base.GameController.StartCoroutine(coroutine2);
			}
			else
			{
				base.GameController.ExhaustCoroutine(coroutine2);
			}
		}

		protected IEnumerator Detransform(GameAction arg = null)
		{
			if (!Human.IsInPlay)
			{
				var swap = this.GameController.SwitchCards(CharacterCardWithoutReplacements, Human, true, false, false, GetCardSource());
				if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(swap); } else { this.GameController.ExhaustCoroutine(swap); }
			}

		}

		protected IEnumerator Transform()
		{
			//Switch To Wolf Form
			if (!Wolf.IsInPlay)
			{
				var swap = this.GameController.SwitchCards(CharacterCardWithoutReplacements, Wolf, true, false, false, GetCardSource());
				if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(swap); } else { this.GameController.ExhaustCoroutine(swap); }
			}
		}
	}
}
