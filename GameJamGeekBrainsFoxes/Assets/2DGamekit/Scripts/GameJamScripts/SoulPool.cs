using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Gamekit2D
{
    public class SoulPool : MonoBehaviour
    {
        public bool spriteOriginallyFacesLeft;

        protected SpriteRenderer m_SpriteRenderer;
        static readonly int VFX_HASH = VFXController.StringToHash("BulletImpact");

        //public GameObject player = GameObject.FindGameObjectWithTag("Player");
        const float k_OffScreenError = 0.01f;

        protected float m_Timer;

        private void OnEnable()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_Timer = 0.0f;
        }


        public GameObject soulBullet;
        public GameObject exolosion;
        public int soulCount = 0;
        public Transform soulSpawnPoint;
        private int soulCountMax = 3;
        private GameObject sl;
        private GameObject expl;
        private bool slCheck = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P) && !slCheck)
            {
                FireSoul();
            }
            if (slCheck)
            { 
                //sl.transform.Translate(Vector3.right * Time.deltaTime);
                if (Input.GetKeyDown(KeyCode.I))
                {
                    transform.position = sl.transform.position;
                    expl = Instantiate(exolosion, transform); 
                    Destroy(sl, 0f);
                    Destroy(expl, 2f);
                    slCheck = false;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Soul")
                AddSoul();
        }

        public void AddSoul()
        {
            if (soulCount < soulCountMax)
                soulCount++;
        }

        public void FireSoul()
        {
            if (soulCount == soulCountMax)
            { sl = Instantiate(soulBullet, transform.parent); sl.transform.position = soulSpawnPoint.position; soulCount = 0; slCheck = true; }
        }

        public void OnHitDamageable(Damager origin, Damageable damageable)
        {
            FindSurface(origin.LastHit);
        }

        public void OnHitNonDamageable(Damager origin)
        {
            FindSurface(origin.LastHit);
        }

        protected void FindSurface(Collider2D collider)
        {
            Vector3 forward = spriteOriginallyFacesLeft ? Vector3.left : Vector3.right;
            if (m_SpriteRenderer.flipX) forward.x = -forward.x;

            TileBase surfaceHit = PhysicsHelper.FindTileForOverride(collider, transform.position, forward);

            VFXController.Instance.Trigger(VFX_HASH, transform.position, 0, m_SpriteRenderer.flipX, null, surfaceHit);
        }
    }
}