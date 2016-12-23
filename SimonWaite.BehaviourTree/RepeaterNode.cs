using System;
using System.Collections.Generic;

namespace SimonWaite.BehaviourTree
{

	/// <summary>
	/// Repeater node. 
	/// Repeater is a decorator that repeats the tick signal until the child node returns 
	/// RUNNING or ERROR. Optionally, a maximum number of repetitions can be defined.
	/// </summary>
	public class RepeaterNode : BaseDecorator
	{
		public HashSet<Result> UntilResult { get; set; }
		public int Count { get; set; }

		int iteration = 0;

		public RepeaterNode ()
		{
			Init ();
		}


		public RepeaterNode (string name = null, Node child = null, int count = 0, HashSet<Result> untilResult = null)
		{
			Init (name, child, count, untilResult);
		}

		void Init (string name = null, Node child = null, int count = 0, HashSet<Result> untilResult = null)
		{
			this.Name = name;
			this.Children = new List<Node> ();
			if (child != null)
				this.Children.Add (child);
			this.Count = count;
			this.UntilResult = untilResult ?? new HashSet<Result> ();
		}



		public override Result Tick (IContext ctx)
		{
			Validate ();
			//TODO this appears not to work
			Result r = Result.Unknown;
			bool done = false;

			if (UntilResult.Count != 0) {
				done = TickUntil (ctx, out r);
			}
			if (!done && Count > 0) {
				done = TickCount (ctx, out r);
			}
			if (done) {
				if (UntilResult.Count != 0 && Count <= iteration) {
					ctx.Log (this, Reason.DesiredResultNotAchieved, Children [0], Result.Failure);
					return Result.Failure;
				}
				return r;
			}
			ctx.TaskSwitch ();
			return Result.Processing;

		}
		bool TickUntil (IContext ctx, out Result r)
		{
			var s = ctx.Process (Children [0]);
			if (UntilResult.Contains (s)) {
				r = Result.Success;
				ctx.Log (this, Reason.DesiredResultAchieved, Children [0], s, Result.Success);
				return true;
			}
			r = Result.Unknown;
			ctx.Log (this, Reason.DesiredResultNotYetAchived, Children [0], s, iteration);
			return false;
		}
		bool TickCount (IContext ctx, out Result r)
		{
			var s = ctx.Process (Children [0]);
			iteration++;
			if (iteration < Count) {
				ctx.Log (this, Reason.IterationLimitNotYetReached, Children [0], s, iteration);
				r = Result.Processing;
				return false;
			}
			ctx.Log (this, Reason.IterationLimitReached, Children [0], Result.Success);
			r = Result.Success;
			return true;
		}


		public override void Validate ()
		{
			base.Validate ();

			if (Count == 0 && UntilResult.Count == 0)
				throw new NotSupportedException ();
		}
		public override string ToString ()
		{
			return string.Format ("[RepeaterNode: UntilResult={0}, Count={1}]", UntilResult, Count);
		}
	}
}
