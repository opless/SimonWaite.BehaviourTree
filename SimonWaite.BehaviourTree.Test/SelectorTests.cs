using NUnit.Framework;
using System;
using System.Collections.Generic;


namespace SimonWaite.BehaviourTree.Test
{
	[TestFixture]
	public class SelectorTests
	{
		public static void Test ()
		{
			Console.WriteLine ("*** SELECTOR TESTS ***");
			var x = new SelectorTests ();
			x.Success ();
			x.Failure ();
			x.Error ();
			x.Processing ();
			x.Unknown ();
		}

		Result CreateSelector (params Result [] args)
		{
			IContext ctx = new SimpleContext ();

			Node root = new SequenceNode ("Root", null, false);
			foreach (var arg in args) {
				root.Children.Add (new AlwaysCmd (arg));
			}
			Node p = root;
			Console.Write ("BEGIN RUN [");
			foreach (var arg in args) {
				Console.Write ("{0}, ", arg);
			}
			Console.WriteLine ("]");
			var ret = p.Tick (ctx);

			Console.WriteLine ("END-RUN: {0}", ret);
			return ret;
		}

		public void SelectorTestModel (Result r)
		{
			var x = CreateSelector (r);
			Assert.AreEqual (r, x);

			x = CreateSelector (Result.Success, r);
			Assert.AreEqual (r, x);

			x = CreateSelector (Result.Success, Result.Success, r);
			Assert.AreEqual (r, x);

			x = CreateSelector (Result.Success, r, Result.Success);
			Assert.AreEqual (r, x);

		}

		[Test]
		public void Success ()
		{
			SelectorTestModel (Result.Success);
		}

		[Test]
		public void Failure ()
		{
			SelectorTestModel (Result.Failure);
		}
		[Test]
		public void Error ()
		{
			SelectorTestModel (Result.Error);
		}
		[Test]
		public void Processing ()
		{
			SelectorTestModel (Result.Processing);
		}
		[Test]
		public void Unknown ()
		{
			SelectorTestModel (Result.Unknown);
		}
	}
}
