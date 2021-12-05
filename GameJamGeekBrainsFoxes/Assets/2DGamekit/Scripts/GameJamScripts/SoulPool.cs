using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Gamekit2D
{
    public class SoulPool : MonoBehaviour
    {
        public bool spriteOriginallyFacesLeft;
        public bool reinforced = false;
        protected SpriteRenderer m_SpriteRenderer;
        public GameObject[] soulUI;
        static readonly int VFX_HASH = VFXController.StringToHash("BulletImpact");

        const float k_OffScreenError = 0.01f;

        protected float m_Timer;

        private void OnEnable()
        {

            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_Timer = 0.0f;
        }

        public float maxSoulLifeTime = 5;
        public float soulBulletLifeTime = 5;
        public GameObject soulBullet;
        public GameObject exolosion;
        public int soulCount = 0;
        public Transform soulSpawnPointR;
        public Transform soulSpawnPointL;
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
                if (soulBulletLifeTime > 0f)
                {
                    soulBulletLifeTime = soulBulletLifeTime - Time.deltaTime;
                }
                else
                {
                    Destroy(sl);
                    slCheck = false;
                    soulBulletLifeTime = maxSoulLifeTime;
                }
                if (Input.GetKeyDown(KeyCode.I))
                {
                    transform.position = sl.transform.position;
                    expl = Instantiate(exolosion, transform); 
                    Destroy(sl, 0f);
                    Destroy(expl, 2f);
                    slCheck = false;
                    soulBulletLifeTime = maxSoulLifeTime;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Soul")
                AddSoul();
            if (collision.gameObject.tag == "Upgrade")
                reinforced = true;

        }

        public void AddSoul()
        {
            if (soulCount < soulCountMax)
            {
                soulUI[soulCount].SetActive(true);
                soulCount++;
            }

        }

        public void FireSoul()
        {
            if (soulCount == soulCountMax && reinforced)
            { 
                for (soulCount = 0; soulCount < soulCountMax; soulCount++)
                    {
                    soulUI[soulCount].SetActive(false);
                    }
                sl = Instantiate(soulBullet, transform.parent);                
                SpriteRenderer sl_spriteRenderer = sl.GetComponent<SpriteRenderer>();
                if (m_SpriteRenderer.flipX)
                {
                    sl.transform.position = soulSpawnPointL.position;
                    sl_spriteRenderer.flipX = true;
                }
                else
                {
                    sl.transform.position = soulSpawnPointR.position;
                    sl_spriteRenderer.flipX = false;
                }
                soulCount = 0; slCheck = true; }
        }

        /*public void UpgradeSoulAbility()
        {
            reinforced = true;
        }*/

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