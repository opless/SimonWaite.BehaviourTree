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
			this.Name = name ?? Guid.NewGuid ().ToString ();
			this.Children = new List<Node> ();
			if (child != null)
				this.Children.Add (child);
			this.Count = count;
			this.UntilResult = untilResult ?? new HashSet<Result> ();
		}



		public override Result Tick (IContext ctx)
		{
			Validate ();

			// if we're (1) still counting, or (2) haven't found a desired result - process
			if ((Count > 0 && iteration < Count) || (Count == 0 && UntilResult.Count > 0)) {
				var state = ctx.Process (Children [0]);

				// we have found our result
				if (UntilResult.Count > 0 && UntilResult.Contains (state)) {
					ctx.Log (this, Reason.DesiredResultAchieved, Children [0], state, Result.Success);
					return Result.Success;
				}

				// not found yet, but lets keep running
				if (Count == 0) {
					ctx.Log (this, Reason.DesiredResultNotYetAchived, Children [0], state, iteration);

				} else {
					// assume iteration not finished yet
					ctx.Log (this, Reason.IterationLimitNotYetReached, Children [0], state, iteration);
				}
				iteration++;
				// Tag ctx to save state.
				ctx.TaskSwitch ();
				return Result.Processing;
			} else if (Count > 0 && iteration >= Count && UntilResult.Count == 0) {
				// passed our iteration limit.
				ctx.Log (this, Reason.IterationLimitReached, Children [0], Result.Success);
				return Result.Success;
			} else {
				// assume we're at our iteration limit and the desired result has not come to pass.
				ctx.Log (this, Reason.DesiredResultNotAchieved, Children [0], Result.Failure);
				return Result.Failure;
			}
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
