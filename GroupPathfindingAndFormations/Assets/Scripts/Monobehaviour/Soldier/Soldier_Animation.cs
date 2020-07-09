using UnityEngine;
using UnityEngine.AI;

namespace Game_Characters
{
    public class Soldier_Animation : MonoBehaviour
    {
        private Animator myAnimator;
        
        private NavMeshAgent navAgent;

        /// <summary>
        /// Initializer
        /// </summary>
        private void Awake()
        {
            this.myAnimator = GetComponent<Animator>();
            
            this.navAgent = GetComponent<NavMeshAgent>();
        }

        /// <summary>
        /// Action that takes places once every physiscs update
        /// </summary>
        private void FixedUpdate()
        {
            /// If we are moving
            if(navAgent.velocity.magnitude > 0)
            {
                /// If have reached our destination
                if(navAgent.remainingDistance <= navAgent.stoppingDistance)
                {
                    StartIdleAnimation();
                }
            }
        }

        /// <summary>
        /// Sets the animator movement float to 0
        /// </summary>
        public void StartIdleAnimation()
        {
            this.myAnimator.SetFloat("MoveSpeed", 0);
        }

        /// <summary>
        /// Sets the animator movement float to 1
        /// </summary>
        public void StartRunAnimation()
        {
            this.myAnimator.SetFloat("MoveSpeed", 1);
        }

        private void OnDrawGizmos() 
        {
            if(UnityEditor.EditorApplication.isPlaying)
            {
                if(this.navAgent.acceleration > 0)
                {
                    for (int pointIndex = 0; pointIndex < this.navAgent.path.corners.Length - 1; pointIndex++)
                    {
                        Gizmos.DrawLine(this.navAgent.path.corners[pointIndex], this.navAgent.path.corners[pointIndex + 1]);
                    }
                }
            }
        }
    }
}