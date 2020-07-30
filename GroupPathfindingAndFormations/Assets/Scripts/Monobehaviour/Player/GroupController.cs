using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Game_AI
{

    public class GroupController : MonoBehaviour
    {
        private const float POINTERCLICKS_LIFESPAM = 2F;
        public UnitFormationType SelectedFormation;

        private GameObject pointerClickPref;

        private List<GameObject> prevPointerClicks;

        private UnitGroup currentGroup;

        /// <summary>
        /// Initializer
        /// </summary>
        private void Awake()
        {
            this.pointerClickPref = Resources.Load<GameObject>("Prefabs/PointerClick");
        }

        /// <summary>
        /// Creates a new group with the units in the parameter
        /// </summary>
        public void SetNewGroup(List<GameObject> units)
        {
            this.currentGroup = new UnitGroup(units);
        }

        /// <summary>
        /// Add new units to the current group, or creates one if it does not exists
        /// </summary>
        public void AddUnitsToCurrentGroup(List<GameObject> newUnits)
        {
            if(this.currentGroup != null)
            {
                this.currentGroup.AddUnits(newUnits);
            }
            else
            {
                SetNewGroup(newUnits);
            }
        }

        /// <summary>
        /// Gets all the positions relative to the leader positions and command the units to move
        /// </summary>
        public void MoveGroup(Vector3 targetPosition)
        {
            var positions = this.currentGroup.GetPositionsWithFormation(targetPosition, this.SelectedFormation);
            PlacePointerClick(positions);

            for (int unitIndex = 0; unitIndex < this.currentGroup.Units.Count; unitIndex++)
            {
                GameObject soldier = this.currentGroup.Units[unitIndex];
                var sInter = soldier.GetComponent<Game_Characters.Soldier_Interact>();

                sInter.SetMovementTarget(positions[unitIndex]);
            }
        }

        /// <summary>
        /// Instantiates a temporary object that shows where the click was done
        /// </summary>
        private void PlacePointerClick(List<Vector3> positions)
        {
            if(this.prevPointerClicks == null)
            {
                this.prevPointerClicks = new List<GameObject>();
            }

            /// If another pointerClick still exists, destroy it
            if(this.prevPointerClicks.Count > 0)
            {
                foreach (var pClick in this.prevPointerClicks)
                {
                    Destroy(pClick);
                }
            }

            foreach (var unitPos in positions)
            {
                var pointerClick = Instantiate
                (
                    this.pointerClickPref, 
                    unitPos, 
                    Quaternion.identity,
                    this.transform
                );

                this.prevPointerClicks.Add(pointerClick);

                StartCoroutine(DestroyPointerAfterTime(pointerClick));
            }
        }

        private IEnumerator DestroyPointerAfterTime(GameObject pointerClick)
        {
            yield return new WaitForSeconds(POINTERCLICKS_LIFESPAM);
            
            this.prevPointerClicks.Remove(pointerClick);

            /// Destroy this pointerClick after 2 seconds
            Destroy(pointerClick);
        }
    }
}

