using System;
namespace SimonWaite.BehaviourTree
{
	public interface IContext
	{
		/// <summary>
		/// Gets the tick. Monotonic, increasing.
		/// </summary>
		/// <value>The tick.</value>
		long Tick { get; }

		/// <summary>
		/// Gets the integer variable indexed by name.
		/// </summary>
		/// <returns>The integer.</returns>
		/// <param name="name">Name.</param>
		long GetInteger (string name);

		/// <summary>
		/// Sets the integer variable indexed by name.
		/// </summary>
		/// <param name="variableName">Variable name.</param>
		/// <param name="value">Value.</param>
		void SetInteger (string variableName, long value);

		/// <summary>
		/// Returns the next Random integer.
		/// </summary>
		/// <returns>Next Random Integer.</returns>
		int RandomInt ();

		/// <summary>
		/// Process (or not) the specified node.
		/// </summary>
		/// <param name="node">Node.</param>
		Result Process (Node node);

		/// <summary>
		/// Instructs the context that it can switch here, during Process().
		/// </summary>
		void TaskSwitch ();

		/// <summary>
		/// Log the specified Node, with a returnReason, the optional targetNode and its resultValue.
		/// </summary>
		/// <param name="currentNode">Current node.</param>
		/// <param name="returnReason">Return reason.</param>
		/// <param name="targetNode">(Optional) Target node.</param>
		/// <param name="resultValue">Result value.</param>
		void Log (Node currentNode, Reason returnReason, Node targetNode, Result resultValue);

		/// <summary>
		/// Log the specified Node, with a returnReason, the optional targetNode and its resultValue.
		/// </summary>
		/// <param name="currentNode">Current node.</param>
		/// <param name="returnReason">Return reason.</param>
		/// <param name="targetNode">(Optional) Target node.</param>
		/// <param name="originalValue">Original value.</param>
		/// <param name="resultValue">Result value.</param>
		void Log (Node currentNode, Reason returnReason, Node targetNode, Result originalValue, Result resultValue);

		/// <summary>
		/// Log the specified Node, with a returnReason, the optional targetNode and its resultValue.
		/// </summary>
		/// <param name="currentNode">Current node.</param>
		/// <param name="returnReason">Return reason.</param>
		/// <param name="targetNode">(Optional) Target node.</param>
		/// <param name="resultValue">Result value.</param>
		/// <param name="iteration">Current iteration counter</param>
		void Log (Node currentNode, Reason returnReason, Node targetNode, Result resultValue, int iteration);

	}

}
