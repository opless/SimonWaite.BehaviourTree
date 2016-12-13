using System;
namespace SimonWaite.BehaviourTree
{
	public abstract class BaseDecorator : Node
	{
		public override void Validate ()
		{
			if ((Children == null) || (Children.Count != 1))
				throw new NotSupportedException ();
		}
	}
}
