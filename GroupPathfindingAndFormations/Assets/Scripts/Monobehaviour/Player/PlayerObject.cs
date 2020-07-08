using System.Collections;
using System.Collections.Generic;
using Game_AI;
using UnityEngine;

namespace Player
{
    public class PlayerObject : MonoBehaviour
    {
        public List<GameObject> MySoldiers;

        private GameObject pointerClickPref;

        private GameObject prevPointerClick;

        private GroupController myGroupController;

        /// <summary>
        /// Initializer
        /// </summary>
        private void Awake()
        {
            this.pointerClickPref = Resources.Load<GameObject>("Prefabs/PointerClick");
            this.myGroupController = GetComponent<GroupController>();
        }

                /// <summary>
        /// Action that takes place at the start of the game
        /// </summary>
        private void Start() 
        {
            this.myGroupController.SetNewGroup(this.MySoldiers);
        }

        /// <summary>
        /// Action that takes places once every update
        /// </summary>
        private void Update() 
        {
            if(Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if(Physics.Raycast(ray, out RaycastHit hit))
                {
                    this.myGroupController.MoveGroup(hit.point);

                    PlacePointerClick(hit.point);
                }
            }
        }

        /// <summary>
        /// Instantiates a temporary object that shows where the click was done
        /// </summary>
        private void PlacePointerClick(Vector3 position)
        {
            /// If the other pointerClick still exists, destroy it
            if(this.prevPointerClick)
            {
                Destroy(this.prevPointerClick);
            }

            var pointerClick = Instantiate
            (
                this.pointerClickPref, 
                position, 
                Quaternion.identity,
                this.transform
            );

            this.prevPointerClick = pointerClick;

            /// Destroy this pointerClick after 2 seconds
            Destroy(pointerClick, 2);
        }
    }
}

