using UnityEngine;
using Canicule.Temperature;


namespace Canicule
{
    public class WaterBowl : InteractibleObject
    {
        [SerializeField] private float m_bowlReduceDegree = 1.0f;
        [SerializeField] private float m_bowlFillDuration = 50.0f;
        private float m_bowlFillTimer = 0.0f;

        private bool m_isBowlFill = false;
        private HeatSource m_heatSource;


        public override void Start()
        {
            base.Start();

            m_heatSource = GetComponent<HeatSource>();
        }

        public override void Update()
        {
            base.Update();
        }

        protected override void Interact()
        {
            base.Interact();


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

        public void FillBowl()
        {
            m_isBowlFill = true;
            m_bowlFillTimer = 0.0f;
            m_heatSource.ActiveHeatSource();
        }


        private void EmptyBowl()
        {
            m_bowlFillTimer = 0.0f;
            m_isBowlFill = false;
            m_heatSource.DeactiveHeatSource();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag != "Player" && collision.tag != "Monster")
                return;

            EmptyBowl();

        }
    }
}
