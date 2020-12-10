using Net.HungryBug.Core.Attribute;
using Net.HungryBug.Core.Reactive;
using Net.HungryBug.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Net.HungryBug.Core.UI.ReusableCollection
{
    [DisallowMultipleComponent]
    public abstract class UIScrollView : UIResolver
    {
        public event Action<UICell, ICellData> OnClicked;
        public event Action<UICell, ICellData> OnCellShowed;
        public readonly ReactiveCollection<ICellData> DataContext = new ReactiveCollection<ICellData>();



        [Header("[UIScrollView]")]
        [UIOutlet("@[UICell] Template")]
        [SerializeField] private UICell cellTemplate;

        [SerializeField] protected ScrollRect ScrollRect;

        [ReadOnly]
        [SerializeField] protected RectTransform Viewport;

        [UIOutlet("@[Content]", true)]
        [SerializeField] protected RectTransform ContentRect;

        [UIOutlet("@[Cache]", true)]
        [SerializeField] protected RectTransform Cache;

        [UIOutlet("@[Transfrom]Empty", true)]
        [SerializeField]
        private GameObject emptyIndicator;

        protected readonly HashSet<UICell> visibleCells = new HashSet<UICell>();
        protected GameObjectPool<UICell> UICellPool;
        private bool isInitialized = false;
        private bool isDirty = false;
        private bool bulkEdit = false;



        protected virtual void Awake()
        {
            this.cellTemplate.gameObject.SetActive(false);
            this.UICellPool = new GameObjectPool<UICell>(this.cellTemplate, this.ContentRect, 0, false, this.Cache);
            this.UICellPool.Initialize().WrapErrors();
        }

        protected virtual void Start()
        {
            //cell pool event hooks
            this.UICellPool.OnCreatedItem += this.UICellPool_OnCreatedItem;
            this.UICellPool.OnCollectedItem += this.UICellPool_OnCollectedItem;

            //raise before spawn default cells on start.
            this.OnStartBeforeSpawn();

            //for first time opened.
            if (this.DataContext.Count != 0)
            {
                for (int i = 0; i < this.DataContext.Count; i++)
                {
                    this.DoOnItemAdded(this.DataContext[i]);
                }
            }

            //collection modification event hooks.
            this.DataContext.OnItemAdded += this.DoOnItemAdded;
            this.DataContext.OnItemEdited += this.DoOnItemEdited;
            this.DataContext.OnItemRemoved += this.DoOnItemRemoved;

            this.isInitialized = true;
        }

        protected virtual void OnDestroy()
        {
            this.UICellPool.OnCreatedItem -= this.UICellPool_OnCreatedItem;
            this.UICellPool.OnCollectedItem -= this.UICellPool_OnCollectedItem;

            //collection modification event hooks.
            this.DataContext.OnItemAdded -= this.DoOnItemAdded;
            this.DataContext.OnItemEdited -= this.DoOnItemEdited;
            this.DataContext.OnItemRemoved -= this.DoOnItemRemoved;
        }

        protected virtual async void OnEnable()
        {
            this.ShowEmptyIndicator(false);
            try
            {
                await UniTask.Delay(500, cancellationToken: this.GetCancellationTokenOnDestroy(), ignoreTimeScale: true);
                this.ShowEmptyIndicator(this.DataContext.Count == 0);
            }
            catch (Exception)
            {
                return;
            }
        }

        protected virtual void OnDisable() { }



        /// <summary>
        /// Gets a visible cell with <see cref="ICellData"/> datacontext, if not found then return null.
        /// </summary>
        public virtual UICell GetVisibleCell(ICellData data)
        {
            foreach (var cell in this.visibleCells)
            {
                if (cell.DataContext == data)
                {
                    return cell;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets all visible cells.
        /// </summary>
        public UICell[] GetVisibleCells()
        {
            return this.visibleCells.ToArray();
        }

        protected virtual void OnStartBeforeSpawn() { }
        protected abstract void DataContext_SetData(IEnumerable<ICellData> data);
        protected abstract void DataContext_OnItemAdded(ICellData cell);
        protected abstract void DataContext_OnItemEdited(object sender, CollectionUpdateEventArgs<ICellData> e);
        protected abstract void DataContext_OnItemRemoved(ICellData cell);

        /// <summary>
        /// Raise on cell showed.
        /// On simple mode, this will be raised on cell has been created.
        /// On resuable mode, this will be raised whenever the cell has been created or reused.
        /// </summary>
        protected void RaiseOnCellShowed(UICell cell, ICellData data)
        {
            this.OnCellShowed?.Invoke(cell, data);
        }

        public void SetData(IEnumerable<ICellData> data)
        {
            this.bulkEdit = true;
            try
            {
                this.DataContext.Clear();
                this.DataContext.AddRange(data);

                if (this.isInitialized == true)
                    this.DataContext_SetData(data);
            }
            catch (Exception)
            {
            }

            this.bulkEdit = false;
        }

        /// <summary>
        /// Invoke whenever an cell is created.
        /// </summary>
        private void UICellPool_OnCreatedItem(UICell cell)
        {
            cell.ShowIndicator(false);
            this.visibleCells.Add(cell);
            cell.OnClicked += this.Cell_OnClicked;
        }

        /// <summary>
        /// Invoke whenever a cell is collected.
        /// </summary>
        private void UICellPool_OnCollectedItem(UICell cell)
        {
            cell.ShowIndicator(false);
            this.visibleCells.Remove(cell);
            cell.OnClicked -= this.Cell_OnClicked;
        }

        /// <summary>
        /// Raise <see cref="OnClicked"/> if the cell get selected.
        /// </summary>
        private void Cell_OnClicked(UICell cell, ICellData data)
        {
            this.OnClicked?.Invoke(cell, data);
        }

        private void DoOnItemAdded(ICellData cell)
        {
            if (this.bulkEdit == true)
                return;

            this.DataContext_OnItemAdded(cell);
        }
        private void DoOnItemEdited(object sender, CollectionUpdateEventArgs<ICellData> e)
        {
            if (this.bulkEdit == true)
                return;

            this.DataContext_OnItemEdited(sender, e);
        }

        private void DoOnItemRemoved(ICellData cell)
        {
            if (this.bulkEdit == true)
                return;

            this.DataContext_OnItemRemoved(cell);
        }

        /// <summary>
        /// Hide or show empty indicator.
        /// </summary>
        protected void ShowEmptyIndicator(bool show)
        {
            if (this.emptyIndicator != null)
            {
                this.emptyIndicator.SetActive(show);
            }
        }

#if UNITY_EDITOR
        protected override void Editor_CustomResolve()
        {
            base.Editor_CustomResolve();
            this.ScrollRect = this.GetComponent<ScrollRect>();
            if (this.ScrollRect != null)
            {
                this.Viewport = this.ScrollRect.viewport;

                if (this.ContentRect == null)
                {
                    this.ContentRect = this.ScrollRect.content.GetComponent<RectTransform>();
                }
            }
        }
#endif
    }
}
