using System.Collections;
using System.Collections.Generic;
using Game_AI;
using UnityEngine;

namespace Player
{
    public class PlayerObject : MonoBehaviour
    {
        public List<GameObject> MySoldiers;

        private GroupController myGroupController;

        /// <summary>
        /// Initializer
        /// </summary>
        private void Awake()
        {
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
                }
            }
        }
    }
}

