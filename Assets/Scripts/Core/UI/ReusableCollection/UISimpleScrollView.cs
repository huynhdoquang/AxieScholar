using System.Collections.Generic;
using Net.HungryBug.Core.Reactive;

namespace Net.HungryBug.Core.UI.ReusableCollection
{
    public sealed class UISimpleScrollView : UIScrollView
    {
        private readonly Dictionary<ICellData, UICell> cells = new Dictionary<ICellData, UICell>();

        /// <summary>
        /// Get visible by faster way.
        /// </summary>
        public override UICell GetVisibleCell(ICellData data)
        {
            this.cells.TryGetValue(data, out var result);
            return result;
        }

        protected override void DataContext_SetData(IEnumerable<ICellData> data)
        {
            foreach (var cell in this.cells)
            {
                this.UICellPool.Collect(cell.Value);
                cell.Value.DataContext = null;
            }
            this.cells.Clear();

            foreach(var cell in data)
                this.DataContext_OnItemAdded(cell);
        }

        protected override void DataContext_OnItemAdded(ICellData cell)
        {
            if (!this.cells.TryGetValue(cell, out var uiCell))
            {
                //spawn cell.
                uiCell = this.UICellPool.Provide();
                uiCell.DataContext = cell;

                //cache cell.
                this.cells.Add(cell, uiCell);

                //raise show event.
                this.RaiseOnCellShowed(uiCell, cell);
            }

            this.ShowEmptyIndicator(this.cells.Count == 0);
        }

        protected override void DataContext_OnItemEdited(object sender, CollectionUpdateEventArgs<ICellData> e)
        {
            if (this.cells.TryGetValue(e.OldValue, out var uiCell) && !this.cells.ContainsKey(e.NewValue))
            {
                //remove old cell.
                this.cells.Remove(e.OldValue);

                //update new datacontext to uiCell.
                uiCell.DataContext = e.NewValue;
                this.cells.Add(e.NewValue, uiCell);
            }

            this.ShowEmptyIndicator(this.cells.Count == 0);
        }

        protected override void DataContext_OnItemRemoved(ICellData cell)
        {
            if (this.cells.TryGetValue(cell, out var uiCell))
            {
                this.cells.Remove(cell);
                this.UICellPool.Collect(uiCell);
                uiCell.DataContext = null;
            }

            this.ShowEmptyIndicator(this.cells.Count == 0);
        }
    }
}