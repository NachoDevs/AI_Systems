using UnityEngine;

namespace Environment
{
    public class SoundSource : MonoBehaviour
    {
        public static SoundSourceEvent OnSoundEmited;

        public float LoudnessLevel;

        private Rigidbody myRB;

        private bool wasGrounded;

        /// <summary>
        /// Initializer
        /// </summary>
        private void Awake()
        {
            this.myRB = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Action that takes place every frame
        /// </summary>
        private void Update()
        {
            float yVel = Mathf.Abs(this.myRB.velocity.y);

            /// If the object has landed send the event that notifies so
            if(yVel == 0 && !this.wasGrounded)
            {
                OnSoundEmited?.Invoke(this.transform.position, LoudnessLevel);
            }

            this.wasGrounded = yVel == 0;
        }

        /// <summary>
        /// Places the sound source up 5 units, for testing purposes
        /// </summary>
        public void ThrowSoundSource()
        {
            var newPos = this.transform.position;
            newPos.y += 5;

            this.transform.position = newPos;
        }
    }
}

public delegate void SoundSourceEvent(Vector3 sourcePos, float loudnessLevel);
