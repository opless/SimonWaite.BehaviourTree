using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SimonWaite.BehaviourTree.Test
{
	[TestFixture]

	public class RepeaterTests
	{
		public static void Test ()
		{
			Console.WriteLine ("*** REPEATER TESTS ***");

			var x = new RepeaterTests ();
			x.Repeat ();
			x.Until ();
		}
		[Test]
		public void Repeat ()
		{
			var ctx = new SimpleContext ();
			var init = 0;//DateTime.UtcNow.Ticks % 0xFFFFFF;
			for (int i = 1; i < 128; i++) {
				ctx.SetInteger ("A" + i, init);
				var chg = new IntegerChangeCmd ("A" + i, 1);
				var rep = new RepeaterNode ("Repeat", chg, i);
				while (Result.Processing == rep.Tick (ctx)) { }

				Assert.AreEqual (init + i, ctx.GetInteger ("A" + i));
			}
		}
		[Test]
		public void Until ()
		{
			var ctx = new SimpleContext ();
			var items = new Result [] { Result.Error, Result.Failure, Result.Processing, Result.Success, Result.Unknown };

			foreach (var i in items) {
				var res = new HashSet<Result> ();
				res.Add (i);
				var rep = new RepeaterNode ("Repeat", new AlwaysCmd (i), 10, res);
				var x = rep.Tick (ctx);
				while (Result.Processing == x) { x = rep.Tick (ctx); }
				Assert.AreEqual (Result.Success, x);
			}
		}
	}
}
