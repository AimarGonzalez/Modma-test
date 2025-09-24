using System;

namespace AG.Gameplay.Actions
{
	public interface IActionStatus
	{
		event Action<ActionStatus> OnActionFinishedEvent;
		ActionStatus Status { get; }
	}
}