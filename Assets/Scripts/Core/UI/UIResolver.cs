using Net.HungryBug.Core.Utility;
using System.Collections.Generic;
using UnityEngine;

using Net.HungryBug.Core.Attribute;
using System.Linq;
using System.Reflection;
using UnityEngine.UI;

namespace Net.HungryBug.Core.UI
{
    public class UIResolver : MonoBehaviour
    {
        [System.Serializable]
        protected class SubViewStorage : SerializableDictionary<string, GameObject> { }

        [Header("UI Resolver")]
        [ReadOnly]
        [SerializeField]
        protected SubViewStorage subViewStorage = new SubViewStorage();

        [ReadOnly]
        [SerializeField]
        protected List<GameObject> subStates = new List<GameObject>();

        /// <summary>
        /// Sets the <see cref="UIView"/> state.
        /// </summary>
        private string state = null;
        public string State
        {
            get { return state; }
            set
            {
                if (this.state == value)
                    return;

                state = value;
                foreach (var item in this.subStates)
                    item.SetActive(item.name.Equals(value));
            }
        }

        /// <summary>
        /// Find the first sub view match the name.
        /// </summary>
        public virtual GameObject FindSubView(string name)
        {
            GameObject sub = null;
            this.subViewStorage.TryGetValue(name, out sub);
            return sub;
        }

        /// <summary>
        /// Find the first sub view with name.
        /// </summary>
        public virtual T FindSubView<T>(string name) where T : Component
        {
            GameObject sub = null;
            if (this.subViewStorage.TryGetValue(name, out sub))
            {
                return sub.GetComponent<T>();
            }

            return null;
        }

        #region [Editor Utilities]
#if UNITY_EDITOR
        [ContextMenu("Prepare deep")]
        public void DeepPrepare()
        {
            var resolvers = new List<UIResolver>() { this };
            this.RefreshStorages(true, resolvers);
            for (int i = 0; i < resolvers.Count; i++)
            {
                resolvers[i].DoResolveOutlets();
                UnityEditor.EditorUtility.SetDirty(resolvers[i].gameObject);
            }
        }

        [ContextMenu("Prepare self")]
        public void Prepare()
        {
            var resolvers = new List<UIResolver>() { this };
            this.RefreshStorages(false, resolvers);
            for (int i = 0; i < resolvers.Count; i++)
            {
                resolvers[i].DoResolveOutlets();
                UnityEditor.EditorUtility.SetDirty(resolvers[i].gameObject);
            }
        }

        protected virtual void Editor_CustomResolve() { }

        private void DoResolveOutlets()
        {
            this.ResolveOutlets(new UIResolver[] { this });
            this.Editor_CustomResolve();


            /*if (this is UIController)
            {
                var controller = this as UIController;

                //find default name
                if (string.IsNullOrEmpty(controller.ControllerName))
                    controller.ControllerName = controller.GetType().Name;

                //find view
                var newView = controller.FindSubView<UIView>("@[View]");

                if (newView == null)
                {
                    newView = controller.FindSubView<UIView>("@[UIView]");
                }

                if (newView == null)
                {
                    foreach (var pair in controller.subViewStorage)
                    {
                        if (pair.Key.StartsWith("@[UIView]"))
                        {
                            newView = pair.Value.GetComponent<UIView>();
                            break;
                        }
                    }
                }

                if (newView != null)
                {
                    controller.View = newView;
                }

                //resolve outlet.
                this.ResolveOutlets(new UIResolver[] { controller, controller.View });
            }
            else
            {
                this.ResolveOutlets(new UIResolver[] { this });
            }

            this.Editor_CustomResolve();*/
        }

        /// <summary>
        /// Refresh prepared mapped objects.
        /// </summary>
        private void RefreshStorages(bool deep, List<UIResolver> allResolvers)
        {
            //reset the last construct.
            this.subViewStorage.Clear();
            this.subStates.Clear();

            //search for subview.
            this.SearchForSubViews(this.transform, this.subViewStorage, deep, allResolvers);
        }

