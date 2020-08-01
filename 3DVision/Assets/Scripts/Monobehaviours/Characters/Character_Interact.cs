using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class Character_Interact : MonoBehaviour
    {
        public float VisibilityThreshold;

        [Header("PriorityByVTargets")]
        [SerializeField]
        private List<Transform> Keys_VisionTargets;

        [SerializeField]
        private List<float> Values_Weights;

        private int visionRaycastLayerIndex;

        /// <summary>
        /// Initializer
        /// </summary>
        private void Awake()
        {
            if(this.Keys_VisionTargets.Count != Values_Weights.Count)
            {
                throw new UnityException("The amount of elements in the list of keys and values should be the same.");
            }

            this.visionRaycastLayerIndex = LayerMask.NameToLayer("Vision Raycast");
        }

        /// <summary>
        /// Checks if this character can be seen from a position
        /// </summary>
        public bool CanBeSeen(Vector3 spotterEyes, float visionDistance)
        {
            float spotingValue = 0;

            for (int vTargetIndex = 0; vTargetIndex < this.Keys_VisionTargets.Count; vTargetIndex++)
            {
                var target  = this.Keys_VisionTargets[vTargetIndex];
                var tWeight = this.Values_Weights[vTargetIndex];

                var lookDir = target.transform.position - spotterEyes;

                Ray ray = new Ray(spotterEyes, lookDir);
                bool isAHit =  false;
                if(Physics.Raycast(ray, out RaycastHit hit, visionDistance/*, ~this.visionRaycastLayerIndex*/));
                {
                    if(hit.collider.transform == target)
                    {
                        spotingValue += tWeight;
                        isAHit = true;
                    }
                }

                Debug.DrawLine
                (
                    spotterEyes, 
                    target.transform.position,
                    isAHit ? Color.green : Color.red,
                    1.9f
                );
            }

            return spotingValue >= this.VisibilityThreshold; 
        }
    }
}
