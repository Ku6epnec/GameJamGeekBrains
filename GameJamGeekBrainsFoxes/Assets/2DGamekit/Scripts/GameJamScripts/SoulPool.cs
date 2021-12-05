using UnityEngine;
using UnityEngine.Tilemaps;


namespace Gamekit2D
{
    public class SoulPool : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject[] soulUI;
        [SerializeField] private GameObject soulBullet;
        [SerializeField] private GameObject exolosion;
        [SerializeField] private Transform soulSpawnPointR;
        [SerializeField] private Transform soulSpawnPointL;

        [SerializeField] private int _soulCount = 0;

        [SerializeField] private float _maxSoulLifeTime = 5;
        [SerializeField] private float _soulBulletLifeTime = 5;

        [SerializeField] private bool _spriteOriginallyFacesLeft;
        [SerializeField] private bool _slCheck = false;
        [SerializeField] private bool _reinforced = false;

        protected SpriteRenderer m_SpriteRenderer;
        const float k_OffScreenError = 0.01f;
        static readonly int VFX_HASH = VFXController.StringToHash("BulletImpact");

        private GameObject sl;
        private GameObject expl;

        private int _soulCountMax = 3;


        #endregion

        #region UnityMethods

        private void OnEnable()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P) && !_slCheck)
            {
                FireSoul();
            }

            if (_slCheck)
            {
                if (_soulBulletLifeTime > 0f)
                {
                    _soulBulletLifeTime = _soulBulletLifeTime - Time.deltaTime;
                }
                else
                {
                    Destroy(sl);
                    _slCheck = false;
                    _soulBulletLifeTime = _maxSoulLifeTime;
                }

                if (Input.GetKeyDown(KeyCode.I))
                {
                    transform.position = sl.transform.position;
                    expl = Instantiate(exolosion, transform); 
                    Destroy(sl, 0f);
                    Destroy(expl, 2f);
                    _slCheck = false;
                    _soulBulletLifeTime = _maxSoulLifeTime;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Soul")
                AddSoul();
            if (collision.gameObject.tag == "Upgrade")
                _reinforced = true;
        }

        #endregion

        #region OutherMethods
        private void AddSoul()
        {
            if (_soulCount < _soulCountMax)
            {
                soulUI[_soulCount].SetActive(true);
                _soulCount++;
            }

        }

        private void FireSoul()
        {
            if (_soulCount == _soulCountMax && _reinforced)
            { 
                for (_soulCount = 0; _soulCount < _soulCountMax; _soulCount++)
                    {
                    soulUI[_soulCount].SetActive(false);
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
                _soulCount = 0; _slCheck = true; 
            }
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
            Vector3 forward = _spriteOriginallyFacesLeft ? Vector3.left : Vector3.right;
            if (m_SpriteRenderer.flipX) forward.x = -forward.x;

            TileBase surfaceHit = PhysicsHelper.FindTileForOverride(collider, transform.position, forward);

            VFXController.Instance.Trigger(VFX_HASH, transform.position, 0, m_SpriteRenderer.flipX, null, surfaceHit);
        }

        #endregion
    }
}