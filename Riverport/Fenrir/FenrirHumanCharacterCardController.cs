using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Fenrir
{
    public class FenrirHumanCharacterCardController : HeroCharacterCardController
    {
        public FenrirHumanCharacterCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {

        }

        public override bool CanBeMovedOutOfGame => true;

        public override IEnumerator UsePower(int index = 0)
        {
            var draw = DrawCard();
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(draw); } 
        }

        public override IEnumerator UseIncapacitatedAbility(int index)
        {
            switch(index)
            {
                case 0:
                    {
                        var heal = this.GameController.SelectAndGainHP(HeroTurnTakerController, 1, false, c => c.IsHeroCharacterCard, 1, 0, cardSource: GetCardSource());
                        if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(heal); } else { this.GameController.ExhaustCoroutine(heal); }
                        break;
                    }
                case 1:
                    {
                        var draw = this.GameController.SelectHeroToDrawCard(HeroTurnTakerController, numberOfCards: 1, cardSource: GetCardSource());
                        if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(draw); } else { this.GameController.ExhaustCoroutine(draw); }
                        break;
                    }
                case 2:
                    {
                        ReduceDamageStatusEffect rdse = new ReduceDamageStatusEffect(1);
                        rdse.TargetCriteria.IsHero = true;
                        rdse.UntilStartOfNextTurn(TurnTaker);
                        var protect = AddStatusEffect(rdse);
                        if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(protect); } else { this.GameController.ExhaustCoroutine(protect); }
                        break;
                    }
            }
        }
    }
}
