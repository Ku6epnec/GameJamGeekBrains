using UnityEngine;

namespace Gamekit2D
{
    public class SoulUI : MonoBehaviour
    {
        public static SoulUI Instance { get; protected set; }
        public int soulCount;
        public GameObject soulIconPrefab;
        public string[] soulNames;

        protected Animator[] m_SoulIconAnimators;

        protected readonly int m_HashActivePara = Animator.StringToHash("Active");
        protected const float s_SoulIconAnchorWidth = 0.08f;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SetInitialKeyCount();
        }

        public void SetInitialKeyCount()
        {
            if (m_SoulIconAnimators != null && m_SoulIconAnimators.Length == soulNames.Length)
                return;

            m_SoulIconAnimators = new Animator[soulNames.Length];

            for (int i = 0; i < m_SoulIconAnimators.Length; i++)
            {
                GameObject healthIcon = Instantiate(soulIconPrefab);
                healthIcon.transform.SetParent(transform);
                RectTransform healthIconRect = healthIcon.transform as RectTransform;
                healthIconRect.anchoredPosition = Vector2.zero;
                healthIconRect.sizeDelta = Vector2.zero;
                healthIconRect.anchorMin -= new Vector2(s_SoulIconAnchorWidth, 0f) * i;
                healthIconRect.anchorMax -= new Vector2(s_SoulIconAnchorWidth, 0f) * i;
                m_SoulIconAnimators[i] = healthIcon.GetComponent<Animator>();
            }
        }

        public void ChangeKeyUI(InventoryController controller)
        {
            for (int i = 0; i < soulNames.Length; i++)
            {
                m_SoulIconAnimators[i].SetBool(m_HashActivePara, controller.HasItem(soulNames[i]));
            }
        }
    }
}