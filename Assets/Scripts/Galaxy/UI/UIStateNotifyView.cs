using Net.HungryBug.Core.UI;

namespace Net.HungryBug.Galaxy.UI.Shared
{
    public class UIStateNotifyView : UIResolver
    {
        public void EnableLoading(bool enableLoading)
        {
            if(enableLoading)
                this.State = "@[State] Loading";
            else
                this.State = "@[State] Empty";
        }

        public void EnableError()
        {
            this.State = "@[State] Error";
        }
    }
}