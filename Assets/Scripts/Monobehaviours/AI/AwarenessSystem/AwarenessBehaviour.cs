using UnityEngine;

namespace Game_AI
{
    public class AwarenessBehaviour : MonoBehaviour
    {
        public Transform LookTarget;

        public float AwarenessRadius;

        public Vector2 VisionLimit;

        private AwarenessTarget currTarget;

        private Vector3 defaultLookTargetOffset;

        /// <summary>
        /// Initializer
        /// </summary>
        private void Awake()
        {
            /// Add a collider for awareness detection
            SphereCollider awarenessCollider = this.gameObject.AddComponent<SphereCollider>();
            awarenessCollider.isTrigger = true;
            awarenessCollider.radius = this.AwarenessRadius;

            this.defaultLookTargetOffset = LookTarget.transform.localPosition;
        }

        /// <summary>
        /// Action that takes places once every physiscs update
        /// </summary>
        private void FixedUpdate()
        {
            if(this.currTarget != null)
            {
                Vector3 targetPos = this.currTarget.transform.position;

                float dotProduct = Vector3.Dot(targetPos, this.transform.right);

                if(dotProduct < this.VisionLimit.x || dotProduct > this.VisionLimit.y)
                {
                    ResetLookDirection();
                    return;
                }

                this.LookTarget.transform.position = targetPos;
            }
        }

        /// <summary>
        /// Action that takes place when an object enters our trigger
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            /// Check if the collision has the AwarenessTarget component
            if (!other.TryGetComponent<AwarenessTarget>(out AwarenessTarget target))
            {
                return;
            }

            /// If we dont have a target yet
            if (this.currTarget == null)
            {
                this.currTarget = target;
            }
            else
            {
                /// If we find some object with higher priority
                if(target.Priority > this.currTarget.Priority)
                {
                    this.currTarget = target;
                }
                else if(target.Priority == this.currTarget.Priority)
                {
                    float targetDist    = Vector3.Distance(this.transform.position, this.currTarget.transform.position);
                    float newDist       = Vector3.Distance(this.transform.position, target.transform.position);

                    /// If we are closer to the new object we set this as our target
                    if(newDist > targetDist)
                    {
                        this.currTarget = target;
                    }
                }
            }
        }

        /// <summary>
        /// Action that takes place when an object enters our trigger
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            /// Check if the collision has the AwarenessTarget component
            if (!other.TryGetComponent<AwarenessTarget>(out AwarenessTarget target))
            {
                return;
            }

            if(target == this.currTarget)
            {
                ResetLookDirection();
                this.currTarget = null;
            }
        }

        private void ResetLookDirection()
        {
            Vector3 newLookPos = this.transform.position + this.transform.forward * 2;
            newLookPos.y = defaultLookTargetOffset.y;
            this.LookTarget.transform.position = newLookPos;
        }
    }
}

