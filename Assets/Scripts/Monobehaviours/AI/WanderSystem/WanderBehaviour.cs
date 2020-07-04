using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game_AI
{
    public class WanderBehaviour : MonoBehaviour
    {
        public List<WanderTarget> PossibleTargets;

        private Animator meshAnimator;

        private NavMeshAgent navAgent;

        private int targetIndex;

        /// <summary>
        /// Initializer
        /// </summary>
        private void Awake()
        {
            this.meshAnimator = this.transform.GetChild(0).GetComponent<Animator>();

            this.navAgent = GetComponent<NavMeshAgent>();
        }

        /// <summary>
        /// Action that takes place after enabling thie object
        /// </summary>
        private void Start()
        {
            SetNewTarget();
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
                    this.meshAnimator.SetFloat("MoveSpeed", 0);
                    SetNewTarget();
                }
            }
        }
        
        /// <summary>
        /// Searches for a new target to move to and sets the navMeshAgent target to it
        /// </summary>
        private void SetNewTarget()
        {
            if(this.PossibleTargets == null || this.PossibleTargets.Count == 0)
            {
                return;
            }

            /// Find a new target
            int newTargetIndex = Random.Range(0, this.PossibleTargets.Count);

            /// Do not repeat targets
            if(newTargetIndex == this.targetIndex)
            {
                newTargetIndex++;
                newTargetIndex %= this.PossibleTargets.Count;
            }

            /// Update new target
            this.targetIndex = newTargetIndex;

            /// Find targets position
            Vector3 targetPosition = this.PossibleTargets[newTargetIndex].transform.position;
            targetPosition.y = this.transform.position.y;

            /// Set NavMeshAgent target
            navAgent.SetDestination(targetPosition);

            /// Set the move speed to represent we are walking
            this.meshAnimator.SetFloat("MoveSpeed", 1);
        }

    }
}

