using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Game_AI
{

    public class GroupController : MonoBehaviour
    {
        public UnitFormationType SelectedFormation;

        private UnitGroup currentGroup;

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


            for (int unitIndex = 0; unitIndex < this.currentGroup.Units.Count; unitIndex++)
            {
                GameObject soldier = this.currentGroup.Units[unitIndex];
                var sInter = soldier.GetComponent<Game_Characters.Soldier_Interact>();

                sInter.SetMovementTarget(positions[unitIndex]);
            }
        }
    }
}

