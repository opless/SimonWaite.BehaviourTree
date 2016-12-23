using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace SimonWaite.BehaviourTree.Test
{
	[TestFixture]
	public class IntegerCompareTests
	{
		public static void Test ()
		{
			Console.WriteLine ("*** INTEGER COMPARE TESTS ***");
			var x = new IntegerCompareTests ();

			x.Assignment ();
			x.Equal ();
			x.NotEqual ();
			x.GreaterThan ();
			x.LessThan ();
			x.LessThanOrEqual ();
			x.GreaterThanOrEqual ();
		}



		[Test]
		public void Assignment ()
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
		[Test]
		public void Equal ()
		{
			var x = Rand ();
			var c = CreateCompare (x, x, Comparison.Equal);
			Assert.AreEqual (c.GetInteger ("Result"), 1);
		}
		[Test]
		public void NotEqual ()
		{
			var x = Rand ();
			var c = CreateCompare (x, x + 1, Comparison.NotEqual);
			Assert.AreEqual (c.GetInteger ("Result"), 1);
		}
		[Test]
		public void LessThan ()
		{
			var x = Rand ();
			var c = CreateCompare (x, x + 1, Comparison.LessThan);
			Assert.AreEqual (c.GetInteger ("Result"), 1);
		}
		[Test]
		public void GreaterThan ()
		{
			var x = Rand ();
			var c = CreateCompare (x, x - 1, Comparison.GreaterThan);
			Assert.AreEqual (c.GetInteger ("Result"), 1);
		}
		[Test]
		public void LessThanOrEqual ()
		{
			var x = Rand ();
			var c = CreateCompare (x, x + 1, Comparison.LessThanOrEqual);
			Assert.AreEqual (c.GetInteger ("Result"), 1);
			var v = CreateCompare (x, x + 1, Comparison.LessThanOrEqual);
			Assert.AreEqual (v.GetInteger ("Result"), 1);
		}
		[Test]
		public void GreaterThanOrEqual ()
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
			Node cmp = new IntegerCompareNode ("compare", "VarA", c, "VarB");
			cmp.Children.Add (new IntegerAssignmentCmd ("Result", 1));
			root.Children.Add (cmp);

			Node p = root;
			Console.WriteLine ("BEGIN RUN {0} {1} {2}", a, c, b);
			var ret = p.Tick (ctx);

			Console.WriteLine ("END-RUN: {0} C={1}", ret, ctx.GetInteger ("C"));
			return ctx;
		}


	}
}
