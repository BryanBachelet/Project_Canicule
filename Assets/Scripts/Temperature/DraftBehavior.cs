using UnityEngine;
using Canicule.Temperature;
using Canicule.Rooms;
using UnityEngine.Events;

namespace Canicule
{
    public class DraftBehavior : MonoBehaviour
    {
        public struct DraftData
        {
            public Door door;
            public Room room;
        };

        private HeatSource m_heatSource;
        private DraftData m_draftDataA;
        private DraftData m_draftDataB;

        [Header("Parameters")]
        [SerializeField] private float m_draftDegreeReduction = 3.0f;

        [HideInInspector] public UnityEvent OnDraftDestroy;

        private bool m_hasBeenInit = false;
        private bool m_hasStartDestroy = false;

        public void Update()
        {
            if (!m_hasBeenInit) return;

            if(IsDraftShouldBeDestroy())
            {
                DestroyDraft();
                return;
            }

            Window windowA = m_draftDataA.room.GetOpenWindow();
            Window windowB = m_draftDataB.room.GetOpenWindow();

            float degreeA = windowA.GetHeatSource().GetTemperature();
            float degreeB = windowB.GetHeatSource().GetTemperature();

            float averageTemperature = (degreeA + degreeB) / 2.0f;

            float draftTemperature = averageTemperature - m_draftDegreeReduction;
            m_heatSource.SetTemperature(draftTemperature);

        }

        public void InitDraft(DraftData draftDataA, DraftData draftDataB)
        {
            m_heatSource = GetComponent<HeatSource>();
            m_draftDataA = draftDataA;
            m_draftDataB = draftDataB;

            m_draftDataA.room.GetHeatManager().RegisterHeatSource(m_heatSource);
            m_draftDataB.room.GetHeatManager().RegisterHeatSource(m_heatSource);
            
            m_hasBeenInit = true;


        }

        public void DestroyDraft()
        {
            if (m_hasStartDestroy)
                return;

            OnDraftDestroy?.Invoke();
            OnDraftDestroy.RemoveAllListeners();
            m_hasStartDestroy = true;

            m_draftDataA.room.GetHeatManager().UnregisterHeatSource(m_heatSource);
            m_draftDataB.room.GetHeatManager().UnregisterHeatSource(m_heatSource);
            Destroy(this.gameObject);
        }

        private bool IsDraftShouldBeDestroy()
        {
            return !m_draftDataA.room.IsAWindowOpen()
                || !m_draftDataB.room.IsAWindowOpen()
                || !m_draftDataA.door.IsOpen()
                || !m_draftDataB.door.IsOpen();
        }
    }
}
