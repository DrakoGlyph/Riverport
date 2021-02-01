using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Artificier
{
    class ArtificierCharacterCardController : ArtificierBaseCharacterCardController
    {
        public ArtificierCharacterCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator UsePower(int index = 0)
        {
            List<Card> top = new List<Card>();
            var reveal = this.GameController.RevealCards(DecisionMaker, TurnTaker.Deck, 1, top, false, cardSource: GetCardSource());
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(reveal); } else { this.GameController.ExhaustCoroutine(reveal); }

            Card revealed = top.FirstOrDefault();

            if (revealed != null)
            {
                Location moveTo = HeroTurnTaker.Hand;
                if(revealed.DoKeywordsContain("relic"))
                {
                    List<YesNoCardDecision> decisions = new List<YesNoCardDecision>();
                    var decide = this.GameController.MakeYesNoCardDecision(DecisionMaker, SelectionType.PlayCard, revealed, null, decisions, null, GetCardSource());
                    if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(decide); } else { this.GameController.ExhaustCoroutine(decide); }
                    if (DidPlayerAnswerYes(decisions)) moveTo = TurnTaker.PlayArea;
                }
                var move = this.GameController.MoveCard(TurnTakerController, revealed, moveTo, false, showMessage: true, cardSource: GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(move); } else { this.GameController.ExhaustCoroutine(move); }
            }
        }

        public override IEnumerator UseIncapacitatedAbility(int index)
        {
            switch(index)
            {
                case 0:
                    {
                        List<DealDamageAction> damages = new List<DealDamageAction>();
                        List<SelectCardDecision> storedResults = new List<SelectCardDecision>();
                        var shoot = this.GameController.SelectHeroCharacterCard(DecisionMaker, SelectionType.DealDamage, storedResults, false, false, GetCardSource());
                        if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(shoot); } else { this.GameController.ExhaustCoroutine(shoot); }
                        Card who = GetSelectedCard(storedResults);
                        Func<DealDamageAction, IEnumerator> Leech = (DealDamageAction dda) => this.GameController.GainHP(who, dda.Amount, cardSource: GetCardSource());
                        shoot = this.GameController.SelectTargetsAndDealDamage(this.GameController.FindHeroTurnTakerController(who.Owner as HeroTurnTaker), new DamageSource(GameController, who), 1, DamageType.Energy, 1, false, 0, addStatusEffect: Leech, cardSource: GetCardSource());
                        if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(shoot); } else { this.GameController.ExhaustCoroutine(shoot); }
                        break;
                    }
                case 1:
                    {
                        List<SelectTurnTakerDecision> decision = new List<SelectTurnTakerDecision>();
                        List<DrawCardAction> drawn = new List<DrawCardAction>();
                        var hero = this.GameController.SelectHeroTurnTaker(DecisionMaker, SelectionType.DrawCard, false, false, decision, cardSource: GetCardSource());
                        if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(hero); } else { this.GameController.ExhaustCoroutine(hero); }
                        if (DidSelectTurnTaker(decision)) {
                            HeroTurnTaker htt = (HeroTurnTaker)GetSelectedTurnTaker(decision);
                            var draw = DrawCard(htt, false, drawn);
                            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(draw); } else { this.GameController.ExhaustCoroutine(draw); }
                            if(DidDrawCards(drawn))
                            {
                                foreach(DrawCardAction dda in drawn)
                                {
                                    Card c = dda.DrawnCard;
                                    if(dda.DidDrawCard && c.DoKeywordsContain("equipment"))
                                    {
                                        var play = this.GameController.PlayCard(FindHeroTurnTakerController(htt), c, true, optional: true, associateCardSource: true, cardSource: GetCardSource());
                                        if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(play); } else { this.GameController.ExhaustCoroutine(play); }
                                    }
                                }
                            }
                        }
                            
                        break;
                    }
                case 2:
                    {
                        List<SelectCardDecision> target = new List<SelectCardDecision>();
                        var select = this.GameController.SelectCardAndStoreResults(DecisionMaker, SelectionType.IncreaseDamage, new LinqCardCriteria(c => c.IsVillainTarget && c.IsInPlayAndHasGameText), target, false, cardSource: GetCardSource());
                        if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(select); } else { this.GameController.ExhaustCoroutine(select); }
                        if (DidSelectCard(target))
                        {
                            IncreaseDamageStatusEffect idse = new IncreaseDamageStatusEffect(1);
                            idse.UntilStartOfNextTurn(TurnTaker);
                            idse.TargetCriteria.IsSpecificCard = GetSelectedCard(target);
                            var effect = AddStatusEffect(idse);
                            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(effect); } else { this.GameController.ExhaustCoroutine(effect);}
                            
                        }
                        break;
                    }
            }
        }
    }
}
