using UnityEngine;

namespace Net.HungryBug.Core.UI
{
    public class GlobalUICoroutine : MonoBehaviour
    {
        public static GlobalUICoroutine _instance;
        public static GlobalUICoroutine Instance
        {
            get
            {
                if (_instance == null)
                {
                    var ga = new GameObject($"GlobalUICoroutine");
                    _instance = ga.AddComponent<GlobalUICoroutine>();
                    GameObject.DontDestroyOnLoad(ga);
                }

                return _instance;
            }
        }
    }
}
