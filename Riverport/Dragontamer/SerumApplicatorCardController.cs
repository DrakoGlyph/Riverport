using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.Dragontamer
{
    public class SerumApplicatorCardController : CardController
    {
        public SerumApplicatorCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddIncreaseDamageTrigger(dda => dda.DamageSource.IsSameCard(CharacterCard) && dda.DamageType == DamageType.Projectile, 1);
        }

        public override IEnumerator UsePower(int index = 0)
        {
            List<DealDamageAction> hit = new List<DealDamageAction>();
            var shoot = DealDamage(CharacterCard, c => c.IsVillainTarget, 2, DamageType.Projectile, false, false, hit);
            if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(shoot); } else { this.GameController.ExhaustCoroutine(shoot); }
            DealDamageAction dda = hit.FirstOrDefault();
            List<SelectCardDecision> chosen = new List<SelectCardDecision>();
            var select = this.GameController.SelectCardAndStoreResults(HeroTurnTakerController, SelectionType.CardToDealDamage, new LinqCardCriteria(c => c.DoKeywordsContain("dragon")), chosen, true, cardSource: GetCardSource());
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(select); } else { this.GameController.ExhaustCoroutine(select); }
            if(DidSelectCard(chosen))
            {
                Card dragon = GetSelectedCard(chosen);
                var sic = DealDamage(dragon, dda.Target, 1, DamageType.Melee, cardSource: GetCardSource());
                if(UseUnityCoroutines) { yield return this.GameController.StartCoroutine(sic); } else { this.GameController.ExhaustCoroutine(sic); }
            }
        }
    }
}
