using Net.HungryBug.Core.Attribute;
using Net.HungryBug.Core.Reactive;
using Net.HungryBug.Core.Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Net.HungryBug.Core.UI.ReusableCollection
{
    [RequireComponent(typeof(RectTransform), typeof(ScrollRect))]
    [DisallowMultipleComponent]
    public sealed class UIReusableScrollView : UIScrollView
    {
        private enum Direction
        {
            Vertical,
            Horizontal
        }

        [Header("[ReusableScrollView]")]
        [SerializeField] private Direction scrollDirection;

        [UIOutlet("@[UIRecord] Template", true)]
        [SerializeField] private UIRecord recordTemplate;

        [Header("Record")]
        [SerializeField] private bool bestFit = false;
        [SerializeField] private int cellPerRecord = 1;
        public float recordSize = 100f;

        [Header("Cell")]
        public Vector2 spacing = new Vector2(20f, 20f);

        [Space]
        [SerializeField] private Vector2 cellSize = new Vector2(100, 100);


        [Space]
        [SerializeField] private RectOffset contentPadding;

        public float recordSpacing = 20f;
        private GameObjectPool<UIRecord> uiRecordPool;
        private bool isGrid = false;
        private Vector2 scrollPosition;
        private readonly LinkedList<UIRecordWrapper> records = new LinkedList<UIRecordWrapper>();
        private readonly List<List<ICellData>> cellByRecords = new List<List<ICellData>>();

        /// <summary>
        /// Initialize UICollection.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            //create UIRecord pool on grid mode.
            if (this.recordTemplate != null)
            {
                this.isGrid = true;
                this.recordTemplate.gameObject.SetActive(false);
                this.uiRecordPool = new GameObjectPool<UIRecord>(this.recordTemplate, this.ContentRect, 0, false);
                this.uiRecordPool.Initialize().WrapErrors();
            }
            else
            {
                this.isGrid = false;
            }

            //reset ContentRect.
            if (scrollDirection == Direction.Vertical)
            {
                this.ContentRect.anchorMin = Vector2.up;
                this.ContentRect.anchorMax = Vector2.one;
            }
            else if (scrollDirection == Direction.Horizontal)
            {
                this.ContentRect.anchorMin = Vector2.zero;
                this.ContentRect.anchorMax = Vector2.up;
            }

            this.ContentRect.anchoredPosition = Vector2.zero;
            this.ContentRect.sizeDelta = Vector2.zero;
            this.ScrollRect.onValueChanged.AddListener(OnScrolled);
        }

        protected override void OnStartBeforeSpawn()
        {
            base.OnStartBeforeSpawn();
            
            //calculate best fit.
            if (this.recordTemplate != null)
            {
                if (this.bestFit)
                {
                    this.cellPerRecord = this.FindBestFitCellPerRecord();
                }
                else if (this.cellPerRecord <= 0)
                {
                    this.cellPerRecord = 1;
                }
            }
            else
            {
                this.cellPerRecord = 1;
            }
        }

        protected override void DataContext_OnItemAdded(ICellData cell)
        {
            this.Rebuild();
        }

        protected override void DataContext_OnItemEdited(object sender, CollectionUpdateEventArgs<ICellData> e)
        {
            this.Rebuild();
        }

        protected override void DataContext_OnItemRemoved(ICellData cell)
        {
            this.Rebuild();
        }

        protected override void DataContext_SetData(IEnumerable<ICellData> data)
        {
            this.Rebuild();
        }

        private void Rebuild()
        {
            this.ReBuildCellDataByRecords();
            this.Refresh(false);
        }

        /// <summary>
        /// Reload collection.
        /// </summary>
        private void Refresh(bool isReset = false)
        {
            //this.ReBuildCellDataByRecords();
            Vector2 sizeDelta = this.ContentRect.sizeDelta;
            float contentSize = 0;
            for (int i = 0; i < this.cellByRecords.Count; i++)
            {
                contentSize += this.recordSize + (i > 0 ? this.recordSpacing : 0);
            }
            if (scrollDirection == Direction.Vertical)
            {
                contentSize += contentPadding.vertical;
                sizeDelta.y = contentSize > this.Viewport.rect.height ? contentSize : this.Viewport.rect.height;
            }
            else if (scrollDirection == Direction.Horizontal)
            {
                contentSize += contentPadding.horizontal;
                sizeDelta.x = contentSize > this.Viewport.rect.width ? contentSize : this.Viewport.rect.width;
            }

            this.ContentRect.sizeDelta = sizeDelta;

            if (isReset)
            {
                foreach (UIRecordWrapper record in records)
                {
                    record.CollectCells();
                    if (record.Record != null)
                    {
                        this.uiRecordPool.Collect(record.Record);
                    }
                }

                records.Clear();
                this.ScrollRect.normalizedPosition = this.ContentRect.anchorMin;
                this.ScrollRect.onValueChanged.Invoke(this.ScrollRect.normalizedPosition);
            }
            else
            {
                UpdateRecords();
                FillRecords();
            }
        }

        /// <summary>
        /// Reuse cell on scrolling.
        /// </summary>
        private void OnScrolled(Vector2 pos)
        {
            ReuseRecords(pos - scrollPosition);
            FillRecords();
            scrollPosition = pos;
        }

        /// <summary>
        /// Create a new record.
        /// </summary>
        private void CreateRecord(int index)
        {
            UIRecordWrapper record = null;
            if (this.isGrid)
            {
                var uiRecord = this.uiRecordPool.Provide();
                record = new UIRecordWrapper(uiRecord, this.cellPerRecord, this.UICellPool.Provide, this.UICellPool.Collect);
            }
            else
            {
                var uiCell = this.UICellPool.Provide(this.ContentRect);
                record = new UIRecordWrapper(uiCell, this.UICellPool.Provide, this.UICellPool.Collect);
            }

            record.SetAnchors(this.ContentRect.anchorMin, this.ContentRect.anchorMax);
            this.UpdateRecord(record, index);

            if (scrollDirection == Direction.Vertical)
            {
                record.Top = (records.Count > 0 ? records.Last.Value.Bottom - this.recordSpacing : -contentPadding.top);
                record.SetOffsetHorizontal(contentPadding.left, contentPadding.right);
            }
            else if (scrollDirection == Direction.Horizontal)
            {
                record.Left = (records.Count > 0 ? records.Last.Value.Right + this.recordSpacing : contentPadding.left);
                record.SetOffsetVertical(contentPadding.top, contentPadding.bottom);
            }

            records.AddLast(record);
        }

        /// <summary>
        /// Update record content.
        /// </summary>
        private void UpdateRecord(UIRecordWrapper record, int index)
        {
            record.DataIndex = index;
            if (record.DataIndex >= 0 && record.DataIndex < this.cellByRecords.Count)
            {
                //if (scrollDirection == Direction.Vertical)
                //{
                //    cell.Height = this.recordSize;
                //}
                //else if (scrollDirection == Direction.Horizontal)
                //{
                //    cell.Width = this.recordSize;
                //}

                var recordData = this.cellByRecords[record.DataIndex];
                record.UpdateData(recordData, this.RaiseOnCellShowed);
                record.Active = true;
            }
            else
            {
                record.Active = false;
            }
        }

        /// <summary>
        /// Sets records position on list.
        /// </summary>
        private void UpdateRecords()
        {
            if (records.Count == 0) return;

            LinkedListNode<UIRecordWrapper> node = records.First;
            this.UpdateRecord(node.Value, node.Value.DataIndex);
            node = node.Next;
            while (node != null)
            {
                this.UpdateRecord(node.Value, node.Previous.Value.DataIndex + 1);

                if (scrollDirection == Direction.Vertical)
                {
                    node.Value.Top = node.Previous.Value.Bottom - this.recordSpacing;
                    node.Value.SetOffsetHorizontal(contentPadding.left, contentPadding.right);
                }
                else if (scrollDirection == Direction.Horizontal)
                {
                    node.Value.Left = node.Previous.Value.Right + this.recordSpacing;
                    node.Value.SetOffsetVertical(contentPadding.top, contentPadding.bottom);
                }

                node = node.Next;
            }
        }

        /// <summary>
        /// Fill records content on scrolling or reloading.
        /// </summary>
        private void FillRecords()
        {
            if (records.Count == 0) this.CreateRecord(0);

            while (this.RecordsTailEdge() + this.recordSpacing <= this.ActiveTailEdge())
            {
                this.CreateRecord(records.Last.Value.DataIndex + 1);
            }
        }

        /// <summary>
        /// Adjust top or bottom cell for reuse.
        /// </summary>
        private void ReuseRecords(Vector2 scrollVector)
        {
            if (records.Count == 0) return;

            if (scrollDirection == Direction.Vertical)
            {
                if (scrollVector.y > 0)
                {
                    while (this.RecordsTailEdge() - this.recordSize >= this.ActiveTailEdge())
                    {
                        MoveRecordLastToFirst();
                    }
                }
                else if (scrollVector.y < 0)
                {
                    while (this.RecordsHeadEdge() + this.recordSize <= this.ActiveHeadEdge())
                    {
                        MoveRecordFirstToLast();
                    }
                }
            }
            else if (scrollDirection == Direction.Horizontal)
            {
                if (scrollVector.x > 0)
                {
                    while (this.RecordsHeadEdge() + this.recordSize <= this.ActiveHeadEdge())
                    {
                        MoveRecordFirstToLast();
                    }
                }
                else if (scrollVector.x < 0)
                {
                    while (this.RecordsTailEdge() - this.recordSize >= this.ActiveTailEdge())
                    {
                        MoveRecordLastToFirst();
                    }
                }
            }
        }

        /// <summary>
        /// Move the first record to last for reuse
        /// </summary>
        private void MoveRecordFirstToLast()
        {
            if (records.Count == 0) return;

            UIRecordWrapper firstRecord = records.First.Value;
            UIRecordWrapper lastRecord = records.Last.Value;
            this.UpdateRecord(firstRecord, lastRecord.DataIndex + 1);

            if (scrollDirection == Direction.Vertical)
            {
                firstRecord.Top = lastRecord.Bottom - this.recordSpacing;
                firstRecord.SetOffsetHorizontal(contentPadding.left, contentPadding.right);
            }
            else if (scrollDirection == Direction.Horizontal)
            {
                firstRecord.Left = lastRecord.Right + this.recordSpacing;
                firstRecord.SetOffsetVertical(contentPadding.top, contentPadding.bottom);
            }

            records.RemoveFirst();
            records.AddLast(firstRecord);
        }

        /// <summary>
        /// Move the last record to first for reuse.
        /// </summary>
        private void MoveRecordLastToFirst()
        {
            if (records.Count == 0) return;

            UIRecordWrapper lastRecord = records.Last.Value;
            UIRecordWrapper firstRecord = records.First.Value;
            UpdateRecord(lastRecord, firstRecord.DataIndex - 1);

            if (scrollDirection == Direction.Vertical)
            {
                lastRecord.Bottom = firstRecord.Top + this.recordSpacing;
                lastRecord.SetOffsetHorizontal(contentPadding.left, contentPadding.right);
            }
            else if (scrollDirection == Direction.Horizontal)
            {
                lastRecord.Right = firstRecord.Left - this.recordSpacing;
                lastRecord.SetOffsetVertical(contentPadding.top, contentPadding.bottom);
            }

            records.RemoveLast();
            records.AddFirst(lastRecord);
        }

        /// <summary>
        /// 
        /// </summary>
        private float ActiveHeadEdge()
        {
            if (scrollDirection == Direction.Vertical)
            {
                return this.ContentRect.anchoredPosition.y;
            }
            else if (scrollDirection == Direction.Horizontal)
            {
                return -this.ContentRect.anchoredPosition.x;
            }

            throw new System.InvalidOperationException($"Invalid ScrollDirection: {scrollDirection}");
        }

        /// <summary>
        /// 
        /// </summary>
        private float ActiveTailEdge()
        {
            if (scrollDirection == Direction.Vertical)
            {
                return this.ContentRect.anchoredPosition.y + this.Viewport.rect.height;
            }
            else if (scrollDirection == Direction.Horizontal)
            {
                return -this.ContentRect.anchoredPosition.x + this.Viewport.rect.width;
            }

            throw new System.InvalidOperationException($"Invalid ScrollDirection: {scrollDirection}");
        }

        /// <summary>
        /// 
        /// </summary>
        private float RecordsHeadEdge()
        {
            if (scrollDirection == Direction.Vertical)
            {
                return records.Count > 0 ? -records.First.Value.Top : contentPadding.top;
            }
            else if (scrollDirection == Direction.Horizontal)
            {
                return records.Count > 0 ? records.First.Value.Left : contentPadding.left;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        private float RecordsTailEdge()
        {
            if (scrollDirection == Direction.Vertical)
            {
                return records.Count > 0 ? -records.Last.Value.Bottom : contentPadding.bottom;
            }
            else if (scrollDirection == Direction.Horizontal)
            {
                return records.Count > 0 ? records.Last.Value.Right : contentPadding.right;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReBuildCellDataByRecords()
        {
            this.cellByRecords.Clear();
            List<ICellData> page = null;
            for (int i = 0; i < this.DataContext.Count; i++)
            {
                //next page.
                if (i % this.cellPerRecord == 0)
                {
                    if (page != null)
                    {
                        this.cellByRecords.Add(page);
                    }

                    page = new List<ICellData>();
                }

                page.Add(this.DataContext[i]);
            }

            //add the final rows if need.
            if (page != null && page.Count > 0)
            {
                this.cellByRecords.Add(page);
            }

            //hide or show empty indicator.
            this.ShowEmptyIndicator(this.cellByRecords.Count == 0);
        }

        /// <summary>
        /// Find the best fit cell per record for dynamic with grid view.
        /// </summary>
        private int FindBestFitCellPerRecord()
        {
            //determine collection spacing.
            var record = this.recordTemplate.GetComponent<RectTransform>();
            float cellInRecordSpacing = 20f;
            float cellInRecordSize = 150f;
            float cellInRecordPadding = 0f;
            float recordExpandSize = 150f;

            switch (scrollDirection)
            {
                case Direction.Vertical:
                    this.recordSpacing = this.spacing.y;
                    cellInRecordSpacing = this.spacing.x;
                    cellInRecordSize = this.cellSize.x;
                    cellInRecordPadding = this.contentPadding.left + this.contentPadding.right;
                    recordExpandSize = record.rect.width;
                    break;

                case Direction.Horizontal:
                    this.recordSpacing = this.spacing.x;
                    cellInRecordSpacing = this.spacing.y;
                    cellInRecordSize = this.cellSize.y;
                    cellInRecordPadding = this.contentPadding.top + this.contentPadding.bottom;
                    recordExpandSize = record.rect.height;
                    break;
            }

            return (int)((recordExpandSize - cellInRecordPadding + cellInRecordSpacing) / (cellInRecordSize + cellInRecordSpacing));
        }
    }
}