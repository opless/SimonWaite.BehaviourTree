using System;
using NUnit.Framework;

namespace SimonWaite.BehaviourTree.Test
{
	[TestFixture]
	public class IntegerAssignmentTests
	{
		public static void Test ()
		{
			Console.WriteLine ("*** INTEGER ASSIGNMENT TESTS ***");
			var x = new IntegerAssignmentTests ();
			x.Assignments ();
		}

		[Test]
		public void Assignments ()
		{
			Random r = new Random ();
			var c = new SimpleContext ();
			for (int i = 0; i < 128; i++) {
				var q = r.Next ();
				if (r.Next () % 2 == 0)
					q = -q;
				var n = new IntegerAssignmentCmd ("A" + i, q);
				n.Tick (c);

				Assert.AreEqual (q, c.GetInteger ("A" + i));
			}
		}
	}
}
