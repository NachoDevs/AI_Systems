using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Characters
{
    public class Mutant_Interact : MonoBehaviour
    {
        public NavMeshAgent soundNavAgent;

        public float HearingSensibility;

        private Mutant_Animation mAnim;

        private NavMeshAgent myNavAgent;

        private NavMeshPath soundPath;

        /// <summary>
        /// Initializer
        /// </summary>
        private void Awake()
        {
            Environment.SoundSource.OnSoundEmited += CheckSound;

            this.myNavAgent = GetComponent<NavMeshAgent>();
            this.mAnim = GetComponent<Mutant_Animation>();
        }
        private void Update()
        {
            /// Since the sound agent has also a navmesh agent, we need to update its position
            ///     to follow the parent. It is placed behind to avoid both agents colliding
            /// I could not find a way to make nav agents of differenty types not collide between
            ///     each other, if anybody knows please contact me! Because this is not a solition
            ///     ready for production, that is for sure!
            var soundNavAgentPos = Vector3.zero;
            soundNavAgentPos.z -= this.myNavAgent.radius + this.soundNavAgent.radius;
            this.soundNavAgent.transform.localPosition = soundNavAgentPos;
        }

        /// <summary>
        /// Checks if a sound is on a distance that can be detected by the character
        /// </summary>
        private void CheckSound(Vector3 soundPosition, float LoudnessLevel)
        {
            this.soundNavAgent.ResetPath();

            this.soundPath = new NavMeshPath();
            this.soundNavAgent.CalculatePath(soundPosition, this.soundPath);

            float? pathDistance = ComputeSoundPathLength();

            /// If no path was found
            if(pathDistance == null)
            {
                return;
            }

            print("Sound travel distance: " + pathDistance);

            /// If the sound is close enough
            if((pathDistance / LoudnessLevel) < this.HearingSensibility)
            {
                StartCoroutine(InvestigateSound(soundPosition));
            }
        }

        /// <summary>
        /// Computes the length of the path to the sound
        /// </summary>
        private float? ComputeSoundPathLength()
        {
            float? length;  /// Nullable float

            if(this.soundPath != null && this.soundPath.corners.Length > 0)
            {
                length = 0;
                for (int cornerIndex = 1; cornerIndex < this.soundPath.corners.Length; cornerIndex++)
                {
                    float newDist = Vector3.Distance(this.soundPath.corners[cornerIndex - 1], this.soundPath.corners[cornerIndex]);
                    length += newDist;
                }
            }
            else
            {
                return null;
            }

            return length;
        }

        /// <summary>
        /// Sequence of actions that take place after hearing a sound
        /// </summary>
        private IEnumerator InvestigateSound(Vector3 targetPos)
        {
            this.mAnim.ReactToSound();

            /// Duration of the 'react' animation
            yield return new WaitForSeconds(2f);

            this.myNavAgent.SetDestination(targetPos);
        }

        /// <summary>
        /// Action that takes place when this object is destroyed
        /// </summary>
        private void OnDestroy()
        {
            Environment.SoundSource.OnSoundEmited -= CheckSound;
        }

        private void OnDrawGizmos()
        {
            if(soundPath != null)
            {
                for (int cornerIndex = 1; cornerIndex < this.soundPath.corners.Length; cornerIndex++)
                {
                    Gizmos.DrawLine(this.soundPath.corners[cornerIndex - 1], this.soundPath.corners[cornerIndex]);
                }
            }
        }
    }
}
