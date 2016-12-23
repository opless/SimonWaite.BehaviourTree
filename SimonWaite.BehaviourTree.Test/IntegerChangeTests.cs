using System;
using NUnit.Framework;

namespace SimonWaite.BehaviourTree.Test
{
	[TestFixture]
	public class IntegerChangeTests
	{
		public static void Test ()
		{
			Console.WriteLine ("*** INTEGER CHANGE TESTS ***");
			var x = new IntegerChangeTests ();
			x.Assignments ();
		}

		[Test]
		public void Assignments ()
		{
			Random r = new Random ();
			var c = new SimpleContext ();

			for (int i = 0; i < 128; i++) {
				var w = r.Next () % 1000;
				c.SetInteger ("A" + i, w);
				var q = r.Next () % 1000;
				if (r.Next () % 2 == 0)
					q = -q;
				var n = new IntegerChangeCmd ("A" + i, q);
				n.Tick (c);

				Assert.AreEqual (w + q, c.GetInteger ("A" + i));
			}
		}
	}
}
