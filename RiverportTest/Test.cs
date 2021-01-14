﻿using NUnit.Framework;
using System;
using Riverport;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Linq;
using System.Collections;
using Handelabra.Sentinels.UnitTest;

namespace RiverportTest
{
    [TestFixture()]
    public class Test : BaseTest
    {
        //protected TurnTakerController baddies { get { return FindVillain("TheBaddies"); } }
        //protected TurnTakerController hug { get { return FindVillain("TheHugMonsterTeam"); } }
        //protected HeroTurnTakerController migrant { get { return FindHero("MigrantCoder"); } }
        protected HeroTurnTakerController dragontamer { get { return FindHero("Dragontamer"); } }
        protected HeroTurnTakerController fenrir { get { return FindHero("Fenrir"); } }
        protected HeroTurnTakerController weaver { get { return FindHero("Weaver"); } }
        protected HeroTurnTakerController scarletX { get { return FindHero("ScarletX"); } }
        [Test()]
        public void TestModWorks()
        {
            //SetupGameController("Workshopping.TheBaddies", "Workshopping.MigrantCoder", "Workshopping.DevStream");
            //Assert.AreEqual(3, this.GameController.TurnTakerControllers.Count());
            SetupGameController("BaronBlade", "Riverport.Weaver", "Riverport.Dragontamer", "Riverport.Fenrir", "FreedomTower");

            //Assert.IsNotNull(baddies);
            //Assert.IsInstanceOf(typeof(TheBaddiesTurnTakerController), baddies);
            //Assert.IsInstanceOf(typeof(TheBaddiesCharacterCardController), baddies.CharacterCardController);

            //Assert.IsNotNull(migrant);
            //Assert.IsInstanceOf(typeof(MigrantCoderTurnTakerController), migrant);
            //Assert.IsInstanceOf(typeof(MigrantCoderCharacterCardController), migrant.CharacterCardController);
            Assert.IsNotNull(dragontamer);
            Assert.IsNotNull(weaver);
            Assert.IsNotNull(fenrir);
            Assert.IsNotNull(env);

            //Assert.AreEqual(40, baddies.CharacterCard.HitPoints);
            //Assert.AreEqual(39, migrant.CharacterCard.HitPoints);
            //QuickHPStorage(baddies, migrant);

            // Always deals 5 psychic!
            //PlayTopCard(baddies);


            //    QuickHPCheck(-5, -6); // Nemesis!
            //    PlayTopCard(env);
            //    // Deals 1 damage
            //    QuickHPCheck(-1, -1);

            //    // Heals 1 at the start of the environment turn
            //    GoToStartOfTurn(env);
            //    QuickHPCheck(1, 1);
        }

        [Test()]
        public void TestLycanFormSwap()
        {
            SetupGameController("BaronBlade", "Riverport.Fenrir", "FreedomTower");
            AssertIsInPlay("FenrirHumanCharacter");
            PlayCard("LycanForm");
            AssertIsInPlay("FenrirWolfCharacter");
            DestroyCard("LycanForm");
            AssertIsInPlay("FenrirHumanCharacter");
        }

        

        [Test()]
        public void TestPaleMoonPendantAbilities()
        {
            SetupGameController("BaronBlade", "Riverport.Fenrir", "FreedomTower");

            QuickHPStorage(fenrir);
            PlayCard("SlashAndBurn");
            QuickHPCheck(-4);
            PlayCard("PaleMoonPendant");
            QuickHPStorage(fenrir);
            GoToEndOfTurn(fenrir);
            GoToNextTurn();
            QuickHPCheck(1);
            PlayCard("LycanForm");
            QuickHPStorage(fenrir);
            PlayCard("SlashAndBurn");
            QuickHPCheck(-2);
        }

        //[Test()]
        //public void TestPunchingBag()
        //{
        //    SetupGameController("BaronBlade", "Workshopping.MigrantCoder", "Megalopolis");

        //    StartGame();

        //    GoToUsePowerPhase(migrant);

        //    // Punching Bag does 1 damage!
        //    QuickHPStorage(migrant);
        //    PlayCard("PunchingBag");
        //    QuickHPCheck(-1);
        //}

        //[Test()]
        //public void TestInnatePower()
        //{
        //    SetupGameController("BaronBlade", "Workshopping.MigrantCoder", "Megalopolis");

        //    StartGame();

        //    var mdp = GetCardInPlay("MobileDefensePlatform");

        //    // Base power draws 3 cards! Deals 1 target 2 damage!
        //    QuickHandStorage(migrant.ToHero());
        //    DecisionSelectTarget = mdp;
        //    QuickHPStorage(mdp);

        //    UsePower(migrant.CharacterCard);

        //    QuickHandCheck(3);
        //    QuickHPCheck(-2);

        //}

        //[Test()]
        //public void TestHugMonster()
        //{
        //    SetupGameController("Workshopping.TheHugMonsterTeam", "Workshopping.MigrantCoder", "BugbearTeam", "Legacy", "GreazerTeam", "TheWraith", "Megalopolis");

        //    StartGame();

        //    var warm = PlayTopCard(hug);

        //    QuickHPStorage(hug, migrant, bugbearTeam, legacy, greazerTeam, wraith);
        //    GoToEndOfTurn(hug);
        //    QuickHPCheck(0, -2, 0, -2, 0, -4);

        //    GoToPlayCardPhase(migrant);
        //    DestroyCard(warm);
        //    GoToEndOfTurn(migrant);
        //    QuickHPCheckZero();

        //}

        //[Test()]
        //public void TestMigrantCoderLockdown()
        //{
        //    SetupGameController("BaronBlade", "Workshopping.MigrantCoder/MigrantCoderLockdown", "Megalopolis");

        //    StartGame();

        //    GoToUsePowerPhase(migrant);

        //    // Use power to reduce damage by 1
        //    QuickHPStorage(migrant);
        //    UsePower(migrant);
        //    PlayCard("PunchingBag");
        //    QuickHPCheck(0);
        //}

        //[Test()]
        //public void TestBunkerVariant()
        //{
        //    SetupGameController("BaronBlade", "Bunker/Workshopping.WaywardBunkerCharacter", "Megalopolis");

        //    StartGame();

        //    Assert.IsTrue(bunker.CharacterCard.IsPromoCard);
        //    Assert.AreEqual("WaywardBunkerCharacter", bunker.CharacterCard.PromoIdentifierOrIdentifier);
        //    Assert.AreEqual(30, bunker.CharacterCard.MaximumHitPoints);

        //    GoToUsePowerPhase(bunker);

        //    // Use the power, it draws 2 cards not 1!
        //    QuickHandStorage(bunker);
        //    UsePower(bunker);
        //    QuickHandCheck(2);
        //}
    }
}
