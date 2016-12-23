using System;
using Newtonsoft.Json;

namespace SimonWaite.BehaviourTree
{
	public class IntegerAssignmentCmd : BaseLeaf
	{
		[JsonRequired]
		public string VariableName { get; set; }
		[JsonRequired]
		public long Value { get; set; }

		public IntegerAssignmentCmd ()
		{
			Init ();
		}

		public IntegerAssignmentCmd (string varName, long value, string name = null)
		{
			Init (varName, value, name);
		}

		void Init (string varName = "", long value = 0, string name = null)
		{
			this.Name = name;
			this.Children = null;
			VariableName = varName;
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
