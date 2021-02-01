using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riverport.Artificier
{
    public class QuestCompleteCardController : ArtificierBaseCardController
    {
        public QuestCompleteCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            var search = SearchForCards(DecisionMaker, true, false, 1, 1, new LinqCardCriteria(c => IsEquipment(c)), true, false, false, true);
            if (UseUnityCoroutines) { yield return this.GameController.StartCoroutine(search); } else { this.GameController.ExhaustCoroutine(search); }
        }
    }
}
