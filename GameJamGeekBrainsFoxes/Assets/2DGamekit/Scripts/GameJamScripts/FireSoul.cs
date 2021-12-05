using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamekit2D
{
    public class FireSoul : MonoBehaviour
    {
        private SpriteRenderer forward_spriteRenderer;
        public GameObject soulBullet;

        private int forward = 1;
        void Start()
        {
            forward_spriteRenderer = GetComponent<SpriteRenderer>();
            if (forward_spriteRenderer.flipX) forward = -1;
        }

        void Update()
        {
            transform.Translate(forward * Vector3.right * Time.deltaTime);
        }
    }
}
