using UnityEngine;
using UnityEngine.AI;

namespace Game_Characters
{
    public class Soldier_Interact : MonoBehaviour
    {
        public const float DEFAULT_UNIT_SPEED = 3.5f;
        
        private NavMeshAgent navAgent;

        private Soldier_Animation myAnimation;

        /// <summary>
        /// Initializer
        /// </summary>
        private void Awake()
        {
            this.navAgent = GetComponent<NavMeshAgent>();

            this.myAnimation = GetComponent<Soldier_Animation>();
        }

        /// <summary>
        /// Sets the new movement target posicion
        /// </summary>
        public void SetMovementTarget(Vector3 target)
        {
            this.myAnimation.StartRunAnimation();

            this.navAgent.speed = DEFAULT_UNIT_SPEED;
            
            this.navAgent.SetDestination(target);
        }
    }
}

