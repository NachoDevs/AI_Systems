# 3D Vision

## Previous SetUp

- Animated character [Recommended] (Used: https://www.mixamo.com)

## Description

When working with vision in 3D games, it is very important to be fair to the player since in a game with a good user experience is not about what is right, but what the user thinks is right. Specially in stealth games. So if we are playing as a character trying to avoid guards, and our character's hand sticks out of the wall, it is probably a good idea not to trigger a guard's alert behavior if it sees just a hand.

That's why a multipoint vision system like this one comes in handy. Here there is a visibility value which adds up for every part of the body the guard sees. Once this threshold is reached, some behavior can be triggered (In this case an exclamation mark will be instantiated on top of the guard). This system also allows to assign different weights to different parts of the body, so if the head is exposed, the alert behavior is triggered, but if just a hand is seen, nothing happens.

The red rays are vision checks that are not successful, and the green ones are the successful on the next gifs.

Not fair:

![](https://i.imgur.com/xHsVwRb.gif)

Fair:

![](https://i.imgur.com/TFVwqiZ.gif)

### Assigning Body Parts and Weights

In the Character_Interact we can find one list where the different body-parts are referenced and another list with their corresponding weights. This should be done on a dictionary (Dictionary<Transform, float>) but it has been done this way to show it more clearly in the inspector. As in most of this projects, some of the inspector assigned variables are not the best way to do it for a normal project, but it is done this way for clarity and simplification. 

### Computing spotting value

The next function computes the value of all the parts that are being detected.

```cs
/// <summary>
/// Checks if this character can be seen from a position
/// </summary>
public bool CanBeSeen(Vector3 spotterEyes, float visionDistance)
{
	float spotingValue = 0;

	for (int vTargetIndex = 0; vTargetIndex < this.Keys_VisionTargets.Count; vTargetIndex++)
	{
		var target  = this.Keys_VisionTargets[vTargetIndex];
		var tWeight = this.Values_Weights[vTargetIndex];

		var lookDir = target.transform.position - spotterEyes;

		Ray ray = new Ray(spotterEyes, lookDir);
		bool isAHit =  false;
		if(Physics.Raycast(ray, out RaycastHit hit, visionDistance/*, ~this.visionRaycastLayerIndex*/));
		{
			if(hit.collider.transform == target)
			{
				spotingValue += tWeight;
				isAHit = true;
			}
		}

		Debug.DrawLine
		(
			spotterEyes, 
			target.transform.position,
			isAHit ? Color.green : Color.red,
			1.9f
		);
	}

	return spotingValue >= this.VisibilityThreshold; 
}
```
