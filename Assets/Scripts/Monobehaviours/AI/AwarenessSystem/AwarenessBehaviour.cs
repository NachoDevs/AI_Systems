using UnityEngine;

namespace Game_AI
{
    public class AwarenessBehaviour : MonoBehaviour
    {
        public Transform LookTarget;

        public float AwarenessRadius;
        public float DotVisionLimit;

        public AwarenessTarget currTarget;

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
                if(!CanSee(this.currTarget.transform))
                {
                    ResetLookDirection();
                    return;
                }

                Vector3 targetPos = this.currTarget.transform.position;
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

            if(!CanSee(other.transform))
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

        ///<summary>
        /// Checks if the object is in front of the player
        ///</summary>
        private bool CanSee(Transform target)
        {
            Vector3 heading = this.transform.position - target.position;
            heading.Normalize();
            float dotProduct = Vector3.Dot(this.transform.forward, heading);

            return dotProduct < this.DotVisionLimit;
        }

        private void ResetLookDirection()
        {
            Vector3 newLookPos = this.transform.position + this.transform.forward * 2;
            newLookPos.y = defaultLookTargetOffset.y;
            this.LookTarget.transform.position = newLookPos;
        }
    }
}

