using NUnit.Framework;
using System;
using System.Collections.Generic;


namespace SimonWaite.BehaviourTree.Test
{
	[TestFixture]
	public class SequenceTests
	{
		public static void Test ()
		{
			Console.WriteLine ("*** SEQUENCE TESTS ***");
			var x = new SequenceTests ();
			x.Success ();
			x.Failure ();
			x.Error ();
			x.Processing ();
			x.Unknown ();
		}

		Result CreateSequence (params Result [] args)
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

		public void SequenceTestModel (Result r)
		{
			var x = CreateSequence (r);
			Assert.AreEqual (r, x);

			x = CreateSequence (Result.Success, r);
			Assert.AreEqual (r, x);

			x = CreateSequence (Result.Success, Result.Success, r);
			Assert.AreEqual (r, x);

			x = CreateSequence (Result.Success, r, Result.Success);
			Assert.AreEqual (r, x);

		}

		[Test]
		public void Success ()
		{
			SequenceTestModel (Result.Success);
		}

		[Test]
		public void Failure ()
		{
			SequenceTestModel (Result.Failure);
		}
		[Test]
		public void Error ()
		{
			SequenceTestModel (Result.Error);
		}
		[Test]
		public void Processing ()
		{
			SequenceTestModel (Result.Processing);
		}
		[Test]
		public void Unknown ()
		{
			SequenceTestModel (Result.Unknown);
		}
	}
}
