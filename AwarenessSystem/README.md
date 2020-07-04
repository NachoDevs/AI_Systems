# Awareness System

## Previous SetUp

- Rigged character [Suggested]
- Pathfinding [Not required]
- Wander System [Not required]

## Description
Non Player Characters (NPCs) exists in almost every game created, and they have an essential role in making the user experience as immersive as possible. A good integration between the behaviors of those NPCs and the animation system of a game is vital. A NPC that walk straight and just looks to the front is nor very organic. With the component created in this Unity3D project, the characters will look alive, being able to look around them to interesting objects and prioritizing those with more interest to them.

### Scene View
![](https://i.imgur.com/rQvQB0V.gif)

### Game View
![](https://i.imgur.com/eERuBzx.gif)

This Awareness System, as it has been called, is composed by two components:

### AwarenessTarget Component
This component contains a priority value that will determine which object is the character looking at. It also has been added a material changing method to better indicate the level of priority, but the only important part of this component is the Priority int value.

```cs
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class AwarenessTarget : MonoBehaviour
{
	[Tooltip("The level of priority used when a character decides which object to look at.")]
	[Range(0, 10)]
	public int Priority = 0;

	/// Just to update it at least as possible, not really needed for the awareness system
	private int prevPriority = -1;

	/// <summary>
	/// Action that takes places once every update
	/// </summary>
	private void Update()
	{
		if(this.Priority != this.prevPriority)
		{
			MatchMaterial();
		}
	}

	/// <summary>
	/// Updates the material to match the current priority level 
	/// </summary>
	private void MatchMaterial()
	{
			/// Update the value
			this.prevPriority = this.Priority;

			var renderer = GetComponent<MeshRenderer>();

			/// Get the current material
			Material newMat = renderer.material;

			/// If the material is null, return
			if(!newMat)
			{
				return;
			}

			/// Calculate the level of redness based on the max of the values of priority
			float nonRedValue = Priority / 10.0f;
			nonRedValue = Mathf.Abs(1 - nonRedValue);

			/// Change the material's color
			newMat.color = new Color(1, nonRedValue, nonRedValue, 1);

			/// Change the renderer material
			renderer.material = newMat;
	}
}
```

### AwarenessBehaviour
This component contains the logic behind the selection of targets. First it will create a collider which will be used to detect objects in front of the character. Once an object enters the collider, the system will decide if it wants to look at the new object, or keep looking where it was before.

This component also contains a method called CanSee, which is used to determine if the character can see this object. In this case it is done with a dot product. This could be enhanced with some ray trace checks to different parts of the object to check if something is blocking the vision, but this particular behavior will be implemented in other AI System.

```cs
public class AwarenessBehaviour : MonoBehaviour
{
	public Transform LookTarget;

	public float AwarenessRadius;
	public float DotVisionLimit;

	public AwarenessTarget currTarget;

	private Vector3 defaultLookTargetOffset;

	/// <summary>
	/// Initializer
	/// </summary>
	private void Awake()
	{
		/// Add a collider for awareness detection
		SphereCollider awarenessCollider = this.gameObject.AddComponent<SphereCollider>();
		awarenessCollider.isTrigger = true;
		awarenessCollider.radius = this.AwarenessRadius;

		Vector3 newCenter = new Vector3(0,0, this.AwarenessRadius);
		awarenessCollider.center = newCenter;

		this.defaultLookTargetOffset = LookTarget.transform.localPosition;
	}

	/// <summary>
	/// Action that takes places once every physiscs update
	/// </summary>
	private void FixedUpdate()
	{
		if(this.currTarget != null)
		{
			if(!CanSee(this.currTarget.transform))
			{
				ResetLookDirection();
				return;
			}

			Vector3 targetPos = this.currTarget.transform.position;
			this.LookTarget.transform.position = targetPos;
		}
	}

	/// <summary>
	/// Action that takes place when an object is inside our trigger
	/// </summary>
	private void OnTriggerStay(Collider other)
	{
		/// Check if the collision has the AwarenessTarget component
		if (!other.TryGetComponent<AwarenessTarget>(out AwarenessTarget newTarget))
		{
			return;
		}

		/// If we don't have a target yet
		if (this.currTarget == null)
		{
			this.currTarget = newTarget;
		}
		else
		{
			if(!CanSee(newTarget.transform))
			{
				return;
			}

			/// If we find some object with higher priority
			if(newTarget.Priority > this.currTarget.Priority)
			{
				this.currTarget = newTarget;
			}
			else if(newTarget.Priority == this.currTarget.Priority)
			{
				/// If they have the same priority, set the closest as the target
				float targetDist    = Vector3.Distance(this.transform.position, this.currTarget.transform.position);
				float newDist       = Vector3.Distance(this.transform.position, newTarget.transform.position);

				/// If we are closer to the new object we set this as our target
				if(newDist > targetDist)
				{
					this.currTarget = newTarget;
				}
			}
		}
	}

	/// <summary>
	/// Action that takes place when an object enters our trigger
	/// </summary>
	private void OnTriggerExit(Collider other)
	{
		/// Check if the collision has the AwarenessTarget component
		if (!other.TryGetComponent<AwarenessTarget>(out AwarenessTarget target))
		{
			return;
		}

		if(target == this.currTarget)
		{
			ResetLookDirection();
			this.currTarget = null;
		}
	}

	///<summary>
	/// Checks if the object is in front of the player
	///</summary>
	private bool CanSee(Transform target)
	{
		Vector3 heading = this.transform.position - target.position;
		heading.Normalize();
		float dotProduct = Vector3.Dot(this.transform.forward, heading);

		return dotProduct < this.DotVisionLimit;
	}

	///<summary>
	/// Makes the character reset the position of the look target
	///</summary>
	private void ResetLookDirection()
	{
		Vector3 newLookPos = this.transform.position + this.transform.forward * 2;
		newLookPos.y = defaultLookTargetOffset.y;
		this.LookTarget.transform.position = newLookPos;
	}
}
```
