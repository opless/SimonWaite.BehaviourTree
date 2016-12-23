using NUnit.Framework;
using System;
using System.Collections.Generic;
namespace SimonWaite.BehaviourTree.Test
{
	[TestFixture]
	public class SerialisationRoundTripTests
	{
		public static void Test ()
		{
			Console.WriteLine ("*** ROUNDTRIP TESTS ***");

			var x = new SerialisationRoundTripTests ();
			x.RoundTrips ();
		}
		[Test]
		public void RoundTrips ()
		{
			var r = new Random (1234);
			for (int i = 0; i < 128; i++) {
				var n = new RootNode ();
				AddComposite (n, r);
				var json = n.ToJsonByNewtonsoft ();
				var node = json.FromJsonByNewtonsoft<RootNode> ();
				var text = node.ToJsonByNewtonsoft ();
				Assert.AreEqual (json, text);
			}
			var q = (int)(DateTime.UtcNow.Ticks & 0xFFFFFF);
			Console.WriteLine ("*** Multiple roundtrip Test base: {0}", q);
			for (int i = 0; i < 128; i++) {
				r = new Random (q + i);
				var n = new RootNode ();
				AddComposite (n, r);
				var json = n.ToJsonByNewtonsoft ();
				var node = json.FromJsonByNewtonsoft<RootNode> ();
				var text = node.ToJsonByNewtonsoft ();
				Assert.AreEqual (json, text, "q =" + q);
			}
		}

		void AddComposite (Node n, Random r)
		{
			var x = r.Next () % 10;
			switch (x) {
			case 0: n.Children.Add (AddParallel (r)); break;
			case 1: n.Children.Add (AddSelector (r)); break;
			case 2: n.Children.Add (AddSequence (r)); break;
			case 3: AddComposite (n, r); break;
			case 4: AddDecoratorOrLeaf (n, r); break;
			default: AddLeaf (n, r); break;
			}
		}

		Node AddParallel (Random r)
		{
			var c = r.Next () % 5;
			c++;
			var n = new ParallelNode (RandomName ("Name", r),
						  null,
						  r.Next () % 3,
						  r.Next () % 3,
						  r.Next () % 3,
						  r.Next () % 3,
						  r.Next () % 3);
			for (int i = 0; i < c; i++)
				AddDecoratorOrLeaf (n, r);
			return n;
		}

		Node AddSelector (Random r)
		{
			var c = r.Next () % 5;
			c++;
			var n = new SelectorNode (RandomName ("Name", r),
						  null,
						  r.Next () % 2 == 0);
			for (int i = 0; i < c; i++)
				AddDecoratorOrLeaf (n, r);
			return n;
		}

		Node AddSequence (Random r)
		{
			var c = r.Next () % 5;
			c++;
			var n = new SequenceNode (RandomName ("Name", r),
						  null,
						  r.Next () % 2 == 0);
			for (int i = 0; i < c; i++)
				AddDecoratorOrLeaf (n, r);
			return n;
		}

		void AddDecoratorOrLeaf (Node n, Random r)
		{
			var x = r.Next () % 10;
			switch (x) {
			case 0: n.Children.Add (AddIntegerCompare (r)); break;
			case 1: n.Children.Add (AddRemapNode (r)); break;
			case 2: n.Children.Add (AddRepeater (r)); break;
			case 3: AddComposite (n, r); break;
			case 4: AddDecoratorOrLeaf (n, r); break;
			default: AddLeaf (n, r); break;
			}
		}

		Node AddRepeater (Random r)
		{
			var a = r.Next () % 5;
			var b = new HashSet<Result> ();
			if (r.Next () % 2 == 0) {
				var c = r.Next () % 4;
				for (int i = 0; i < c; i++) {
					b.Add (RandomEnum<Result> (r));
				}
			} else b = null;

			var xx = new RepeaterNode ("Repeater", null, a, b);
			AddLeaf (xx, r);
			return xx;
		}

		Node AddRemapNode (Random r)
		{
			var mapping = new SortedDictionary<Result, Result> ();

			var items = new Result [] { Result.Error, Result.Failure, Result.Processing, Result.Success, Result.Unknown };
			foreach (var i in items) {
				mapping.Add (i, RandomEnum<Result> (r));
			}
			var xx = new RemapNode (RandomName ("Name", r), null, mapping);
			AddLeaf (xx, r);
			return xx;
		}

		Node AddIntegerCompare (Random r)
		{
			var a = RandomName ("Var", r);
			var b = RandomName ("Var", r);
			var n = RandomName ("Name", r);
			var xx = new IntegerCompareNode (n, a, RandomEnum<Comparison> (r), b);
			AddLeaf (xx, r);
			return xx;
		}

		void AddLeaf (Node n, Random r)
		{
			var x = r.Next () % 3;
			Node add = null;
			switch (x) {
			case 0: add = AddAlways (r); break;
			case 1: add = AddIntegerAssignment (r); break;
			case 2: add = AddIntegerChange (r); break;
			default:
				throw new NotImplementedException ();
			}
			n.Children.Add (add);
		}
		T RandomEnum<T> (Random r)
		{
			Type type = typeof (T);
			Array values = Enum.GetValues (type);
			object value = values.GetValue (r.Next (values.Length));
			return (T)Convert.ChangeType (value, type);
		}
		string RandomName (string prefix, Random r)
		{
			return prefix + (100 + (r.Next () % 100));
		}
		Node AddAlways (Random r)
		{
			return new AlwaysCmd (RandomEnum<Result> (r), RandomName ("Name", r));
		}

		Node AddIntegerAssignment (Random r)
		{
			string name = RandomName ("Var", r);
			long value = r.Next ();
			var n = new IntegerAssignmentCmd (name, value);
			return n;
		}

		Node AddIntegerChange (Random r)
		{
			string name = RandomName ("Var", r);
			long value = r.Next () % 1000;
			if (r.Next () % 1 == 0)
				value = -value;
			var n = new IntegerChangeCmd (name, value);
			return n;
		}
	}
}
