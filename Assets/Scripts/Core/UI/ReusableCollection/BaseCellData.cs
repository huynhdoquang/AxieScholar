using System;

namespace Net.HungryBug.Core.UI.ReusableCollection
{
    public class BaseCellData : ICellData
    {
        /// <summary>
        /// Gets or sets the ICellData unique id.
        /// </summary>
        public int UniqueId { get; set; }

        /// <summary>
        /// Gets or sets the cell group id.
        /// </summary>
        public int GroupId { get; set; } = 0;

        /// <summary>
        /// Invokw whenever the cell that contains this data is selected.
        /// </summary>
        public event EventHandler<UICellEventArgs> OnCellSelected;

        /// <summary>
        /// Raise the cell selected.
        /// </summary>
        public void RaiseCellSelected(UICell cell) { this.OnCellSelected?.Invoke(this, new UICellEventArgs(this, cell)); }

        /// <summary>
        /// Invokw whenever the cell that contains this data is selected.
        /// </summary>
        public event EventHandler<UICellEventArgs> OnCellUnSelected;

        /// <summary>
        /// Raise the cell selected.
        /// </summary>
        public void RaiseCellUnSelected(UICell cell) { this.OnCellUnSelected?.Invoke(this, new UICellEventArgs(this, cell)); }

        /// <summary>
        /// Gets or sets the cell's selected state.
        /// </summary>
        public bool IsSelected { get; set; } = false;
    }
}

