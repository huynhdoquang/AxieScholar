using System;
using System.Collections.Generic;
using UnityEngine;

namespace Net.HungryBug.Core.UI.ReusableCollection
{
    public sealed class UIRecordWrapper
    {
        private enum Mode
        {
            List,
            Grid,
        }

        public readonly UIRecord Record;
        public readonly UICell Cell;
        private readonly Func<Transform, UICell> cellFactory;
        private readonly Action<UICell> cellCollector;

        private Mode CollectionMode;
        private RectTransform rectTransform;
        public int DataIndex;

        /// <summary>
        /// Gets the object container.
        /// </summary>
        private GameObject Container
        {
            get { return this.CollectionMode == Mode.Grid ? this.Record.gameObject : this.Cell.gameObject; }
        }

        /// <summary>
        /// Active or deactive record.
        /// </summary>
        public bool Active
        {
            get { return this.Container.activeSelf; }
            set
            {
                if (this.Container.activeSelf != value)
                {
                    this.Container.SetActive(value);
                }
            }
        }

        /// <summary>
        /// Create a record container from a <see cref="UIRecord"/>.
        /// </summary>
        public UIRecordWrapper(UIRecord record, int cellPerRecord, Func<Transform, UICell> cellFactory, Action<UICell> cellCollector)
        {
            this.Record = record;
            this.CollectionMode = Mode.Grid;
            this.cellFactory = cellFactory;
            this.cellCollector = cellCollector;

            this.rectTransform = this.Record.GetComponent<RectTransform>();
            this.Record.Initialize(cellPerRecord, cellFactory, cellCollector);
        }

        /// <summary>
        /// Create a record container from a <see cref="UICell"/>
        /// </summary>
        public UIRecordWrapper(UICell cell, Func<Transform, UICell> cellFactory, Action<UICell> cellCollector)
        {
            this.Cell = cell;
            this.rectTransform = this.Cell.GetComponent<RectTransform>();
            this.CollectionMode = Mode.List;
            this.cellFactory = cellFactory;
            this.cellCollector = cellCollector;
        }

        /// <summary>
        /// Update record data.
        /// </summary>
        public void UpdateData(List<ICellData> cellData, Action<UICell, ICellData> raiseCellShowed)
        {
            switch (this.CollectionMode)
            {
                case Mode.List:
                    this.Cell.DataContext = cellData[0];
                    raiseCellShowed(this.Cell, cellData[0]);
                    break;

                case Mode.Grid:
                    this.Record.UpdateDataAsRecord(cellData, raiseCellShowed);
                    break;
            }
        }

        /// <summary>
        /// Update record data.
        /// </summary>
        public void UpdateDataAsExpander(int groupId, bool isExpanding)
        {
            if (this.CollectionMode == Mode.Grid)
            {
                if (this.Record != null)
                {
                    this.Record.UpdateDataAsExpander(groupId, isExpanding);
                }
            }
        }

        /// <summary>
        /// Collect all cells.
        /// </summary>
        public void CollectCells()
        {
            switch (this.CollectionMode)
            {
                case Mode.List:
                    this.cellCollector(this.Cell);
                    break;

                case Mode.Grid:
                    this.Record.CollectCells();
                    break;
            }
        }

        public void SetAnchors(Vector2 min, Vector2 max)
        {
            rectTransform.anchorMin = min;
            rectTransform.anchorMax = max;
        }

        public void SetOffsetVertical(float top, float bottom)
        {
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, bottom);
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -top);
        }

        public void SetOffsetHorizontal(float left, float right)
        {
            rectTransform.offsetMin = new Vector2(left, rectTransform.offsetMin.y);
            rectTransform.offsetMax = new Vector2(-right, rectTransform.offsetMax.y);
        }

        public float Width
        {
            get
            {
                return rectTransform.sizeDelta.x;
            }
            set
            {
                Vector2 sizeDelta = rectTransform.sizeDelta;
                sizeDelta.x = value;
                rectTransform.sizeDelta = sizeDelta;
            }
        }

        public float Height
        {
            get
            {
                return rectTransform.sizeDelta.y;
            }
            set
            {
                Vector2 sizeDelta = rectTransform.sizeDelta;
                sizeDelta.y = value;
                rectTransform.sizeDelta = sizeDelta;
            }
        }

        public float Left
        {
            get
            {
                Vector3[] corners = new Vector3[4];
                rectTransform.GetLocalCorners(corners);
                return rectTransform.anchoredPosition.x + corners[0].x;
            }
            set
            {
                Vector3[] corners = new Vector3[4];
                rectTransform.GetLocalCorners(corners);
                rectTransform.anchoredPosition = new Vector2(value - corners[0].x, 0);
            }
        }

        public float Top
        {
            get
            {
                Vector3[] corners = new Vector3[4];
                rectTransform.GetLocalCorners(corners);
                return rectTransform.anchoredPosition.y + corners[1].y;
            }
            set
            {
                Vector3[] corners = new Vector3[4];
                rectTransform.GetLocalCorners(corners);
                rectTransform.anchoredPosition = new Vector2(0, value - corners[1].y);
            }
        }

        public float Right
        {
            get
            {
                Vector3[] corners = new Vector3[4];
                rectTransform.GetLocalCorners(corners);
                return rectTransform.anchoredPosition.x + corners[2].x;
            }
            set
            {
                Vector3[] corners = new Vector3[4];
                rectTransform.GetLocalCorners(corners);
                rectTransform.anchoredPosition = new Vector2(value - corners[2].x, 0);
            }
        }

        public float Bottom
        {
            get
            {
                Vector3[] corners = new Vector3[4];
                rectTransform.GetLocalCorners(corners);
                return rectTransform.anchoredPosition.y + corners[3].y;
            }
            set
            {
                Vector3[] corners = new Vector3[4];
                rectTransform.GetLocalCorners(corners);
                rectTransform.anchoredPosition = new Vector2(0, value - corners[3].y);
            }
        }
    }
}
