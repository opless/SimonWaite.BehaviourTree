using System;
namespace SimonWaite.BehaviourTree
{
	public class IntegerChangeCmd : BaseLeaf
	{
		public string VariableName { get; set; }

		public long Value { get; set; }

		public IntegerChangeCmd ()
		{
			VariableName = string.Empty;
			Value = 0;
		}

		public IntegerChangeCmd (string name, long value)
		{
			Name = name;
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
