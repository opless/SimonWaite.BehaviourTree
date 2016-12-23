using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SimonWaite.BehaviourTree
{
	public class IntegerCompareNode : BaseDecorator
	{
		[JsonRequired]
		public string VariableNameA { get; set; }
		[JsonRequired]
		public string VariableNameB { get; set; }
		[JsonRequired]
		public Comparison CompareAction { get; set; }

		public IntegerCompareNode ()
		{
			Init ();
		}
		public IntegerCompareNode (string name, string a, Comparison cmp, string b)
		{
			Init (name, a, cmp, b);
		}

		void Init (string name = null, string a = "", Comparison cmp = Comparison.Equal, string b = "")
		{
			this.Name = name;
			this.Children = new List<Node> ();

			VariableNameA = a;
			VariableNameB = b;
			CompareAction = cmp;
		}

		public override Result Tick (IContext ctx)
		{
			var a = ctx.GetInteger (VariableNameA);
			var b = ctx.GetInteger (VariableNameB);
			var result = false;

			switch (CompareAction) {
			case Comparison.NotEqual:
				result = a != b;
				break;
			case Comparison.Equal:
				result = a == b;
				break;
			case Comparison.GreaterThan:
				result = a > b;
				break;
			case Comparison.LessThan:
				result = a < b;
				break;
			case Comparison.GreaterThanOrEqual:
				result = a >= b;
				break;
			case Comparison.LessThanOrEqual:
				result = a <= b;
				break;
			default:
				throw new ArgumentOutOfRangeException ();
			}

			if (result) {
				var state = ctx.Process (Children [0]);
				return state;
			} else {
				return Result.Success;
			}
		}
		public override string ToString ()
		{
			return string.Format ("[IntegerCompareCmd: A={0},Op={2} B={1}]", VariableNameA, VariableNameB, CompareAction);
		}
	}
}
