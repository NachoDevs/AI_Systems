using UnityEngine;

namespace Game_AI
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class AwarenessTarget : MonoBehaviour
    {
        [Range(0, 10)]
        public int Priority = 0;

        /// Just to update it at least as possible, not really needed for the awareness system
        private int prevPriority = -1;

        private void Update()
        {
            if(this.Priority != this.prevPriority)
            {
                /// Update the value
                this.prevPriority = this.Priority;

                var renderer = GetComponent<MeshRenderer>();

                /// Get the current material
                Material newMat = renderer.material;

                if(!newMat)
                {
                    return;
                }

                /// Calculate the level of redness based on the max of the values of priority
                float nonRedValue = Priority / 10.0f;
                nonRedValue = Mathf.Abs(1 - nonRedValue);

                /// Change the material's color
                newMat.color = new Color(1, nonRedValue, nonRedValue, 1);

                /// Change the renderer material
                renderer.material = newMat;
            }
        }
    }
}
