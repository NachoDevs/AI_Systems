# Group Pathfinding and Formations

## Previous SetUp

- Pathfinding [Required]
- Animated character [Recommended] (Used: https://assetstore.unity.com/packages/3d/characters/humanoids/toon-soldiers-ww2-demo-85702)

## Description

When moving a big amount of units, calculating the paths for each one of those units can be very expensive if the distance is large. But we can use group pathfinding to make this way faster. The way it is achieved in this project is by assigning one unit as a leader, calculating the path for that unit and making the others follow it. Now, this tend to make the follower units form a big blob of agents, and that is not very realistic. For this we can apply algorithms like 'flocking'. In this case it is used one of the rules of the flocking algorithm, the avoidance calculation. This avoidance vector returns a direction for the unit that will make it flee from the closest units around him, that, combined with a movement towards the leader will make a much more realistic group movement and making way more efficient than calculating the path for each individual.

The magenta rays are the avoidance vectors:
![]{https://i.imgur.com/OP9SX4S.gif}

Now, usually, when moving large amounts of units, we don't want them to try to reach the same spot, we want them to stop in an organized way. This is where formations come into place. Once the leader unit is getting closer to the target position the behavior for the follower units will change from following the leader while avoiding other units to a normal pathfinding towards a relative position from the target position. This relative position will be calculated ahead, and it will depend on the formation we choose. In this project there are two formations, an arrow like formation and a square one.

Not good:
![](https://i.imgur.com/qkVIWh5.gif)

Much better:
![]{https://i.imgur.com/h2OL8Hy.gif}

### Follower Units Positioning

This is function computes and says to each unit (except the leader) where it should go taking into account the position of the leader and the avoidance vector.

```cs
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
```

### Formation Calculation

To calculate the formation first the rotation relative to the group of units and the target position is calculated to rotate the default formation accordingly, then each point is assigned, rotated and moved relative to the target position.

```cs
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

	Vector3 moveDirection = targetPosition - unitsCenterPos;
	moveDirection.Normalize();

	Vector3 defFormationDir = new Vector3(0, 0, -1);
	float diffAngle = Vector3.SignedAngle(moveDirection, defFormationDir, Vector3.up);
	diffAngle *= Mathf.Deg2Rad;

	float angleCos = Mathf.Cos(diffAngle);
	float angleSin = Mathf.Sin(diffAngle);
	
	List<Vector3> defPositions = new List<Vector3>();
	switch (formation)
	{
		case UnitFormationType.Arrow:
		{
			int currentRow = 1;
			while(defPositions.Count < (this.Units.Count - 1))
			{
				int rowUnitCount = currentRow * 2 + 1;
				for (int wIndex = 0; wIndex < rowUnitCount; wIndex++)
				{
					int cIndex = wIndex - currentRow;

					Vector3 defPosition = new Vector3(cIndex, 0, currentRow);
					defPositions.Add(defPosition);
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
				Vector3 defPosition = new Vector3
				(
					unitIndex / squareWidth, 
					0, 
					unitIndex % squareWidth
				);

				defPositions.Add(defPosition);
			}
		}
		break;
	}

	List<Vector3> positions = new List<Vector3>() { targetPosition };
	/// Apply rotations
	foreach (var defPosition in defPositions)
	{
		Vector3 newPosition = new Vector3
		(
			defPosition.x * angleCos - defPosition.z * angleSin,
			0,
			defPosition.x * angleSin + defPosition.z * angleCos
		);

		/// Unit spacing
		newPosition *= 2;

		/// Relative to the selected position
		newPosition += targetPosition;

		positions.Add(newPosition);
	}

	return positions;
}
```
