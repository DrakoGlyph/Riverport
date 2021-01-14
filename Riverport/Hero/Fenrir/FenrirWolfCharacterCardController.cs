using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Collections;

namespace Riverport.Fenrir
{
    public class FenrirWolfCharacterCardController : HeroCharacterCardController
    {
        public FenrirWolfCharacterCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            
        }

        public override void AddTriggers()
        {
            base.AddTriggers();

            
        }

        public override bool CanBeMovedOutOfGame => true;
        

        public override IEnumerator UsePower(int index = 0)
        {
            var howl = this.GameController.SelectTargetsAndDealDamage(HeroTurnTakerController, new DamageSource(GameController, Card), 1, DamageType.Sonic, 3, false, 0, false, false, false, IsVillainTarget, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(howl); } else { this.GameController.ExhaustCoroutine(howl); }
        }

        public override IEnumerator UseIncapacitatedAbility(int index)
        {
            switch (index)
            {
                case 0:
                    {
                        var rend = this.GameController.SelectAndDestroyCard(HeroTurnTakerController, new LinqCardCriteria(c => c.DoKeywordsContain("device") || c.DoKeywordsContain("ongoing")), true, cardSource: GetCardSource());
                        if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(rend); } else { this.GameController.ExhaustCoroutine(rend); }
                        break;
                    }
                case 1:
                    {
                        IncreaseDamageStatusEffect idse = new IncreaseDamageStatusEffect(1);
                        idse.SourceCriteria.IsHero = true;
                        idse.UntilStartOfNextTurn(TurnTaker);
                        var inspire = AddStatusEffect(idse);
                        if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(inspire); } else { this.GameController.ExhaustCoroutine(inspire); }
                        break;
                    }
                case 2:
                    {
                        var draw = this.GameController.SelectHeroToDrawCard(HeroTurnTakerController, numberOfCards: 1, cardSource: GetCardSource());
                        if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(draw); } else { this.GameController.ExhaustCoroutine(draw); }
                        break;
                    }
            }
        }
    }
}
