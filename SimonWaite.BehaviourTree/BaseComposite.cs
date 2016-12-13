using System;
namespace SimonWaite.BehaviourTree
{
	public abstract class BaseComposite : Node
	{
		public override void Validate ()
		{
			if (Children == null)
				throw new NotSupportedException ();
		}
	}
}
