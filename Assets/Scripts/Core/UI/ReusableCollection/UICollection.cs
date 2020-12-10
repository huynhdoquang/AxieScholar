using Net.HungryBug.Core.Attribute;
using Net.HungryBug.Core.Reactive;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Net.HungryBug.Core.UI.ReusableCollection
{
    public enum SelectionMode
    {
        None,
        Single,
        Multi,
    }

    public sealed class UICollection : UIResolver
    {
        public event Action<UICell, ICellData> OnCellSelected;
        public event Action<UICell, ICellData> OnCellUnSelected;
        public event Action<UICell, ICellData> OnCellClicked;

        /// <summary>
        /// The scroll view.
        /// </summary>
        [Header("UICollection")]
        [UIOutlet("@[ScrollView]", true)]
        public UIScrollView ScrollView;

        /// <summary>
        /// The selection mode.
        /// </summary>
        [SerializeField] private SelectionMode mode;

        /// <summary>
        /// allow to unselect a selected cell, only available on single selection mode.
        /// </summary>
        [Tooltip("Only available on Single selection mode")]
        [SerializeField] private bool allowUnSelectCell = false;

        private ICellData SingleItem = null;
        private readonly HashSet<ICellData> MultiItems = new HashSet<ICellData>();
        private bool bulkEdit = false;

        public ReactiveCollection<ICellData> DataContext { get { return this.ScrollView.DataContext; } }

        /// <summary>
        /// 
        /// </summary>
        public void SetData(IEnumerable<ICellData> cellData)
        {
            //reset all.
            this.MultiItems.Clear();
            this.SingleItem = null;

            //do bulk edit.
            this.bulkEdit = true;
            try
            {
                this.ScrollView.SetData(cellData);
            }
            catch (Exception)
            {

            }
            this.bulkEdit = false;

            //set selected items.
            if (this.gameObject.activeInHierarchy == true)
            {
                foreach (var item in cellData)
                    this.DataContext_OnItemAdded(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICellData[] GetAllCellData() { return this.ScrollView.DataContext.ToArray(); }

        /// <summary>
        /// Initialize UICollection, install hooks.
        /// </summary>
        private void Start()
        {
            this.ScrollView.OnClicked += this.ScrollView_OnClicked;
            this.ScrollView.OnCellShowed += this.ScrollView_OnCellShowed;

            this.ScrollView.DataContext.OnItemAdded += this.DataContext_OnItemAdded;
            this.ScrollView.DataContext.OnItemEdited += this.DataContext_OnItemEdited;
            this.ScrollView.DataContext.OnItemRemoved += this.DataContext_OnItemRemoved;
        }

        /// <summary>
        /// Uninstall hooks on destroy.
        /// </summary>
        private void OnDestroy()
        {
            this.ScrollView.OnClicked -= this.ScrollView_OnClicked;
            this.ScrollView.OnCellShowed -= this.ScrollView_OnCellShowed;

            this.ScrollView.DataContext.OnItemAdded -= this.DataContext_OnItemAdded;
            this.ScrollView.DataContext.OnItemEdited -= this.DataContext_OnItemEdited;
            this.ScrollView.DataContext.OnItemRemoved -= this.DataContext_OnItemRemoved;

            this.MultiItems.Clear();
            this.DataContext.Clear();
        }

        private void DataContext_OnItemAdded(ICellData cell)
        {
            if (this.bulkEdit == true)
                return;

            switch (this.mode)
            {
                case SelectionMode.Single:
                    {
                        if (cell.IsSelected)
                        {
                            this.SingleItem = cell;
                        }

                        break;
                    }

                case SelectionMode.Multi:
                    {
                        this.MultiItems.Remove(cell);
                        if (cell.IsSelected)
                        {
                            this.MultiItems.Add(cell);
                        }
                        break;
                    }
            }
        }

        private void DataContext_OnItemEdited(object sender, CollectionUpdateEventArgs<ICellData> e)
        {
            if (this.bulkEdit == true)
                return;

            switch (this.mode)
            {
                case SelectionMode.Single:
                    {
                        if (!e.NewValue.IsSelected)
                        {
                            this.SingleItem = null;
                        }

                        else
                        {
                            this.SingleItem = e.NewValue;
                        }

                        break;
                    }

                case SelectionMode.Multi:
                    {
                        if (this.MultiItems.Contains(e.OldValue))
                        {
                            this.MultiItems.Remove(e.OldValue);
                        }

                        if (e.NewValue.IsSelected)
                        {
                            this.MultiItems.Add(e.NewValue);
                        }
                        else
                        {
                            this.MultiItems.Remove(e.NewValue);
                        }

                        break;
                    }
            }
        }

        private void DataContext_OnItemRemoved(ICellData cell)
        {
            if (this.bulkEdit == true)

                return;
            if (this.SingleItem == cell)
            {
                this.SingleItem = null;
            }

            if (this.MultiItems.Contains(cell))
            {
                this.MultiItems.Remove(cell);
            }
        }

        public void SelectCell(ICellData cell)
        {
            var uiCell = this.ScrollView.GetVisibleCell(cell);
            this.ScrollView_OnClicked(uiCell, cell);
        }

        /// <summary>
        /// Refresh all visible cells.
        /// </summary>
        public void RefreshAll()
        {
            var cells = this.ScrollView.GetVisibleCells();
            foreach (var cell in cells)
            {
                cell.Refresh();
            }
        }

        /// <summary>
        /// Gets a list of selected cells on <see cref="SelectionMode.Multi"/>.
        /// <returns></returns>
        public ICellData[] GetSelectedCells()
        {
            if (this.mode == SelectionMode.Multi)
            {
                return this.MultiItems.ToArray();
            }
            else if (mode == SelectionMode.Single && this.SingleItem != null)
            {
                return new ICellData[] { this.SingleItem };
            }
            else
            {
                return new ICellData[0];
            }
        }

        /// <summary>
        /// Gets selected cell on <see cref="SelectionMode.Single"/>.
        /// </summary>
        public ICellData GetSelectedCell()
        {
            return this.SingleItem;
        }

        /// <summary>
        /// Invoke whenever a cell has been created or reused.
        /// </summary>
        private void ScrollView_OnCellShowed(UICell cell, ICellData data)
        {
            switch (this.mode)
            {
                case SelectionMode.Single:
                case SelectionMode.Multi:
                    {
                        cell.ShowIndicator(data.IsSelected);
                        break;
                    }
            }
        }

        /// <summary>
        /// Invoke whenever a cell has been selected.
        /// </summary>
        private void ScrollView_OnClicked(UICell cell, ICellData data)
        {
            switch (this.mode)
            {
                case SelectionMode.None:
                    {
                        data.RaiseCellSelected(cell);
                        break;
                    }
                case SelectionMode.Single:
                    {
                        if (this.SingleItem == null)
                        {
                            this.SingleItem = data;
                            this.SetSelectCell(cell, this.SingleItem);
                        }
                        else
                        {
                            if (this.SingleItem == data)
                            {
                                if (this.allowUnSelectCell)
                                {
                                    this.SetUnSelectCell(cell, this.SingleItem);
                                    this.SingleItem = null;
                                }
                                else
                                {
                                    this.SetSelectCell(cell, this.SingleItem);
                                }
                            }
                            else
                            {
                                //unselect current.
                                var currentSelectedCell = this.ScrollView.GetVisibleCell(this.SingleItem);
                                this.SetUnSelectCell(currentSelectedCell, this.SingleItem);

                                //select new.
                                this.SingleItem = data;
                                this.SetSelectCell(cell, this.SingleItem);
                            }
                        }
                        break;
                    }

                case SelectionMode.Multi:
                    {
                        if (this.MultiItems.Add(data))
                        {
                            this.SetSelectCell(cell, data);
                        }
                        else
                        {
                            this.MultiItems.Remove(data);
                            this.SetUnSelectCell(cell, data);
                        }
                        break;
                    }
            }

            this.OnCellClicked?.Invoke(cell, data);
        }

        /// <summary>
        /// Sets data and its cell to selected state.
        /// </summary>
        private void SetSelectCell(UICell cell, ICellData data)
        {
            data.IsSelected = true;
            //show indicator if cell is visible.
            if (cell != null)
            {
                cell.ShowIndicator(true);
            }

            data.RaiseCellSelected(cell);
            this.OnCellSelected?.Invoke(cell, data);
        }

        /// <summary>
        /// Sets data and its cell to unselected state.
        /// </summary>
        private void SetUnSelectCell(UICell cell, ICellData data)
        {
            data.IsSelected = false;
            //hide indicator if cell is visible.
            if (cell != null)
            {
                cell.ShowIndicator(false);
            }

            data.RaiseCellUnSelected(cell);
            this.OnCellUnSelected?.Invoke(cell, data);
        }

#if UNITY_EDITOR
        protected override void Editor_CustomResolve()
        {
            base.Editor_CustomResolve();
            if (this.ScrollView == null)
            {
                this.ScrollView = this.GetComponent<UIScrollView>();
            }
        }
#endif
    }
}