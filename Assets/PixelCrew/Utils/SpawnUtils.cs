using UnityEngine;

namespace PixelCrew.Utils
{
    public class SpawnUtils
    {
        private const string ContainerName = "###SPWNED###";

        public static GameObject Spawn(GameObject prefab, Vector3 position)
        {
            var container = GameObject.Find(ContainerName);
            if (container == null)
                container = new GameObject(ContainerName);

            Object.Instantiate(prefab, position, Quaternion.identity, container.transform);
        }
    }
}