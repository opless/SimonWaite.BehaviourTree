using NUnit.Framework;
using System;
using System.Collections.Generic;


namespace SimonWaite.BehaviourTree.Test
{
	[TestFixture]
	public class AlwaysTests
	{
		public static void Test ()
		{
			Console.WriteLine ("*** ALWAYS TESTS ***");

			var x = new AlwaysTests ();

			x.AlwaysAsError ();
			x.AlwaysAsFailure ();
			x.AlwaysAsProcessing ();
			x.AlwaysAsSuccess ();
			x.AlwaysAsUnknown ();
		}
		Result AlwaysAs (Result res)
		{
			IContext ctx = new SimpleContext ();

			Node root = new SelectorNode ("Root", null, false);
			root.Children.Add (new AlwaysCmd (res));

			Console.WriteLine ("BEGIN RUN: {0}", res);
			var ret = root.Tick (ctx);

			Console.WriteLine ("END-RUN..: {0}", ret);
			return ret;
		}

		[Test]
		public void AlwaysAsError ()
		{
			var x = AlwaysAs (Result.Error);
			Assert.AreEqual (Result.Error, x);
		}
		[Test]
		public void AlwaysAsFailure ()
		{
			var x = AlwaysAs (Result.Failure);
			Assert.AreEqual (Result.Failure, x);
		}
		[Test]
		public void AlwaysAsProcessing ()
		{
			var x = AlwaysAs (Result.Processing);
			Assert.AreEqual (Result.Processing, x);
		}
		[Test]
		public void AlwaysAsSuccess ()
		{
			var x = AlwaysAs (Result.Success);
			Assert.AreEqual (Result.Success, x);
		}
		[Test]
		public void AlwaysAsUnknown ()
		{
			var x = AlwaysAs (Result.Unknown);
			Assert.AreEqual (Result.Unknown, x);
		}

	}
}
