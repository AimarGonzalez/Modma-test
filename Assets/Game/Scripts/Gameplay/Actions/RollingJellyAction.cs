namespace AG.Gameplay.Actions
{
    public class RollingJellyAction : BaseAction
    {
        //----- Inspector fields -------------------

       
        // TODO ADD FEEDBACK field

        //----- External dependencies ----------------


        //----- Internal variables -------------------

        protected override void Awake()
        {
        }

        //--------------------------------

        protected override void DoStartAction(object parameters)
        {
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