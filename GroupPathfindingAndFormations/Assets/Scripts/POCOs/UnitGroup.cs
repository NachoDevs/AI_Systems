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

            var formationDirection = targetPosition - this.Units[0].transform.position;

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

                            Vector3 newPosition = new Vector3(cIndex, 0, currentRow);

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

