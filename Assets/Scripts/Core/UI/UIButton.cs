using UnityEngine;

namespace Net.HungryBug.Core.UI
{
    using Net.HungryBug.Core.Attribute;
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class UIButton : UITouchCounter
    {
        const string stateActive = "@[State] Active";
        const string stateDeactive = "@[State] Deactive";

        [UIOutlet("@[Button] Select", true)]
        [SerializeField] private Button selectButton;
        private Button SelectButton { get { return this.selectButton = this.selectButton ?? this.GetComponent<Button>(); } }

#if UNITY_EDITOR
        public Button EDITOR_selectButton { get { return this.selectButton; } }
#endif

        private UnityEvent _onclick = null;
        /// <summary>
        /// Gets button click event.
        /// </summary>
        public UnityEvent onClick => this._onclick = this._onclick ?? new UnityEvent();

        /// <summary>
        /// Gets button's image.
        /// </summary>
        public Image image
        {
            get { return this.SelectButton.image; }
            set { this.SelectButton.image = value; }
        }

        /// <summary>
        /// Invoke whenever its button has been clicked.
        /// </summary>
        public event Action OnClick;

        /// <summary>
        /// Gets or sets <see cref="UIButton"/> interactable state.
        /// </summary>
        public bool interactable
        {
            get { return this.SelectButton.interactable; }
            set { this.SelectButton.interactable = value; }
        }

        protected virtual void Awake() { }

        protected virtual void Start() { }

        protected virtual void OnDestroy() { }

        /// <summary>
        /// Install bindings.
        /// </summary>
        protected virtual void OnEnable()
        {
            if (this.SelectButton == null)
                Debug.LogError("[UIButton] Missing button: ", this.gameObject);
            else
                this.SelectButton.onClick.AddListener(this.OnButtonClicked);
        }

        /// <summary>
        /// Uninstall bindings.
        /// </summary>
        protected virtual void OnDisable()
        {
            if(this.SelectButton != null)
                this.SelectButton.onClick.RemoveListener(this.OnButtonClicked);
        }

        /// <summary>
        /// Sets active state for ui button.
        /// Sets state to "@[State] Active" if value is true, otherwise sets to "@[State] Deactive".
        /// </summary>
        public void SetState(bool active)
        {
            string newState = active ? stateActive : stateDeactive;
            this.State = newState;
        }

        /// <summary>
        /// Invoke whenever its button has been clicked.
        /// </summary>
        protected virtual void OnButtonClicked()
        {
            if (this.interactable)
            {
                if (UITouchCounter.IsNextTouchValid())
                {
                    this.OnClick?.Invoke();
                    this._onclick?.Invoke();
                }
            }
        }

        protected bool SimulateClickNow()
        {
            if (this.interactable)
            {
                this.OnClick?.Invoke();
                this._onclick?.Invoke();
                return true;
            }
            else
            {
                return false;
            }
        }


#if UNITY_EDITOR
        protected override void Editor_CustomResolve()
        {
            base.Editor_CustomResolve();
            if (this.selectButton == null)
                this.selectButton = this.GetComponent<Button>();
        }
#endif
    }
}

