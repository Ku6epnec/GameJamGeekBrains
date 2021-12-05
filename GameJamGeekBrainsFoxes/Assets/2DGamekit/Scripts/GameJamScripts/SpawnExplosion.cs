using UnityEngine;


namespace GameJam
{
    public class SpawnExplosion : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject explose;

        #endregion

        #region OtherMethods

        public void spawnExplosion()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            GameObject exp = Instantiate(explose, player.transform);
            Destroy(exp, 1f);
        }

        #endregion
    }
}