        /// <summary>
        /// Gets all valid sub views.
        /// </summary>
        private void SearchForSubViews(Transform parent, SubViewStorage data, bool deepSearch, List<UIResolver> allResolvers)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                var tran = parent.GetChild(i);

                //Add a new subview
                if (tran.name.StartsWith("@"))
                    this.AddSubViewData(tran.gameObject, tran.name);

                //skip GoView's children.
                if (tran.GetComponent<UIResolver>() != null)
                {
                    //PreConstruct its sub if running in editor mode.
                    if (deepSearch)
                    {
                        var child = tran.GetComponent<UIResolver>();
                        allResolvers.Add(child);
                        child.RefreshStorages(deepSearch, allResolvers);
                    }
                    continue;
                }

                this.SearchForSubViews(tran, data, deepSearch, allResolvers);
            }
        }

        /// <summary>
        /// Add a game object to <see cref="dataContext"/>
        /// </summary>
        private void AddSubViewData(GameObject view, string name)
        {
            if (!this.subViewStorage.Contains(name))
            {
                this.subViewStorage.Add(name, view);

                //Update states list.
                if (view.name.StartsWith("@[State]"))
                {
                    this.subStates.Add(view);
                }
            }
        }

        /// <summary>
        /// Construct outlets.
        /// </summary>
        private void ResolveOutlets(UIResolver[] sources)
        {
            var currentType = this.GetType();
            while (currentType != typeof(UIResolver))
            {
                var fields = currentType.GetFields(
                                            BindingFlags.Instance |
                                            BindingFlags.NonPublic |
                                            BindingFlags.Public);

                foreach (var field in fields)
                {
                    if (field.CustomAttributes == null || field.CustomAttributes.Count() == 0)
                        continue;

                    foreach (var outlet in field.CustomAttributes)
                    {
                        if (outlet.AttributeType == typeof(UIOutletAttribute))
                        {
                            string targetObjectName = (string)outlet.ConstructorArguments[0].Value;
                            bool optionalOutlet = (bool)outlet.ConstructorArguments[1].Value;

                            GameObject result = null;

                            //find full name.
                            foreach (var s in sources)
                            {
                                if (s == null)
                                    continue;

                                if (result != null)
                                    break;

                                result = s.FindSubView(targetObjectName);
                            }

                            //find ind child by part.
                            if (result == null && targetObjectName.Contains("."))
                            {
                                var paths = targetObjectName.Split('.');
                                foreach (var s in sources)
                                {
                                    if (s == null)
                                        continue;

                                    UIResolver searchFrom = s;
                                    for (int i = 0; i < paths.Length; i++)
                                    {
                                        if (i == paths.Length - 1)
                                        {
                                            result = searchFrom.FindSubView(paths[i]);
                                        }
                                        else
                                        {
                                            searchFrom = searchFrom.FindSubView<UIResolver>(paths[i]);
                                            if (searchFrom == null)
                                                break;
                                        }
                                    }
                                }
                            }


                            if (result == null)
                            {
                                if (!optionalOutlet)
                                {
                                    Debug.LogErrorFormat($"{this.gameObject.name}: {this.GetType().Name}, {targetObjectName} not found");
                                }
                            }
                            else
                            {
                                if (field.FieldType == typeof(GameObject))
                                {
                                    field.SetValue(this, result);

                                }
                                else
                                {
                                    var component = result.GetComponent(field.FieldType);
                                    if (component == null)
                                    {
                                        if (!optionalOutlet)
                                            Debug.LogErrorFormat($"{this.gameObject.name}: {this.GetType().Name}: missing component {field.FieldType.Name} on {targetObjectName}");
                                    }
                                    else
                                    {
                                        field.SetValue(this, component);
                                    }
                                }
                            }

                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }

                currentType = currentType.BaseType;
            }
        }
#endif 
        #endregion
    }
}