using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class Spotter : MonoBehaviour
    {
        [Header("Vision related")]
        public Transform MyEyes;

        public float VisionDistance;
        public float DotVisionLimit;

        [Space]
        
        public Character_Interact Target;

        public bool IsBlind;

        private GameObject exclamationSignPref;

        private float elapsedTime;
        private float spottingPeriod = 2;

        /// <summary>
        /// Initializer
        /// </summary>
        private void Awake()
        {
            IsBlind = false;

            this.exclamationSignPref = Resources.Load<GameObject>("Prefabs/ExclamationSign");
        }

        /// <summary>
        /// Action that takes place when the game starts
        /// </summary>
        private void Start()
        {
            this.elapsedTime = 0;
        }

        /// <summary>
        /// Action that takes place one every frame
        /// </summary>
        private void Update()
        {
            this.elapsedTime += Time.deltaTime;

            if(this.elapsedTime >= this.spottingPeriod)
            {
                SpotBehaviour();
                this.elapsedTime = 0;
            }
        }

        /// <summary>
        /// Tries to spot a target
        /// </summary>
        private void SpotBehaviour()
        {
            /// If the target is not infront of us
            if(!CanSee(this.Target.transform))
            {
                return;
            }

            bool spottedATarget = this.Target.CanBeSeen(this.MyEyes.transform.position, this.VisionDistance);

            /// If we can see enough of the target
            if(spottedATarget)
            {
                InstantiateSpottingFeedback();
            }
        }

        ///<summary>
        /// Checks if the object is in front of the spotter
        ///</summary>
        private bool CanSee(Transform target)
        {
            Vector3 heading = this.MyEyes.position - target.position;
            heading.Normalize();
            float dotProduct = Vector3.Dot(this.MyEyes.forward, heading);

            return dotProduct < this.DotVisionLimit;
        }

        /// <summary>
        /// Instantiates an exclamation sign on top of the spotter to show that a target
        ///     has been spotted
        /// </summary>
        private void InstantiateSpottingFeedback()
        {
            Vector3 signPos = this.transform.position;
            signPos.y += .5f;

            var sign = Instantiate
            (
                this.exclamationSignPref, 
                signPos,
                Quaternion.identity, 
                this.transform
            );

            Destroy(sign, this.spottingPeriod);
        }
    }
}

