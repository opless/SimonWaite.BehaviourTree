using System;
using System.Collections.Generic;

namespace SimonWaite.BehaviourTree
{
	public class RemapNode : BaseDecorator
	{
		public Dictionary<Result, Result> Mapping;

		public RemapNode ()
		{
			Init ();
		}


		public RemapNode (string name = null, Node child = null, Dictionary<Result, Result> mapping = null)
		{
			Init (name, child, mapping);
		}

		void Init (string name = null, Node child = null, Dictionary<Result, Result> mapping = null)
		{
			this.Name = name ?? Guid.NewGuid ().ToString ();
			this.Children = new List<Node> ();
			if (child != null)
				this.Children.Add (child);

			Mapping = mapping ?? DefaultMapping ();
		}

		Dictionary<Result, Result> DefaultMapping ()
		{
			var map = new Dictionary<Result, Result> ();
			foreach (var x in new Result [] {
				Result.Error, Result.Processing, Result.Unknown}) {
				map.Add (x, x);
			}
			map.Add (Result.Failure, Result.Success);
			map.Add (Result.Success, Result.Failure);
			return map;
		}

		public override Result Tick (IContext ctx)
		{
			Validate ();

			var state = ctx.Process (Children [0]);

			var changedState = state;

			if (Mapping.ContainsKey (state)) {
				changedState = Mapping [state];
				ctx.Log (this, Reason.StateWasRemapped, Children [0], state, changedState);
			} else {
				ctx.Log (this, Reason.StateWasNotRemapped, Children [0], state);
			}
			return changedState;
		}

		public override string ToString ()
		{
			return string.Format ("[RemapNode]");
		}
	}
}
