using UnityEngine;

namespace Game_AI
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class AwarenessTarget : MonoBehaviour
    {
        [Tooltip("The level of priority used when a character decides which object to look at.")]
        [Range(0, 10)]
        public int Priority = 0;

        /// Just to update it at least as possible, not really needed for the awareness system
        private int prevPriority = -1;

        /// <summary>
        /// Action that takes places once every update
        /// </summary>
        private void Update()
        {
            if(this.Priority != this.prevPriority)
            {
                MatchMaterial();
            }
        }

        /// <summary>
        /// Updates the material to match the current priority level 
        /// </summary>
        private void MatchMaterial()
        {
                /// Update the value
                this.prevPriority = this.Priority;

                var renderer = GetComponent<MeshRenderer>();

                /// Get the current material
                Material newMat = renderer.material;

                /// If the material is null, return
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
