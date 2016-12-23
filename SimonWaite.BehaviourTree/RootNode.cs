using System;
namespace SimonWaite.BehaviourTree
{
	public class RootNode : BaseDecorator
	{
		public RootNode ()
		{
		}
		public RootNode (string name, Node kid)
		{
			this.Name = name ?? "Root";
			this.Children.Add (kid);
		}
		public override Result Tick (IContext ctx)
		{
			throw new NotImplementedException ();
		}
	}
}
