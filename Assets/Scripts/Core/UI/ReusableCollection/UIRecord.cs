using Net.HungryBug.Core.Attribute;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Net.HungryBug.Core.UI.ReusableCollection
{
    /// <summary>
    /// The record may be row or column.
    /// In the case of horizontal grid, the record is a column.
    /// In the case of vertical grid, the record is a row.
    /// </summary>
    public sealed class UIRecord : UIResolver
    {
        const string STATE_NORMAL = "@[State] Normal";
        const string STATE_EXPANDER = "@[State] Expander";

        const string STATE_BTN_EXPAND = "@[State] Collapse";
        const string STATE_BTN_COLLAPSE = "@[State] Expand";

        public event Action<int, bool> OnExpandChanged;
        public readonly Reactive.ReactiveProperty<int> GroupId = new Reactive.ReactiveProperty<int>(0);
        private bool isExpand;

        [UIOutlet("@[UIButton] Expand", true)]
        [SerializeField] private UIButton expandButton;

        private UICell[] Cells;
        private Func<Transform, UICell> cellFactory;
        private Action<UICell> cellCollector;

        /// <summary>
        /// Install binding on start.
        /// </summary>
        private void Start()
        {
            if (this.expandButton != null)
            {
                this.expandButton.OnClick += ExpandButton_OnClick;
            }
        }

        /// <summary>
        /// Uninstall binding on destroy.
        /// </summary>
        private void OnDestroy()
        {
            if (this.expandButton != null)
            {
                this.expandButton.OnClick += ExpandButton_OnClick;
            }
        }

        /// <summary>
        /// Group expand state changed.
        /// </summary>
        private void ExpandButton_OnClick()
        {
            if (this.State == STATE_EXPANDER)
            {
                this.isExpand = !this.isExpand;
                this.expandButton.State = isExpand ? STATE_BTN_COLLAPSE : STATE_BTN_EXPAND;
                this.OnExpandChanged?.Invoke(this.GroupId.Value, this.isExpand);
            }
        }

        /// <summary>
        /// Initialize ui record.
        /// </summary>
        public void Initialize(int cellCount, Func<Transform, UICell> cellFactory, Action<UICell> cellCollector)
        {
            this.Cells = new UICell[cellCount];
            this.cellFactory = cellFactory;
            this.cellCollector = cellCollector;
        }

        /// <summary>
        /// Collect all cells.
        /// </summary>
        public void CollectCells()
        {
            for (int i = 0; i < this.Cells.Length; i++)
            {
                if (this.Cells[i] != null)
                {
                    this.cellCollector(this.Cells[i]);
                    this.Cells[i] = null;
                }
            }
        }

        /// <summary>
        /// Update record cells data. create new cells and collect unuse cells if need.
        /// </summary>
        public void UpdateDataAsRecord(List<ICellData> cellData, Action<UICell, ICellData> raiseCellShowed)
        {
            if (cellData.Count > this.Cells.Length)
                throw new System.InvalidOperationException("Cells count is bigger than record max cell couunt");

            this.State = STATE_NORMAL;

            for (int i = 0; i < this.Cells.Length; i++)
            {
                //update cell's content.
                if (i < cellData.Count)
                {
                    if (this.Cells[i] == null)
                    {
                        this.Cells[i] = this.cellFactory(this.transform);
                    }

                    this.Cells[i].DataContext = cellData[i];
                    raiseCellShowed(this.Cells[i], cellData[i]);
                }

                //collect unuse cell.
                else
                {
                    if (this.Cells[i] != null)
                    {
                        this.cellCollector(this.Cells[i]);
                        this.Cells[i] = null;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateDataAsExpander(int groupId, bool isExpand)
        {
            this.State = STATE_EXPANDER;
            this.expandButton.State = isExpand ? STATE_BTN_COLLAPSE : STATE_BTN_EXPAND;
            this.GroupId.Value = groupId;
            this.isExpand = isExpand;

            //collect all cells.
            for (int i = 0; i < this.Cells.Length; i++)
            {
                if (this.Cells[i] != null)
                {
                    this.cellCollector(this.Cells[i]);
                    this.Cells[i] = null;
                }
            }
        }
    }
}
