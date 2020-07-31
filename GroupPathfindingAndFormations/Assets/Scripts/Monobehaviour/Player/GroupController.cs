using System.Collections;
using System.Collections.Generic;
using Game_Characters;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

namespace Game_AI
{

    public class GroupController : MonoBehaviour
    {
        private const float POINTERCLICKS_LIFESPAM = 2F;

        public UnitFormationType SelectedFormation;

        public float StopFlockingDistance = 5f;

        private List<GameObject> prevPointerClicks;
        private List<Vector3> targetPositions;

        private GameObject groupLeader;
        private GameObject pointerClickPref;

        private UnitGroup currentGroup;

        private bool areFlocking;
        private bool isMovementBeingComputed;

        /// <summary>
        /// Initializer
        /// </summary>
        private void Awake()
        {
            this.pointerClickPref = Resources.Load<GameObject>("Prefabs/PointerClick");
        }

        /// <summary>
        /// Action that takes place every frame
        /// </summary>
        private void Update()
        {
            if(this.isMovementBeingComputed)
            {
                if(this.areFlocking)
                {
                    ApplyAvoidanceRules();

                    /// If we are getting closer to the target we switch to pathfinding
                    float distanceToTarget = Vector3.Distance
                    (
                        this.currentGroup.Units[0].transform.position,
                        targetPositions[0]
                    );

                    this.areFlocking = distanceToTarget >= this.StopFlockingDistance;
                }
                else
                {
                    PathfindUnitsToTargetPositions();
                    this.isMovementBeingComputed = false;
                }
            }
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
        /// Gets all the positions relative to the leader positions and starts the movement of units
        /// </summary>
        public void MoveGroup(Vector3 targetPosition)
        {
            this.targetPositions = new List<Vector3>();
            
            this.targetPositions = this.currentGroup.GetPositionsWithFormation(targetPosition, this.SelectedFormation);
            PlacePointerClick(this.targetPositions);

            /// Get the closest to the target and set it as the leader
            this.groupLeader = this.currentGroup.Units[0];
            var leaderInteract = groupLeader.GetComponent<Soldier_Interact>();
            leaderInteract.SetMovementTarget(targetPosition);

            this.isMovementBeingComputed = true;
            this.areFlocking = true;
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

        /// <summary>
        /// Calculates target positions for the units using a group avoidance rule
        /// </summary>
        private void ApplyAvoidanceRules()
        {
            int groupSize = this.currentGroup.Units.Count;

            /// We are going to compare each unit with the others
            for (int unitIndex = 0; unitIndex < groupSize; unitIndex++)
            {
                GameObject unit = this.currentGroup.Units[unitIndex];

                /// The leader is the only one who will use pathfinding
                ///     the rest will follow
                if(unit == this.groupLeader)
                {
                    continue;
                }

                Vector3 gAvoidance  = Vector3.zero; // Group Avoidance
                float neighborDistance;

                for (int comparedUnitIndex = 0; comparedUnitIndex < groupSize; comparedUnitIndex++)
                {
                    GameObject comparedUnit = this.currentGroup.Units[comparedUnitIndex];

                    if(comparedUnit == this.groupLeader)
                    {
                        continue;
                    }

                    if(unit != comparedUnit)
                    {
                        neighborDistance = Vector3.Distance
                        (
                            comparedUnit.transform.position, 
                            unit.transform.position
                        );

                        if(neighborDistance < 2f)
                        {
                            gAvoidance += unit.transform.position - comparedUnit.transform.position;
                        }
                    }
                }

                gAvoidance.Normalize();
                gAvoidance *= 4;

                Vector3 direction = gAvoidance + (this.groupLeader.transform.position - unit.transform.position);
                direction.Normalize();
                direction *= this.StopFlockingDistance;

                Debug.DrawRay(unit.transform.position, direction, Color.black, .1f);
                Debug.DrawRay(unit.transform.position, gAvoidance, Color.magenta, .1f);

                Vector3 newTargetPos = direction + unit.transform.position;

                var sInteract = unit.GetComponent<Soldier_Interact>();
                sInteract.SetMovementTarget(newTargetPos);
            }
        }

        /// <summary>
        /// Destroys a pointerclick after some time has elapsed 
        /// </summary>
        private IEnumerator DestroyPointerAfterTime(GameObject pointerClick)
        {
            yield return new WaitForSeconds(POINTERCLICKS_LIFESPAM);
            
            this.prevPointerClicks.Remove(pointerClick);

            /// Destroy this pointerClick after 2 seconds
            Destroy(pointerClick);
        }

        /// <summary>
        /// Enables the pathfinding to all units to adjust to their corresponding target position
        /// </summary>
        private void PathfindUnitsToTargetPositions()
        {
            for (int unitIndex = 0; unitIndex < this.currentGroup.Units.Count; unitIndex++)
            {
                GameObject soldier = this.currentGroup.Units[unitIndex];
                var sInter = soldier.GetComponent<Soldier_Interact>();

                sInter.SetMovementTarget(this.targetPositions[unitIndex]);
            }
        }
    }
}

