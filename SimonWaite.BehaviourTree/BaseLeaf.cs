using System;

namespace SimonWaite.BehaviourTree
{
	public abstract class BaseLeaf : Node
	{
		public override void Validate ()
		{
			if (Children != null && Children.Count != 0)
				throw new NotSupportedException ();
		}
	}
}