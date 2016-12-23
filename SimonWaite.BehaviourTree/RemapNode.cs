using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SimonWaite.BehaviourTree
{
	public class RemapNode : BaseDecorator
	{
		[JsonRequired]
		public SortedDictionary<Result, Result> Mapping { get; set; }

		public RemapNode ()
		{
			Init ();
		}


		public RemapNode (string name = null, Node child = null, SortedDictionary<Result, Result> mapping = null)
		{
			Init (name, child, mapping);
		}

		void Init (string name = null, Node child = null, SortedDictionary<Result, Result> mapping = null)
		{
			this.Name = name;
			this.Children = new List<Node> ();
			if (child != null)
				this.Children.Add (child);

			Mapping = mapping ?? DefaultMapping ();
		}

		SortedDictionary<Result, Result> DefaultMapping ()
		{
			var map = new SortedDictionary<Result, Result> ();
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
