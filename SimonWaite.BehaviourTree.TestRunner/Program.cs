using System;
using SimonWaite.BehaviourTree.Test;

namespace SimonWaite.BehaviourTree.TestRunner
{
	class MainClass
	{
		public static void Main (string [] args)
		{
			Console.WriteLine ("*** COMPOSITE TESTS ***");


			SequenceTests.Test ();
			SelectorTests.Test ();
			ParallelTests.Test ();

			Console.WriteLine ("*** DECORATOR TESTS ***");

			IntegerCompareTests.Test ();
			RemapTests.Test ();
			RepeaterTests.Test ();

			Console.WriteLine ("*** LEAF TESTS ***");
			AlwaysTests.Test ();
			IntegerAssignmentTests.Test ();
			IntegerChangeTests.Test ();


			SerialisationRoundTripTests.Test ();
			Console.WriteLine ("Done.");
		}
	}
}
