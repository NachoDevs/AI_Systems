# 3D Sound

## Previous SetUp

- NavMesh Components [Required] (https://github.com/Unity-Technologies/NavMeshComponents)
- Animated character [Recommended] (Used: https://www.mixamo.com)

## Description

Sound is a very important aspect of games, and a good ai integration where it is used as a detection mechanism, can bring really good results. Not every environment prop affects the same way the path that the sound travels as it affects the NPCs, so it needs a different NavMesh setup. In this project pathfinding is used to calculate the travel distance of the sound, and checking if the distance is short enough to be heard by the AI agent. It also takes into account the 'loudness level' of the sound, the longer the distance the sound travels the louder it needs to be for the sound to be heard.

The interesting part of this project is the use of the NavMeshLinks with a dynamic cost modification based on the destruction level of the level props, in this case a wall. Since we need a different NavMesh for the sound, we can have unique links for that NavMesh which is only used by agents of type 'sound'.

Here is an example where if the wall is broken the path to the sound source is way shorter. The white line is the path traveled by the sound.

![](https://i.imgur.com/XzjdI1P.gif)

In this project the most important is not the code provided but the NavMeshLink and the NavMeshSurface provided by the Unity-Technologies team in the link above and the way it is set up in the scene.

As you can see in the next image, there is an area around the wall, this is the NavMeshLink which allows calculating a path through the wall. If an agent tried to walk this path it would be blocked by the wall, but since we are only using it to calculate a path for the sound, there is no problem in using it this way. And this is the reason why the NavMesh components from Unity-Technologies are used, because by default it is not possible (or I don't know how to do it) to bake a mesh through an object which is affecting the Navmesh of the normal agents and be able to recalculate the cost of the area.

![](https://i.imgur.com/2BQ2C6y.png)

## Disclaimer

Even though the 'mutant' model is free (you can get it in the link above) it has been removed from the repository due to its heavy file size.
