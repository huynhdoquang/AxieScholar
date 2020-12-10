using UnityEngine;

namespace Net.HungryBug.Core.UI
{
    public class UIRotate : MonoBehaviour
    {
        [SerializeField] private Vector3 rotateVelocity = new Vector3(0, 0, 35);

        void Update()
        {
            transform.Rotate(rotateVelocity * Time.unscaledDeltaTime);
        }
    }
}
