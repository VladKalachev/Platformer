using System;
using PixelCrew.Creatures.Hero;
using Unity.Cinemachine;
using UnityEngine;

namespace PixelCrew.Components
{
    [RequireComponent(typeof(CinemachineCamera))]
    public class SetFollowComponent : MonoBehaviour
    {
        private void Start()
        {
            var vCamera = GetComponent<CinemachineCamera>();
            vCamera.Follow = FindObjectOfType<Hero>().transform;
        }
    }
}