using System;

namespace SimonWaite.BehaviourTree.TestRunner
{
	class MainClass
	{
		public static void Main (string [] args)
		{
			Console.WriteLine ("Hello World!");
			var x = new SimonWaite.BehaviourTree.Test.Test ();
			x.SeqenceIntegerAssignment ();
			x.SeqenceIntegerCompareEqual ();
			x.SeqenceIntegerCompareNotEqual ();
			x.SeqenceIntegerCompareGT ();
			x.SeqenceIntegerCompareLT ();
			x.SeqenceIntegerCompareLTE ();
			x.SeqenceIntegerCompareGTE ();

			Console.WriteLine ("\n-----\n");

			x.AlwaysTestAsError ();
			x.AlwaysTestAsFailure ();
			x.AlwaysTestAsProcessing ();
			x.AlwaysTestAsSuccess ();
			x.AlwaysTestAsUnknown ();

			Console.WriteLine ("\n-----\n");
			x.RemapTestAsError ();
			x.RemapTestAsFailure ();
			x.RemapTestAsSuccess ();
			x.RemapTestAsUnknown ();
			x.RemapTestAsProcessing ();

			Console.WriteLine ("Done.");
		}
	}
}
