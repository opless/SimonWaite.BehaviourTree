using System;
namespace SimonWaite.BehaviourTree
{
	public enum Reason
	{
		Unclear,
		ChildDidNotReturnSuccess,
		AllChildrenReportedSuccess,
		ChildDidNotReturnFailure,
		AllChildrenDidNotReportSuccess,
		ThresholdReached,
		StateWasRemapped,
		StateWasNotRemapped,
		DesiredResultAchieved,
		DesiredResultNotAchieved,
		DesiredResultNotYetAchived,
		IterationLimitReached,
		IterationLimitNotYetReached,
		StateWasSet,
	}
}
