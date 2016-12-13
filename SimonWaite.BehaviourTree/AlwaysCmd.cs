using System;
namespace SimonWaite.BehaviourTree
{
	public class AlwaysCmd : BaseLeaf
	{
		public AlwaysCmd ()
		{
		}

		public Result ReturnAs { get; set; }

		public AlwaysCmd (Result returnAs)
		{
			ReturnAs = returnAs;
		}

		public override Result Tick (IContext ctx)
		{
			ctx.Log (this, Reason.StateWasSet, null, ReturnAs);
			return ReturnAs;
		}

		public override string ToString ()
		{
			return string.Format ("[AlwaysCmd: ReturnAs={0}]", ReturnAs);
		}
	}
}
