using UnityEngine;

namespace Canicule.Temperature
{
    public class HeatGeneralData : MonoBehaviour
    {
        public static HeatGeneralData instance;

        public float valueNextDegreeHeatScore = 100;
        public float positifHeatScoreGain = 4;
        public float negatifHeatScoreGain = -2;
        public float ratioGainDegreeGap = .25f;

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        #region Static Function

        public static float GetValueNextDegreeHeatScore()
        {
            if (instance == null)
                return -1;

            return instance.valueNextDegreeHeatScore;
        }

        public static float GetPositifHeatScoreGain()
        {
            if (instance == null)
                return -1;

            return instance.positifHeatScoreGain;
        }

        public static float GetNegatifHeatScoreGain()
        {
            if (instance == null)
                return -1;

            return instance.negatifHeatScoreGain;
        }

        public static float GetRatioPerDegreeGap()
        {
            if (instance == null)
                return -1;

            return instance.ratioGainDegreeGap;
        }


        #endregion

    }
}
