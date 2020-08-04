using UnityEngine;
using UnityEngine.AI;

namespace Environment
{
    public class DestructibleWall : MonoBehaviour
    {
        public Mesh DefaultWallMesh;
        public Mesh DestroyedWallMesh;

        public int DefaultSoundCost = 99;

        private NavMeshLink soundLink;
        
        /// <summary>
        /// Initializer
        /// </summary>
        private void Awake()
        {
            this.soundLink = GetComponent<NavMeshLink>();
        }

        /// <summary>
        /// Action that takes place when this object is enabled
        /// </summary>
        private void OnEnable()
        {
            this.soundLink.width = this.transform.localScale.x;
            ChangeSoundCost(this.DefaultSoundCost);

            var meshRenderer = GetComponent<MeshFilter>();
            meshRenderer.mesh = this.DefaultWallMesh;
        }

        /// <summary>
        /// Action that takes place when this wall is destroyed
        /// </summary>
        public void DestroyWall()
        {
            ChangeSoundCost(0);

            /// Chaging the mesh of the wall
            var meshRenderer = GetComponent<MeshFilter>();
            meshRenderer.mesh = this.DestroyedWallMesh;
        }

        /// <summary>
        /// Changes the cost of the pathfinding link calculation
        /// </summary>
        private void ChangeSoundCost(int newCost)
        {
            this.soundLink.costModifier = newCost;
        }
    }
}

