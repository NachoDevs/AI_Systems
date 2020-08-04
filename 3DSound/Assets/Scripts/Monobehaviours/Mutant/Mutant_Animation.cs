using UnityEngine;
using UnityEngine.AI;

namespace Characters
{
    public class Mutant_Animation : MonoBehaviour
    {
        private Animator myAnimator;

        private NavMeshAgent myNavAgent;

        private Transform myMesh;

        /// <summary>
        /// Initializador
        /// </summary>
        private void Awake()
        {
            this.myNavAgent = GetComponent<NavMeshAgent>();

            this.myMesh = this.transform.GetChild(0);
            this.myAnimator = this.myMesh.GetComponent<Animator>();
        }

        /// <summary>
        /// Action that takes place every frame
        /// </summary>
        private void Update()
        {
            float currSpeed = this.myNavAgent.velocity.magnitude;
            this.myAnimator.SetFloat("MoveSpeed", currSpeed);
        }

        /// <summary>
        /// Action that takes place when a sound is heard by the mutant
        /// </summary>
        public void ReactToSound()
        {
            this.myAnimator.SetTrigger("OnSoundHeard");
        }
    }
}
