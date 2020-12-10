using Net.HungryBug.Core.Attribute;
using Net.HungryBug.Core.DI;
using Net.HungryBug.Core.Engine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Net.HungryBug.Core.UI.ReusableCollection
{
    [DisallowMultipleComponent]
    public class UICell : UIResolver
    {
        /// <summary>
        /// Invoke whenever the <see cref="selectButton"/> get clicked.
        /// </summary>
        public event Action<UICell, ICellData> OnClicked;

        [Header("UICell")]
        /// <summary>
        /// The select cell button.
        /// </summary>
        [UIOutlet("@[Button] Select", true)]
        [SerializeField] private Button selectButton;

        /// <summary>
        /// The select indicator.
        /// </summary>
        [UIOutlet("@[SelectIndicator]", true)]
        [SerializeField] private GameObject selectIndicator;

        /// <summary>
        /// 
        /// </summary>
        private bool isDirty = true;

        private ICellData dataContext;
        private Coroutine lazyRefreshCoroutine;

        /// <summary>
        /// Gets or sets UICell's datacontext.
        /// </summary>
        public ICellData DataContext
        {
            get { return this.dataContext; }
            set
            {
                this.dataContext = value;
                this.isDirty = true;

                //start lazy update cycle if need.
                if (this.lazyRefreshCoroutine == null)
                {
                    this.lazyRefreshCoroutine = GlobalUICoroutine.Instance.StartCoroutine(this.LazyRefreshCell());
                }
            }
        }

        /// <summary>
        /// Install hooks.
        /// </summary>
        protected virtual void OnEnable()
        {
            if (this.selectButton != null)
            {
                this.selectButton.onClick.AddListener(this.OnCellClicked);
            }
        }

        /// <summary>
        /// Uninstall hooks.
        /// </summary>
        protected virtual void OnDisable()
        {
            if (this.selectButton != null)
            {
                this.selectButton.onClick.RemoveListener(this.OnCellClicked);
            }
        }

        /// <summary>
        /// Start late update coroutine.
        /// </summary>
        protected virtual void Start()
        {
            //start lazy update cycle if need, the second condition may never be happend.
            if (this.isDirty)
            {
                this.lazyRefreshCoroutine = GlobalUICoroutine.Instance.StartCoroutine(this.LazyRefreshCell());
            }
        }

        /// <summary>
        /// UnInitialize.
        /// </summary>
        protected virtual void OnDestroy()
        {
            try
            {
                if (this.lazyRefreshCoroutine != null)
                {
                    GlobalUICoroutine.Instance.StopCoroutine(this.lazyRefreshCoroutine);
                }

                this.lazyRefreshCoroutine = null;
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Refresh Cell visualization.
        /// </summary>
        public virtual void Refresh()
        {
            if (this.dataContext != null)
            {
                this.ShowIndicator(this.dataContext.IsSelected);
            }
        }

        /// <summary>
        /// Hide or show the selection indicator.
        /// </summary>
        public void ShowIndicator(bool visible)
        {
            if (this.selectIndicator != null)
            {
                this.selectIndicator.SetActive(visible);
            }
        }

        /// <summary>
        /// Invoke the <see cref="OnClicked"/>.
        /// </summary>
        private void OnCellClicked()
        {
            this.OnClicked?.Invoke(this, this.dataContext);
        }

        /// <summary>
        /// Delay the cell refreshing to the end of frame to avoid call UGUI modification two times per frame.
        /// </summary>
        /// <returns></returns>
        private IEnumerator LazyRefreshCell()
        {
            while (this.isDirty)
            {
                yield return new WaitForEndOfFrame();
                if (this == null)
                    break;

                if (this.isDirty && this.gameObject.activeInHierarchy)
                {
                    this.Refresh();
                    this.isDirty = false;
                }
            }

            if(this.lazyRefreshCoroutine != null)
            {
                GlobalUICoroutine.Instance.StopCoroutine(this.lazyRefreshCoroutine);
                this.lazyRefreshCoroutine = null;
            }
        }

#if UNITY_EDITOR
        protected override void Editor_CustomResolve()
        {
            base.Editor_CustomResolve();
            if (this.selectButton == null)
            {
                this.selectButton = this.GetComponent<Button>();
            }
        }
#endif
    }
}
