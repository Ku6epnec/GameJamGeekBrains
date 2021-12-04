using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamekit2D
{
    public class SoulController : MonoBehaviour
    {
        public GameObject soulBullet;
        public int soulCount;
        private int soulCountMax = 3;
        public void FireSoul()
        {
            if (soulCount == soulCountMax)
                Instantiate(soulBullet);
        }
    }
}