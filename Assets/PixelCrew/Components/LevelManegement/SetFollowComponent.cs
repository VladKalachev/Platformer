using System;
using PixelCrew.Creatures.Hero;
using Unity.Cinemachine;
using UnityEngine;

namespace PixelCrew.Components
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class SetFollowComponent : MonoBehaviour
    {
        private void Start()
        {
            var vCamera = GetComponent<CinemachineVirtualCamera>();
            vCamera.Follow = FindObjectOfType<Hero>().transform;
        }
    }
}