using System;
using System.Collections.Generic;

namespace SimonWaite.BehaviourTree
{
	/// <summary>
	/// Parallel node. We run all the child tasks 'at once'.
	/// This actually means that we run all of them in (random) sequence, in one 'tick'.
	/// </summary>
	public class ParallelNode : BaseComposite
	{
		public int SuccessThreshold;
		public int FailureThreshold;
		public int ProcessingThreshold;
		public int ErrorThreshold;
		public int UnknownThreshold;

		const bool Randomized = true;

		private int position;
		private List<Node> list;

		public ParallelNode ()
		{
			Init ();
		}


		public ParallelNode (string name = null, IEnumerable<Node> children = null,
				     int successThreshold = 0,
				     int failureThreshold = 0,
				     int processingThreshold = 0,
				     int errorThreshold = 0,
				     int unknownThreshold = 0)
		{
			Init (name, children,
			      successThreshold,
			      failureThreshold,
			      processingThreshold,
			      errorThreshold,
			      unknownThreshold);
		}

		void Init (string name = null, IEnumerable<Node> children = null,
			   int successThreshold = 0,
			   int failureThreshold = 0,
			   int processingThreshold = 0,
			   int errorThreshold = 0,
			   int unknownThreshold = 0)
		{
			this.Name = name ?? Guid.NewGuid ().ToString ();
			this.Children = new List<Node> (children ?? new Node [0]);
			list = null;
			position = 0;

			SuccessThreshold = successThreshold;
			FailureThreshold = failureThreshold;
			ProcessingThreshold = processingThreshold;
			ErrorThreshold = errorThreshold;
			UnknownThreshold = unknownThreshold;
		}

		public override Result Tick (IContext ctx)
		{
			if (list == null) {
				list = Children.Randomize (Randomized, ctx);
				position = 0;
			}
			var unknown = 0;
			var processing = 0;
			var error = 0;
			var success = 0;
			var failure = 0;
			for (int i = position; i < list.Count; i++) {
				var state = ctx.Process (list [i]);

				switch (state) {
				case Result.Unknown: unknown++; break;
				case Result.Processing: processing++; break;
				case Result.Error: error++; break;
				case Result.Success: success++; break;
				case Result.Failure: failure++; break;
				default:
					throw new ArgumentOutOfRangeException ();
				}
			}
			Reset ();

			var ret = Result.Unknown;

			//TODO: order might need tweaking here.
			if (SuccessThreshold > 0 && success >= SuccessThreshold) {
				ret = Result.Success;
			} else if (FailureThreshold > 0 && failure >= FailureThreshold) {
				ret = Result.Failure;
			} else if (ProcessingThreshold > 0 && processing >= ProcessingThreshold) {
				ret = Result.Processing;
			} else if (ErrorThreshold > 0 && error >= ErrorThreshold) {
				ret = Result.Error;
			} else if (UnknownThreshold > 0 && unknown >= UnknownThreshold) {
				ret = Result.Unknown;
			}

			ctx.Log (this, Reason.ThresholdReached, null, ret);

			return ret;
		}

		void Reset ()
		{
			list = null;
			position = 0;
		}

		public override string ToString ()
		{
			return string.Format ("[ParallelNode Children={0}]", Children.Count);
		}
	}
}
