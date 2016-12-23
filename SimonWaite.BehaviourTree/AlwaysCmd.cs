using System;
using Newtonsoft.Json;

namespace SimonWaite.BehaviourTree
{
	public class AlwaysCmd : BaseLeaf
	{
		public AlwaysCmd ()
		{
			Init ();
		}
		[JsonRequired]
		public Result ReturnAs { get; set; }

		public AlwaysCmd (Result returnAs, string name = null)
		{
			Init (returnAs, name);
		}
		void Init (Result returnAs = Result.Success, string name = null)
		{
			this.Name = name;
			this.Children = null;
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
