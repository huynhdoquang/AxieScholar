using UnityEngine.UI;

namespace Net.HungryBug.Core.UI.ReusableCollection.Example
{
    public class TextCellData : BaseCellData
    {

    }

    public class ExampleCell : UICell
    {
        public Text Text;

        public override void Refresh()
        {
            base.Refresh();
            var x = this.DataContext as TextCellData;
            this.Text.text = x.UniqueId.ToString();
        }
    }
}
