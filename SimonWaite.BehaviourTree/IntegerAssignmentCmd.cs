using System;
namespace SimonWaite.BehaviourTree
{
	public class IntegerAssignmentCmd : BaseLeaf
	{
		public string VariableName { get; set; }

		public long Value { get; set; }

		public IntegerAssignmentCmd ()
		{
			VariableName = string.Empty;
			Value = 0;
		}

		public IntegerAssignmentCmd (string name, long value)
		{
			VariableName = name;
			Value = value;
		}

		public override Result Tick (IContext ctx)
		{
			ctx.SetInteger (VariableName, Value);
			return Result.Success;
		}

		public override string ToString ()
		{
			return string.Format ("[IntegerAssignmentCmd: VariableName={0}, Value={1}]", VariableName, Value);
		}
	}
}
