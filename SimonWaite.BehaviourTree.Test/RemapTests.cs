using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace SimonWaite.BehaviourTree.Test
{
	[TestFixture]
	public class RemapTests
	{
		public static void Test ()
		{
			Console.WriteLine ("*** REMAP TESTS ***");

			var x = new RemapTests ();
			x.RemapAsError ();
			x.RemapAsFailure ();
			x.RemapAsSuccess ();
			x.RemapAsUnknown ();
			x.RemapAsProcessing ();
		}

		Result RemapAs (Result res)
		{
			IContext ctx = new SimpleContext ();

			var mapping = new SortedDictionary<Result, Result> ();
			mapping [Result.Error] = Result.Failure;
			mapping [Result.Failure] = Result.Failure;
			mapping [Result.Processing] = Result.Failure;
			mapping [Result.Success] = Result.Failure;
			mapping [Result.Unknown] = Result.Failure;

			mapping [res] = Result.Success;

			Node root = new RemapNode ("remap", new AlwaysCmd (res), mapping);

			Console.WriteLine ("BEGIN RUN: {0}", res);
			var ret = root.Tick (ctx);

			Console.WriteLine ("END-RUN..: {0}", ret);
			return ret;
		}

		[Test]
		public void RemapAsError ()
		{
			var x = RemapAs (Result.Error);
			Assert.AreEqual (Result.Success, x);
		}
		[Test]
		public void RemapAsFailure ()
		{
			var x = RemapAs (Result.Failure);
			Assert.AreEqual (Result.Success, x);
		}
		[Test]
		public void RemapAsProcessing ()
		{
			var x = RemapAs (Result.Processing);
			Assert.AreEqual (Result.Success, x);
		}
		[Test]
		public void RemapAsSuccess ()
		{
			var x = RemapAs (Result.Success);
			Assert.AreEqual (Result.Success, x);
		}
		[Test]
		public void RemapAsUnknown ()
		{
			var x = RemapAs (Result.Unknown);
			Assert.AreEqual (Result.Success, x);
		}

	}
}
