using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace SimonWaite.BehaviourTree.Test
{
	[TestFixture ()]
	public class Test
	{
		[Test ()]
		public void SeqenceIntegerAssignment ()
		{
			IContext ctx = new SimpleContext ();

			Node root = new SequenceNode ("Root", null, false);
			root.Children.Add (new IntegerAssignmentCmd ("VarA", 1234));
			root.Children.Add (new IntegerAssignmentCmd ("VarB", 4321));


			Node p = root;
			var ret = p.Tick (ctx);

			Console.WriteLine ("END-RUN: {0} VarA={1}, VarB={2}",
					   ret,
					   ctx.GetInteger ("VarA"),
					   ctx.GetInteger ("VarB"));

			Assert.AreEqual (ctx.GetInteger ("VarA"), 1234);
			Assert.AreEqual (ctx.GetInteger ("VarB"), 4321);
		}
		[Test ()]
		public void SeqenceIntegerCompareEqual ()
		{
			var x = Rand ();
			var c = CreateCompare (x, x, Comparison.Equal);
			Assert.AreEqual (c.GetInteger ("Result"), 1);
		}
		[Test ()]
		public void SeqenceIntegerCompareNotEqual ()
		{
			var x = Rand ();
			var c = CreateCompare (x, x + 1, Comparison.NotEqual);
			Assert.AreEqual (c.GetInteger ("Result"), 1);
		}
		[Test ()]
		public void SeqenceIntegerCompareLT ()
		{
			var x = Rand ();
			var c = CreateCompare (x, x + 1, Comparison.LessThan);
			Assert.AreEqual (c.GetInteger ("Result"), 1);
		}
		[Test ()]
		public void SeqenceIntegerCompareGT ()
		{
			var x = Rand ();
			var c = CreateCompare (x, x - 1, Comparison.GreaterThan);
			Assert.AreEqual (c.GetInteger ("Result"), 1);
		}
		[Test ()]
		public void SeqenceIntegerCompareLTE ()
		{
			var x = Rand ();
			var c = CreateCompare (x, x + 1, Comparison.LessThanOrEqual);
			Assert.AreEqual (c.GetInteger ("Result"), 1);
			var v = CreateCompare (x, x + 1, Comparison.LessThanOrEqual);
			Assert.AreEqual (v.GetInteger ("Result"), 1);
		}
		[Test ()]
		public void SeqenceIntegerCompareGTE ()
		{
			var x = Rand ();
			var c = CreateCompare (x, x - 1, Comparison.GreaterThanOrEqual);
			Assert.AreEqual (c.GetInteger ("Result"), 1);
			var v = CreateCompare (x, x, Comparison.GreaterThanOrEqual);
			Assert.AreEqual (v.GetInteger ("Result"), 1);
		}
		static Random r = new Random ();
		static long Rand ()
		{
			return r.Next ();
		}

		IContext CreateCompare (long a, long b, Comparison c)
		{
			IContext ctx = new SimpleContext ();

			Node root = new SequenceNode ("Root", null, false);
			root.Children.Add (new IntegerAssignmentCmd ("VarA", a));
			root.Children.Add (new IntegerAssignmentCmd ("VarB", b));
			Node cmp = new IntegerCompareNode ("VarA", c, "VarB");
			cmp.Children.Add (new IntegerAssignmentCmd ("Result", 1));
			root.Children.Add (cmp);

			Node p = root;
			Console.WriteLine ("BEGIN RUN {0} {1} {2}", a, c, b);
			var ret = p.Tick (ctx);

			Console.WriteLine ("END-RUN: {0} C={1}", ret, ctx.GetInteger ("C"));
			return ctx;
		}

		Result AlwaysTestAs (Result res)
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
		public void AlwaysTestAsError ()
		{
			var x = AlwaysTestAs (Result.Error);
			Assert.AreEqual (Result.Error, x);
		}
		[Test]
		public void AlwaysTestAsFailure ()
		{
			var x = AlwaysTestAs (Result.Failure);
			Assert.AreEqual (Result.Failure, x);
		}
		[Test]
		public void AlwaysTestAsProcessing ()
		{
			var x = AlwaysTestAs (Result.Processing);
			Assert.AreEqual (Result.Processing, x);
		}
		[Test]
		public void AlwaysTestAsSuccess ()
		{
			var x = AlwaysTestAs (Result.Success);
			Assert.AreEqual (Result.Success, x);
		}
		[Test]
		public void AlwaysTestAsUnknown ()
		{
			var x = AlwaysTestAs (Result.Unknown);
			Assert.AreEqual (Result.Unknown, x);
		}


		Result RemapAs (Result res)
		{
			IContext ctx = new SimpleContext ();

			Dictionary<Result, Result> mapping = new Dictionary<Result, Result> ();
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
		public void RemapTestAsError ()
		{
			var x = RemapAs (Result.Error);
			Assert.AreEqual (Result.Success, x);
		}
		[Test]
		public void RemapTestAsFailure ()
		{
			var x = RemapAs (Result.Failure);
			Assert.AreEqual (Result.Success, x);
		}
		[Test]
		public void RemapTestAsProcessing ()
		{
			var x = RemapAs (Result.Processing);
			Assert.AreEqual (Result.Success, x);
		}
		[Test]
		public void RemapTestAsSuccess ()
		{
			var x = RemapAs (Result.Success);
			Assert.AreEqual (Result.Success, x);
		}
		[Test]
		public void RemapTestAsUnknown ()
		{
			var x = RemapAs (Result.Unknown);
			Assert.AreEqual (Result.Success, x);
		}
	}
}
