using System;
namespace SimonWaite.BehaviourTree
{
	public class IntegerCompareNode : BaseDecorator
	{
		public string VariableNameA { get; set; }
		public string VariableNameB { get; set; }
		public Comparison CompareAction { get; set; }

		public IntegerCompareNode ()
		{
			VariableNameA = string.Empty;
			VariableNameB = string.Empty;
			CompareAction = Comparison.Equal;
		}
		public IntegerCompareNode (string a, Comparison cmp, string b)
		{
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
