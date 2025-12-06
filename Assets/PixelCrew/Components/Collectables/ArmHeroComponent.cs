using System;
using UnityEngine;

namespace PixelCrew.Components.Collectables
{
    public class ArmHeroComponent: MonoBehaviour
    {
        public void ArmHero(GameObject go)
        {
           var hero =  go.GetComponent<Hero>();
           if (hero != null)
           {
               hero.ArmHero();
           }
        }
    }
}