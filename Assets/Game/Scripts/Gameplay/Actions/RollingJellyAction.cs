using MoreMountains.Feedbacks;
using SharedLib.ExtensionMethods;
using System.Threading.Tasks;
using UnityEngine;

namespace AG.Gameplay.Actions
{
	public class RollingJellyAction : BaseAction
	{
		//----- Inspector fields -------------------

		[SerializeField]
		private MMF_Player _feedbacks;

		// TODO ADD FEEDBACK field

		//----- External dependencies ----------------


		//----- Internal variables -------------------

		protected override void Awake()
		{
		}

		//--------------------------------

		protected override void DoStartAction(object parameters)
		{
			StartActionAsync().RunAsync();
		}

		private async Task StartActionAsync()
		{
			await _feedbacks.PlayFeedbacksTask();
			Status = ActionStatus.Finished;
		}

		protected override ActionStatus DoUpdateAction()
		{
			return Status;
		}

		protected override void DoOnActionFinished()
		{
		}

		protected override void DoInterruptAction()
		{
		}
	}
}