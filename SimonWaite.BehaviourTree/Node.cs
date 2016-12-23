using System;
using System.Collections.Generic;

namespace SimonWaite.BehaviourTree
{
	public abstract class Node
	{
		public Node ()
		{
			Children = new List<Node> ();
		}

		public string Name { get; set; }

		public List<Node> Children { get; set; }

		public abstract Result Tick (IContext ctx);

		public abstract void Validate ();
	}
}
