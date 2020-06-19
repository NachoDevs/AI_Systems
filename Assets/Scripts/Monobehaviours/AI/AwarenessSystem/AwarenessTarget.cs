using UnityEngine;

namespace Game_AI
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class AwarenessTarget : MonoBehaviour
    {
        public int Priority;
    }
}
