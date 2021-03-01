using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TweenSystem
{
    /// <summary>
    /// Tweener for UI panels.
    /// Can tween X position, Y position and alpha (transparency).
    /// The tweening is executed by TweenTimers which are spawned for each tweening.
    /// </summary>
    public class Tweener : MonoBehaviour
    {
        [Serializable]
        public class TweenDesc
        {
            [Tooltip("X position start and end")]
            [SerializeField] private Vector2 _xPos;
            [Tooltip("Y position start and end")]
            [SerializeField] private Vector2 _yPos;
            [Tooltip("Alpha value start and end")]
            [SerializeField] private Vector2 _alpha;
            [Tooltip("Tween time")]
            [SerializeField] private float _time;
            [Tooltip("Type of tween easing")]
            [SerializeField] private TweenSystem.EaseType _ease;

            public Vector2 XPos => _xPos;
            public Vector2 YPos => _yPos;
            public Vector2 Alpha => _alpha;
            public float Time => _time;
            public TweenSystem.EaseType Ease => _ease;
        }

        public enum TweenName
        {
            Set, Show, Hide
        }

        [Header("References")]
        [Tooltip("Set this if you want to tween a UI object")]
        [SerializeField] private RectTransform _rectTransform;
        [Tooltip("Set this if you want to tween the object's alpha (transparency)")]
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Prefabs")]
        [Tooltip("Prefab with a TweenTimer script on it")]
        [SerializeField] private Timer _timerPrefab;

        [Header("Settings")]
        [Tooltip("If the objects should move in the X axis")]
        [SerializeField] private bool _tweenX;
        [Tooltip("If the objects should move in the Y axis")]
        [SerializeField] private bool _tweenY;
        [Tooltip("If the object's alpha (transparency) should be tweened")]
        [SerializeField] private bool _tweenAlpha;

        [Header("States")]
        [SerializeField] private TweenDesc _set;
        [SerializeField] private TweenDesc _show;
        [SerializeField] private TweenDesc _hide;


        private List<Timer> _timers = new List<Timer>();


        public RectTransform RectTransform => _rectTransform;
        public CanvasGroup CanvasGroup => _canvasGroup;


        // INTERFACE FOR USERS

        public Sequence Begin()
        {
            for(int i = _timers.Count - 1; i >= 0; i--)
            {
                var timer = _timers[i];
                if(timer != null)
                {
                    timer.Stop();
                    timer.End();
                }
            }
            return new Sequence(this);
        }


        // INTERFACE FOR SEQUENCE

        public void Tween(TweenName stateName, float delay)
        {
            TweenDesc tween = GetState(stateName);

            //if (tween.Time != 0)
            //{
            //    Timer timer = Instantiate(_timerPrefab, Vector2.zero, Quaternion.identity, transform);
            //    _timers.Add(timer);
            //    timer.Set(this, delay).BeginTween(tween);
            //}
            //else
            //{
            //    ApplyTweenImmediately(tween);
            //}

            Timer timer = Instantiate(_timerPrefab, Vector2.zero, Quaternion.identity, transform);
            _timers.Add(timer);
            timer.Set(this, delay).BeginTween(tween);
        }

        public void Call(Action action, float delay)
        {
            Timer timer = Instantiate(_timerPrefab, Vector2.zero, Quaternion.identity, transform);
            _timers.Add(timer);
            timer.Set(this, delay).BeginCallback(action);
        }

        public TweenDesc GetState(TweenName stateName)
        {
            return stateName switch
            {
                TweenName.Set => _set,
                TweenName.Show => _show,
                TweenName.Hide => _hide,
                _ => _set
            };
        }


        // INTERFACE FOR TIMERS

        public void ApplyTween(float lerp, Func<float, float> tweenFunc, Vector2 posXBounds, Vector2 posYBounds, Vector2 alphaBounds)
        {
            float posX = _tweenX ? GetValue(posXBounds.x, posXBounds.y, tweenFunc, lerp) : _rectTransform.anchoredPosition.x;
            float posY = _tweenY ? GetValue(posYBounds.x, posYBounds.y, tweenFunc, lerp) : _rectTransform.anchoredPosition.y;
            float alpha = _tweenAlpha ? GetValue(alphaBounds.x, alphaBounds.y, tweenFunc, lerp) : _canvasGroup.alpha;

            _rectTransform.anchoredPosition = new Vector2(posX, posY);
            _canvasGroup.alpha = alpha;
        }

        public void ApplyTween(TweenDesc tween, Func<float, float> func, float lerp)
        {
            float posX = _tweenX ? GetValue(tween.XPos, func, lerp) : _rectTransform.anchoredPosition.x;
            float posY = _tweenY ? GetValue(tween.YPos, func, lerp) : _rectTransform.anchoredPosition.y;
            float alpha = _tweenAlpha ? GetValue(tween.Alpha, func, lerp) : _canvasGroup.alpha;

            _rectTransform.anchoredPosition = new Vector2(posX, posY);
            _canvasGroup.alpha = alpha;
        }

        public void UnregisterTimer(Timer timer)
        {
            _timers.Remove(timer);
        }


        // PRIVATE METHODS

        private void ApplyTweenImmediately(TweenDesc state)
        {
            float posX = _tweenX ? state.XPos.y : _rectTransform.anchoredPosition.x;
            float posY = _tweenY ? state.YPos.y : _rectTransform.anchoredPosition.y;
            float alpha = _tweenAlpha ? state.Alpha.y : _canvasGroup.alpha;

            _rectTransform.anchoredPosition = new Vector2(posX, posY);
            _canvasGroup.alpha = alpha;
        }

        private float GetValue(float start, float end, Func<float, float> ease, float lerp)
        {
            return start + ease(lerp) * (end - start);
        }

        private float GetValue(Vector2 startEnd, Func<float, float> func, float lerp)
        {
            return startEnd.x + func(lerp) * (startEnd.y - startEnd.x);
        }
    }
}