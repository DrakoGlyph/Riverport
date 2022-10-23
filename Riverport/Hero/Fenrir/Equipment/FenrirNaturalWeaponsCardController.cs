using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Fenrir
{
    public class FenrirNaturalWeaponsCardController : BaseFenrirCardController
    {
        public FenrirNaturalWeaponsCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController, true)
        {
        }

        

        public override IEnumerator UsePower(int index = 0)
        {
            IEnumerator e;
            switch (index)
            {
                case 0:
                    // One player may search their deck for an Ongoing and put it into play. If no card enters play this way, draw a card
                    
                    List<SelectTurnTakerDecision> selected = null;
                    e = this.GameController.SelectHeroTurnTaker(HeroTurnTakerController, SelectionType.SearchDeck, true, false, selected, cardSource: GetCardSource());
                    if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                    else this.GameController.ExhaustCoroutine(e);
                    if (!DidSelectTurnTaker(selected)) goto draw;
                    HeroTurnTakerController httc = FindHeroTurnTakerController(GetSelectedTurnTaker(selected) as HeroTurnTaker);
                    List<SelectCardDecision> chosen = new List<SelectCardDecision>();
                    e = SearchForCards(httc, true, false, null, 1, new LinqCardCriteria(c => c.IsOngoing, "Ongoing"), true, false, false, true, chosen);
                    if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                    else this.GameController.ExhaustCoroutine(e);
                    if (DidSelectCard(chosen)) break;
                draw:
                    e = DrawCard(optional: true);
                    if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                    else this.GameController.ExhaustCoroutine(e);
                    break;
                case 1:
                    // Fenrir deals 1 target 3 Melee damage
                    e = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, Fenrir, 3, DamageType.Melee, 1, false, 1, cardSource: GetCardSource());
                    if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                    else this.GameController.ExhaustCoroutine(e);
                    break;
            }
        }
    }
}
