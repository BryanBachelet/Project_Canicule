using UnityEngine;
using Canicule.Temperature;
using NUnit.Framework.Interfaces;


namespace Canicule
{
    public class WaterBowl : InteractibleObject
    {
        [SerializeField] private float m_bowlReduceDegree = 1.0f;
        [SerializeField] private float m_bowlFillDuration = 50.0f;
        private float m_bowlFillTimer = 0.0f;

        private bool m_isBowlFill = false;
        private HeatSource m_heatSource = null;

        [Header("Sprite")]
        [SerializeField] private Sprite m_emptyBowlSprite;
        [SerializeField] private Sprite m_waterBowlSprite;
        private SpriteRenderer m_spriteRenderer = null;

        private bool m_isInitializationDone = false;

        public override void Start()
        {
            base.Start();

            InitializationObject();
        }


        public override void Update()
        {
            base.Update();
        }

        private void InitializationObject()
        {
            if (m_isInitializationDone) return;

            m_heatSource = GetComponent<HeatSource>();
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            m_isInitializationDone = true;
        }

        protected override void Interact()
        {
            base.Interact();

            ItemData itemData = CreateItemInstance();
            WaterBowlEffect bowlEffect = itemData.GetItemEffect<WaterBowlEffect>();
            bowlEffect.m_isFill = m_isBowlFill;
            bowlEffect.m_fillDuration = m_bowlFillTimer;
            RetrieveObject(itemData);
        }

        public void UpdateBowl()
        {
            if (m_isBowlFill)
            {
                float degree = m_roomOwner.GetRoomDegree() - m_bowlReduceDegree;
                m_heatSource.SetTemperature(degree);

                if (m_bowlFillTimer > m_bowlFillDuration)
                {

                    m_isBowlFill = false;
                    m_heatSource.DeactiveHeatSource();
                }
            }
        }

        public void FillBowl(float fillTimer = 0)
        {
            m_isBowlFill = true;
            m_bowlFillTimer = fillTimer;
            m_heatSource.ActiveHeatSource();
            m_spriteRenderer.sprite = m_waterBowlSprite;

        }


        private void EmptyBowl()
        {
            m_bowlFillTimer = 0.0f;
            m_isBowlFill = false;
            m_heatSource.DeactiveHeatSource();
            m_spriteRenderer.sprite = m_emptyBowlSprite;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag != "Player" && collision.tag != "Monster")
                return;

           // EmptyBowl();

        }

        public override void CreateObject(ItemData data)
        {
            base.CreateObject(data);
            WaterBowlEffect waterEffect = data.GetItemEffect<WaterBowlEffect>();
            if(waterEffect != null && waterEffect.m_isFill)
            {
                InitializationObject();
                FillBowl(waterEffect.m_fillDuration);
            }
        }
    }
}
