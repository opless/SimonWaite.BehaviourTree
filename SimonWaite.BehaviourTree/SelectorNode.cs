using System;
using System.Collections.Generic;

namespace SimonWaite.BehaviourTree
{
	/// <summary>
	/// Selector Nodes execute their children from left to right, 
	/// and will stop executing its children when one of their children succeeds. 
	/// If a Selector's child succeeds, the Selector succeeds. 
	/// If all the Selector's children fail, the Selector fails.
	/// </summary>
	public class SelectorNode : BaseComposite
	{
		public bool Randomized { get; set; }

		private int position;
		private List<Node> list;

		public SelectorNode ()
		{
			Init ();
		}


		public SelectorNode (string name = null, IEnumerable<Node> children = null, bool randomized = false)
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
				case Result.Success:
					ctx.Log (this, Reason.ChildDidNotReturnFailure, list [i], state);
					Reset ();
					return state;
				case Result.Failure:
					continue;
				default:
					throw new ArgumentOutOfRangeException ();
				}
			}
			Reset ();
			ctx.Log (this, Reason.AllChildrenDidNotReportSuccess, null, Result.Failure);

			return Result.Failure;
		}

		void Reset ()
		{
			list = null;
			position = 0;
		}

		public override string ToString ()
		{
			return string.Format ("[SelectorNode: Randomized={0} Children={1}]", Randomized, Children.Count);
		}
	}
}
