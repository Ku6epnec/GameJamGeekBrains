using UnityEngine;


namespace GameJam
{
    public class FireSoul : MonoBehaviour
    {
        #region Fields

        [SerializeField] private float _speed = 2f;

        private SpriteRenderer _forwardSpriteRenderer;

        #endregion

        #region UnityMethods

        private void Start()
        {
            _forwardSpriteRenderer = GetComponent<SpriteRenderer>();
            if (_forwardSpriteRenderer.flipX) _speed = -_speed;
        }

        private void Update()
        {
            transform.Translate(_speed * Vector3.right * Time.deltaTime);
        }

        #endregion
    }
}
