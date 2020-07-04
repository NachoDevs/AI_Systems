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

            Vector3 newCenter = new Vector3(0,0, this.AwarenessRadius);
            awarenessCollider.center = newCenter;

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
        /// Action that takes place when an object is inside our trigger
        /// </summary>
        private void OnTriggerStay(Collider other)
        {
            /// Check if the collision has the AwarenessTarget component
            if (!other.TryGetComponent<AwarenessTarget>(out AwarenessTarget newTarget))
            {
                return;
            }

            /// If we don't have a target yet
            if (this.currTarget == null)
            {
                this.currTarget = newTarget;
            }
            else
            {
                if(!CanSee(newTarget.transform))
                {
                    return;
                }

                /// If we find some object with higher priority
                if(newTarget.Priority > this.currTarget.Priority)
                {
                    this.currTarget = newTarget;
                }
                else if(newTarget.Priority == this.currTarget.Priority)
                {
                    /// If they have the same priority, set the closest as the target
                    float targetDist    = Vector3.Distance(this.transform.position, this.currTarget.transform.position);
                    float newDist       = Vector3.Distance(this.transform.position, newTarget.transform.position);

                    /// If we are closer to the new object we set this as our target
                    if(newDist > targetDist)
                    {
                        this.currTarget = newTarget;
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

        ///<summary>
        /// Makes the character reset the position of the look target
        ///</summary>
        private void ResetLookDirection()
        {
            Vector3 newLookPos = this.transform.position + this.transform.forward * 2;
            newLookPos.y = defaultLookTargetOffset.y;
            this.LookTarget.transform.position = newLookPos;
        }
    }
}

