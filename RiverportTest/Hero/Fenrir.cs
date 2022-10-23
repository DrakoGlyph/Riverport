using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.UnitTest;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiverportTest
{
    [TestFixture()]
    public class Fenrir : BaseTest
    {
        
        protected HeroTurnTakerController fenrir { get { return FindHero("Fenrir"); } }

        [Test()]
        public void TestTransformSwap()
        {
            SetupGameController("BaronBlade", "Riverport.Fenrir", "FreedomTower");
            AssertIsInPlay("FenrirHumanCharacter");
            PlayCard("FenrirTransformingHowl");
            AssertIsInPlay("FenrirWolfCharacter");
            PlayCard("FenrirRefocus");
            AssertIsInPlay("FenrirHumanCharacter");
        }
    }
}
