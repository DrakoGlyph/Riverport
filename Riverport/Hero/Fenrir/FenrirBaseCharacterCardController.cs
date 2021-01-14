using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Fenrir
{
    class FenrirBaseCharacterCardController : HeroCharacterCardController
    {
        public FenrirBaseCharacterCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
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
	}
}
