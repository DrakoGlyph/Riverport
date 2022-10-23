using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Fenrir
{
    public class FenrirHumanCharacterCardController : BaseFenrirCharacterCardController
    {

        public FenrirHumanCharacterCardController(Card card, TurnTakerController ttc) : base(card, ttc)
        {
            
        }

        public override IEnumerator UsePower(int index = 0)
        {
            switch (index)
            {
                case 0:
                    /*
                     * One Hero may draw a card now. 
                     * If No cards are drawn this way, Fenrir may transform.
                     */
                    List<SelectTurnTakerDecision> who = new List<SelectTurnTakerDecision>();
                    var e = this.GameController.SelectHeroTurnTaker(HeroTurnTakerController, SelectionType.DrawCard, true, false, who, cardSource: GetCardSource());
                    if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                    else this.GameController.ExhaustCoroutine(e);

                    TurnTaker selected = GetSelectedTurnTaker(who);
                    if (selected == null) goto trans;
                    List<DrawCardAction> drawn = new List<DrawCardAction>();
                    e = this.GameController.DrawCard(selected as HeroTurnTaker, true, drawn, cardSource: GetCardSource());
                    if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                    else this.GameController.ExhaustCoroutine(e);
                    if (GetNumberOfCardsDrawn(drawn) > 0) break;
                    trans:
                    List<YesNoCardDecision> doIt = new List<YesNoCardDecision>();
                    e = this.GameController.MakeYesNoCardDecision(HeroTurnTakerController, SelectionType.SwitchToHero, Card, storedResults: doIt, cardSource: GetCardSource());
                    if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                    else this.GameController.ExhaustCoroutine(e);
                    break;
            }
        }

        public override IEnumerator UseIncapacitatedAbility(int index)
        {
            IEnumerator e;
            switch(index)
            {
                case 0:
                    /*
                     * One player may draw a card now
                     */
                    e = this.GameController.SelectHeroToDrawCard(HeroTurnTakerController, true, cardSource: GetCardSource());
                    if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                    else this.GameController.ExhaustCoroutine(e);
                    break;
                case 1:
                    /*
                     * One Hero may deal 1 target 2 Psychic damage
                     */
                    e = this.GameController.SelectHeroToSelectTargetAndDealDamage(HeroTurnTakerController, 2, DamageType.Psychic, cardSource: GetCardSource());
                    if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                    else this.GameController.ExhaustCoroutine(e);
                    break;
                case 2:
                    /* 
                     * One Hero deals themself 1 Psychic damage, then that Hero may use a power
                     */
                    List<SelectTurnTakerDecision> who = new List<SelectTurnTakerDecision>();
                    e = this.GameController.SelectHeroTurnTaker(HeroTurnTakerController, SelectionType.DealDamageSelf, false, false, who, cardSource: GetCardSource());
                    if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                    else this.GameController.ExhaustCoroutine(e);
                    TurnTaker hero = GetSelectedTurnTaker(who);
                    if (hero == null) break;
                    e = DealDamage(hero.CharacterCard, hero.CharacterCard, 1, DamageType.Psychic, cardSource: GetCardSource());
                    if (UseUnityCoroutines) yield return this.GameController.StartCoroutine(e);
                    else this.GameController.ExhaustCoroutine(e);
                    break;
            }
        }

    }
}
