using System;
using System.Collections.Generic;

namespace SimonWaite.BehaviourTree
{
	public class SimpleContext : IContext
	{
		Dictionary<string, long> integerStore = new Dictionary<string, long> ();

		Random prng = new Random ();

		public SimpleContext ()
		{
		}

		public long Tick {
			get {
				return DateTime.UtcNow.Ticks;
			}
		}

		public long GetInteger (string name)
		{
			if (!integerStore.ContainsKey (name)) {
				return 0;
			}
			return integerStore [name];
		}

		public void Log (Node currentNode, Reason returnReason, Node targetNode, Result resultValue)
		{
			D ("{0} {1} {2} {3}", currentNode, returnReason, targetNode, resultValue);
		}

		public void Log (Node currentNode, Reason returnReason, Node targetNode, Result resultValue, int iteration)
		{
			D ("{0} {1} {2} {3} #{4}", currentNode, returnReason, targetNode, resultValue, iteration);
		}

		public void Log (Node currentNode, Reason returnReason, Node targetNode, Result originalValue, Result resultValue)
		{
			D ("{0} {1} {2} was:{3} now:{4}", currentNode, returnReason, targetNode, originalValue, resultValue);
		}

		public Result Process (Node node)
		{
			D ("Processing: {0}", node);
			return node.Tick (this);
		}

		public int RandomInt ()
		{
			return prng.Next ();
		}

		public void SetInteger (string variableName, long value)
		{
			if (!integerStore.ContainsKey (variableName)) {
				integerStore.Add (variableName, value);
			} else {
				integerStore [variableName] = value;
			}
		}

		public void TaskSwitch ()
		{
			D ("no-task-switching-here");
		}
		void D (string text)
		{
			D ("{0}", text);
		}
		void D (string format, params object [] args)
		{
			Console.WriteLine (format, args);
		}
	}
}
