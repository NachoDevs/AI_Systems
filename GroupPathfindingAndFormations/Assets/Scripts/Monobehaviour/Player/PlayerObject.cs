using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerObject : MonoBehaviour
    {
        public List<Game_Characters.Soldier_Interact> MySoldiers;

        private GameObject pointerClickPref;

        /// <summary>
        /// Initializer
        /// </summary>
        private void Awake()
        {
            this.pointerClickPref = Resources.Load<GameObject>("Prefabs/PointerClick");
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
                    MoveAllSoldiersToPosition(hit.point);

                    PlacePointerClick(hit.point);
                }
            }
        }

        /// <summary>
        /// Moves all the player soldiers to a position
        /// </summary>
        private void MoveAllSoldiersToPosition(Vector3 position)
        {
            foreach (var soldier in this.MySoldiers)
            {
                soldier.SetMovementTarget(position);
            }
        }

        /// <summary>
        /// Instantiates a temporary object that shows where the click was done
        /// </summary>
        private void PlacePointerClick(Vector3 position)
        {
            var pointerClick = Instantiate
            (
                this.pointerClickPref, 
                position, 
                Quaternion.identity,
                this.transform
            );

            Destroy(pointerClick, 2);
        }
    }
}

