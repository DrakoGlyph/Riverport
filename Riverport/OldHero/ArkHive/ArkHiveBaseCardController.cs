using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace Riverport.ArkHive
{
    public abstract class ArkHiveBaseCardController : CardController
    {
        protected ArkHiveBaseCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }


        protected string ListArkhiveAndTheSpares()
        {
            string rtn = "There are Spare Nanobots next to ";
            foreach(Card card in ArkHiveAndTheSparesCards)
            {
                if (card == CharacterCard) continue;
                rtn += card.Title + " ";
            }
            return rtn + ".";
        }

        protected int SpareNanobots
        {
            get
            {
                return Nanobots.Count();
            }
        }

        protected List<Card> ArkHiveAndTheSparesCards
        {
            get
            {
                List<Card> rtn = new List<Card>();
                rtn.Add(CharacterCard);
                foreach(Card c in Nanobots)
                {
                    CardController cc = FindCardController(c);
                    if(cc is SpareNanobotCardController sncc)
                    {
                        Card eq = sncc.Equipped;
                        if (eq != null) rtn.Add(eq);
                    }
                }

                return rtn;
            }
        }

        protected IEnumerable<Card> Nanobots
        {
            get
            {
                return FindCardsWhere(c => c.IsInPlayAndHasGameText && c.DoKeywordsContain("nanobot"));
            }
        }

        protected List<HeroTurnTaker> ArkHiveAndTheSparesTurnTakers
        {
            get
            {
                List<HeroTurnTaker> rtn = new List<HeroTurnTaker>();
                rtn.Add(HeroTurnTaker);
                foreach (Card c in Nanobots)
                {
                    CardController cc = FindCardController(c);
                    if(cc is SpareNanobotCardController sncc)
                    {
                        Card eq = sncc.Equipped;
                        if (eq != null && eq.Owner is HeroTurnTaker hero) rtn.Add(hero);
                    }
                }

                return rtn;
            }
        }


        protected bool FilterTurnTakerByArchiveAndSpares(HeroTurnTaker arg)
        {
            return ArkHiveAndTheSparesTurnTakers.Contains(arg);
        }

        protected LinqCardCriteria PlanFilter
        {
            get
            {
                return new LinqCardCriteria(c => c.DoKeywordsContain("plan"), "Plans", false, false, "Plan", "Plans");
            }
        }
    }
}
