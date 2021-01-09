using System;
using NUnit.Framework;
using System.Reflection;
using Handelabra.Sentinels.Engine.Model;
using Handelabra;

namespace RiverportTest
{
    [SetUpFixture]
    public class Setup
    {
        [OneTimeSetUp]
        public void DoSetup()
        {
            Log.DebugDelegate += Output;
            Log.WarningDelegate += Output;
            Log.ErrorDelegate += Output;

            var Riverport = Assembly.GetAssembly(typeof(Riverport.Dragontamer.DragontamerTurnTakerController));
            ModHelper.AddAssembly("Riverport", Riverport);

            // Tell the engine about our mod assembly so it can load up our code.
            // It doesn't matter which type as long as it comes from the mod's assembly.
            
        }

        protected void Output(string message)
        {
            Console.WriteLine(message);
        }
    }
}
