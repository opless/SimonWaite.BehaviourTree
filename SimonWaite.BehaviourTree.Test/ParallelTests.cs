using NUnit.Framework;
using System;
using System.Collections.Generic;


namespace SimonWaite.BehaviourTree.Test
{
	[TestFixture]
	public class ParallelTests
	{
		public static void Test ()
		{
			Console.WriteLine ("*** PARALLEL TESTS ***");
			var x = new ParallelTests ();
			x.Success ();
			x.Fail ();
			x.Error ();
			x.Processing ();
			x.Unknown ();

		}
		Random r = new Random (12345);

		T RandomEnum<T> (Random rand)
		{
			Type type = typeof (T);
			Array values = Enum.GetValues (type);
			object value = values.GetValue (rand.Next (values.Length));
			return (T)Convert.ChangeType (value, type);
		}
		T RandomEnumNot<T> (Random rand, T not)
		{
			var x = RandomEnum<T> (rand);
			while (x.Equals (not)) {
				x = RandomEnum<T> (rand);
			}
			return x;
		}
		[Test]
		public void Success ()
		{
			AttemptSingle (Result.Success);
		}
		[Test]
		public void Fail ()
		{
			AttemptSingle (Result.Failure);
		}
		[Test]
		public void Error ()
		{
			AttemptSingle (Result.Error);
		}
		[Test]
		public void Processing ()
		{
			AttemptSingle (Result.Processing);
		}
		[Test]
		public void Unknown ()
		{
			AttemptSingle (Result.Unknown);
		}

		void AttemptSingle (Result interest)
		{
			for (int i = 1; i < 128; i++) {
				var kids = new List<Result> ();

				for (int k = 0; k < i; k++) {
					kids.Add (interest);
				}

				for (int k = 0; k < (i * 2); k++) {
					kids.Add (RandomEnumNot (this.r, interest));
				}

				kids.Sort ((Result x, Result y) => this.r.Next (-1, 1));
				var q = CreateParallel (interest == Result.Success ? i : 0,
							   interest == Result.Failure ? i : 0,
							   interest == Result.Processing ? i : 0,
							   interest == Result.Error ? i : 0,
							   interest == Result.Unknown ? i : 0,
							   kids.ToArray ());
				Assert.AreEqual (interest, q);
			}
		}

		Result CreateParallel (int s, int f, int p, int e, int u, params Result [] args)
		{
			IContext ctx = new SimpleContext ();

			Node root = new ParallelNode ("Root", null, s, f, p, e, u);
			foreach (var arg in args) {
				root.Children.Add (new AlwaysCmd (arg));
			}

			var ret = root.Tick (ctx);

			return ret;
		}
	}
}
