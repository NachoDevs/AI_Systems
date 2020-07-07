using UnityEngine;
using UnityEngine.AI;

namespace Game_Characters
{
    public class Soldier_Interact : MonoBehaviour
    {
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
            this.navAgent.SetDestination(target);
        }

        private void Update() 
        {
            if(Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if(Physics.Raycast(ray, out RaycastHit hit))
                {
                    SetMovementTarget(hit.point);
                }
            }
        }
    }
}

