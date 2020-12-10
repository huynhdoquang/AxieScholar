using System;

namespace Net.HungryBug.Core.UI.ReusableCollection
{
    public class UICellEventArgs : EventArgs
    {
        /// <summary>
        /// The cell view. NOTED THAT, on reusable mode, this value may be null (invisible)
        /// </summary>
        public readonly UICell Cell;

        /// <summary>
        /// The cell data.
        /// </summary>
        public readonly ICellData Data;

        public UICellEventArgs(ICellData data, UICell cell)
        {
            this.Cell = cell;
            this.Data = data;
        }
    }

    public interface ICellData
    {
        /// <summary>
        /// Gets or sets the ICellData unique id.
        /// </summary>
        int UniqueId { get; set; }

        /// <summary>
        /// Gets or sets the cell group id.
        /// </summary>
        int GroupId { get; set; }

        /// <summary>
        /// Invokw whenever the cell that contains this data is selected.
        /// </summary>
        event EventHandler<UICellEventArgs> OnCellSelected;

        /// <summary>
        /// Raise the cell is selected.
        /// </summary>
        void RaiseCellSelected(UICell cell);

        /// <summary>
        /// Invoke whenever the cell that contains this data is unselected.
        /// </summary>
        event EventHandler<UICellEventArgs> OnCellUnSelected;

        /// <summary>
        /// Raise the cell is unselected.
        /// </summary>
        void RaiseCellUnSelected(UICell cell);

        /// <summary>
        /// Gets or sets the cell's selected state.
        /// </summary>
        bool IsSelected { get; set; }
    }
}
