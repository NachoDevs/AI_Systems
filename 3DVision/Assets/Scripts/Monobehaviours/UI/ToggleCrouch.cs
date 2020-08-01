using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI_Utilities
{
    public class ToggleCrouch : MonoBehaviour
    {
        public Animator Target;

        private bool isCrouching;

        /// <summary>
        /// Initialize
        /// </summary>
        private void Awake()
        {
            this.isCrouching = false;
        }

        /// <summary>
        /// Toggles the animator variable related to the crouch animation
        /// </summary>
        public void ToggleCrouching()
        {
            this.isCrouching = !this.isCrouching;

            this.Target.SetBool("IsCrouching", this.isCrouching);
        }
    }
}

