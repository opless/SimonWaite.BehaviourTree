using System;
using Newtonsoft.Json;

namespace SimonWaite.BehaviourTree
{
	public class IntegerChangeCmd : BaseLeaf
	{
		[JsonRequired]
		public string VariableName { get; set; }

		[JsonRequired]
		public long Value { get; set; }

		public IntegerChangeCmd ()
		{
			VariableName = string.Empty;
			Value = 0;
		}

		public IntegerChangeCmd (string varName, long value, string name = null)
		{
			Name = name;
			VariableName = varName;
			Value = value;
		}

		public override Result Tick (IContext ctx)
		{
			var x = ctx.GetInteger (VariableName);
			x += Value;
			ctx.SetInteger (VariableName, x);
			return Result.Success;
		}
		public override string ToString ()
		{
			return string.Format ("[IntegerChangeCmd: VariableName={0}, AlterBy={1}]", VariableName, Value);
		}
	}
}
