using Net.HungryBug.Core.Attribute;
using System.Collections.Generic;
using UnityEngine;

namespace Net.HungryBug.Galaxy.Network
{
    /// <summary>
    /// The <see cref="IHost"/> as a scriptable.
    /// </summary>
    [CreateAssetMenu(menuName = "Configs", fileName = "ConfigApi", order = 0)]
    public class HostAsset : ScriptableObject
    {
        /// <summary>
        /// This method must be call on Build post process to cleanup private information.
        /// </summary>
        public static void ProductionBuildPostProcess()
        {
#if BUILD_PRODUCTION && UNITY_EDITOR
            var host = UnityEditor.AssetDatabase.LoadAssetAtPath<HostAsset>("Assets/Resources/Host.asset");
            host.develop = null;
            host.staging = null;
            host.hosts = null;
            host.selectedIndex = -1;

            UnityEditor.EditorUtility.SetDirty(host);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }

        [System.Serializable]
        private class hostConfig
        {
            public string name;
            public string api;
            public string cdn;

            public string skymavis_api;
            public string coingecko_api;
        }

        private string configName;
        private string api;
        private string cdn;

        private string skymavis_api;
        private string coingecko_api;

        /// <summary>
        /// 
        /// </summary>
        public string Name { get { return this.configName; } }

        /// <summary>
        /// Gets the base url.
        /// </summary>
        public string Api { get { return this.api; } }

        /// <summary>
        /// 
        /// </summary>
        public string Cdn { get { return this.cdn; } }


        public string SkymavisApi { get { return this.skymavis_api; } }

        public string CoingeckoApi { get { return this.coingecko_api; } }

        [Header("BUILD_PRODUCTION")]
        [SerializeField]
        private hostConfig production;

        [Header("BUILD_DEVELOP")]
        [SerializeField]
        private hostConfig develop;

        [Header("BUILD_STAGING")]
        [SerializeField]
        private hostConfig staging;

        /// <summary>
        /// Initialize deafult host for all requests.
        /// </summary>
        private void OnEnable()
        {
            hostConfig useHost = null;
#if BUILD_PRODUCTION
            useHost = this.production;
#elif BUILD_STAGING
            useHost = this.staging;
#elif BUILD_DEVELOP
            useHost = this.develop;
#elif UNITY_EDITOR
            useHost = this.hosts != null && this.hosts.Length > 0 && selectedIndex >= 0 ? this.hosts[selectedIndex] : useHost = this.develop;
#else
            //default we're gonna use develop config.
            useHost = this.develop;
#endif
            this.configName = useHost.name;
            this.api = useHost.api;
            this.cdn = useHost.cdn;
            this.skymavis_api = useHost.skymavis_api;
            this.coingecko_api = useHost.coingecko_api;
        }

#if UNITY_EDITOR
        [Header("UNITY_EDITOR")]
        [ValueDropdown("availableHosts", "selectedIndex")]
        public ValueDropdownList SelectApi;

        [SerializeField]
        private hostConfig[] hosts;

        [HideInInspector]
        [SerializeField]
        private int selectedIndex;

        /// <summary>
        /// Gather values for <see cref="ValueDropdownList"/>.
        /// </summary>
        ValueDropdownItem[] availableHosts()
        {
            var items = new List<ValueDropdownItem>();
            if (this.hosts != null && this.hosts.Length > 0)
            {
                for (int i = 0; i < this.hosts.Length; i++)
                {
                    items.Add(new ValueDropdownItem(this.hosts[i].name, i));
                }
            }

            return items.ToArray();
        }
#endif
    }
}
