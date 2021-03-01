using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TweenSystem
{
    public static class TweenSystem
    {
        public enum EaseType
        {
            Linear,
            ElasticIn, ElasticOut,
            SquareIn, SquareOut
        }

        public static Func<float, float> GetFunc(this EaseType ease)
        {
            return ease switch
            {
                EaseType.Linear => Linear,
                EaseType.ElasticOut => ElasticIn,
                EaseType.SquareIn => SquareIn,
                EaseType.SquareOut => SquareOut,
                _ => Linear
            };
        }


        // EASE FUNCTIONS

        public static float Linear(float val) => val;

        public static float ElasticIn(float val)
        {
            float overshoot = 0.33f;
            float period = 0.33f;

            if (val == 0f || val == 1f) 
                return val;

            float s = 0f;
            float a = 0f;

            if (a < 1)
                s = period / 4f;
            else
                s = period / (2f * Mathf.PI) * Mathf.Asin(1 / a);

            a = Mathf.Max(1, a);

            if (overshoot > 1f && val < 0.4f)
                overshoot = 1f + (val / 0.4f * (overshoot - 1f));

            return 1 + a * Mathf.Pow(2f, -10f * val) * Mathf.Sin((val - s) * (2f * Mathf.PI) / period) * overshoot;
        }

        public static float SquareOut(float val) => val != 0 ? Mathf.Pow(val, 2) : 0;

        public static float SquareIn(float val) => val != 0 ? Mathf.Sqrt(val) : 0;
    }
}