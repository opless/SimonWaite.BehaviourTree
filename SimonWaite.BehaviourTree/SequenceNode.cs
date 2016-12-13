using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace SimonWaite.BehaviourTree
{
	/// <summary>
	/// Sequence node. Execute each child in turn (optionally in random order). 
	/// If any fail, abort the sequence and return that value.
	/// </summary>
	public class SequenceNode : BaseComposite
	{
		public bool Randomized { get; set; }

		private int position;
		private List<Node> list;

		public SequenceNode ()
		{
			Init ();
		}


		public SequenceNode (string name = null, IEnumerable<Node> children = null, bool randomized = false)
		{
			Init (name, children, randomized);
		}

		void Init (string name = null, IEnumerable<Node> children = null, bool randomized = false)
		{
			this.Name = name ?? Guid.NewGuid ().ToString ();
			this.Children = new List<Node> (children ?? new Node [0]);
			this.Randomized = randomized;
			list = null;
			position = 0;
		}

		public override Result Tick (IContext ctx)
		{
			if (list == null) {
				list = Children.Randomize (this.Randomized, ctx);
				position = 0;
			}

			for (int i = position; i < list.Count; i++) {
				var state = ctx.Process (list [i]);

				switch (state) {
				case Result.Unknown:
				case Result.Processing:
				case Result.Error:
				case Result.Failure:
					ctx.Log (this, Reason.ChildDidNotReturnSuccess, list [i], state);
					return state;
				case Result.Success:
					break;
				default:
					throw new ArgumentOutOfRangeException ();
				}
			}
			ctx.Log (this, Reason.AllChildrenReportedSuccess, null, Result.Success);
			return Result.Success;
		}

		public override string ToString ()
		{
			return string.Format ("[SequenceNode: Randomized={0} Children={1}]", Randomized, Children.Count);
		}
	}
}
