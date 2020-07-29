using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class UnitGroup
    {
        /// <summary>
        /// Constructor for a new Unit Group
        /// </summary>
        public List<GameObject> Units;

        public UnitGroup (List<GameObject> newUnits)
        {
            this.Units = new List<GameObject>(newUnits);
        }

        /// <summary>
        /// Add new units to this group
        /// </summary>
        public void AddUnits(List<GameObject> newUnits)
        {
            if(this.Units != null)
            {
                this.Units.AddRange(newUnits);
            }
            else
            {
                this.Units = new List<GameObject>(newUnits);
            }
        }

        /// <summary>
        /// Returns the correct position for each unit based on a relative position and a formation
        /// </summary>
        public List<Vector3> GetPositionsWithFormation(Vector3 targetPosition, UnitFormationType formation)
        {
            SortUnitsByDistanceToPosition(targetPosition);
            
            Vector3 unitsCenterPos = Vector3.zero;
            foreach (var unit in this.Units)
            {
                unitsCenterPos += unit.transform.position;
            }
            unitsCenterPos /= this.Units.Count;

<<<<<<< HEAD
            Vector3 moveDirection = targetPosition - unitsCenterPos;
            moveDirection.Normalize();

            Vector3 defaultFormationDir = new Vector3(0, 0, -1);
            float diffAngle = Vector3.SignedAngle(defaultFormationDir, moveDirection, Vector3.up);

            Debug.Log(diffAngle);

=======
            var target2DPos = new Vector2
            (
                targetPosition.x, 
                targetPosition.z
            );
            
>>>>>>> 81e4e91d5c8f61c373cf31126cfc44828b45768b
            List<Vector3> positions = new List<Vector3>() { targetPosition };
            switch (formation)
            {
                case UnitFormationType.Arrow:
                {
                    int currentRow = 1;
                    while(positions.Count < this.Units.Count)
                    {
                        int rowUnitCount = currentRow * 2 + 1;
                        for (int wIndex = 0; wIndex < rowUnitCount; wIndex++)
                        {
                            int cIndex = wIndex - currentRow;

                            Vector2 defPosition = new Vector2(cIndex, currentRow);

<<<<<<< HEAD
                            float angleCos = Mathf.Cos(diffAngle);
                            float angleSin = Mathf.Sin(diffAngle);
=======
                            Vector2 defRelDirection = Vector2.zero - defPosition;

                            float angleDiff = Vector2.SignedAngle(defRelDirection, target2DPos);

                            float angleCos = Mathf.Cos(angleDiff);
                            float angleSin = Mathf.Sin(angleDiff);
>>>>>>> 81e4e91d5c8f61c373cf31126cfc44828b45768b

                            Vector3 newPosition = new Vector3
                            (
                                defPosition.x * angleCos - defPosition.y * angleSin,
                                0,
                                defPosition.x * angleSin + defPosition.y * angleCos
                            );

                            /// Unit spacing
                            newPosition *= 2;

                            /// Relative to the selected position
                            newPosition += targetPosition;

                            positions.Add(newPosition);
                        }
                        currentRow++;
                    }
                }
                break;
                default:
                case UnitFormationType.Square:
                {
                    int squareWidth = Mathf.CeilToInt(Mathf.Sqrt(this.Units.Count));

                    /// Skip the leather
                    for (int unitIndex = 1; unitIndex < this.Units.Count; unitIndex++)
                    {
                        Vector3 newPosition = new Vector3(unitIndex / squareWidth, 0, unitIndex % squareWidth);
                        
                        /// Unit spacing
                        newPosition *= 2;

                        /// Relative to the selected position
                        newPosition += targetPosition;

                        positions.Add(newPosition);
                    }
                }
                break;
            }


            return positions;
        }

        /// <summary>
        /// Function that sorts the units by their distance to the target position
        /// </summary>
        private void SortUnitsByDistanceToPosition(Vector3 targetPosition)
        {
            this.Units.Sort((unitA, unitB) => 
            {
                float aDist = Vector3.Distance(unitA.transform.position, targetPosition);
                float bDist = Vector3.Distance(unitB.transform.position, targetPosition);

                return aDist.CompareTo(bDist);
            });
        }
    }

    public enum UnitFormationType
    {
        Arrow,
        Square,
    }
}

