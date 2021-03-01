using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TweenSystem
{
    public class Timer : MonoBehaviour
    {
        private Tweener _tweener;
        private float _delay;
        private float _delayTimer;
        private Coroutine _coroutine;

        // tween
        private Tweener.TweenDesc _state;
        private Func<float, float> _tweenFunc;
        private float _tweenTimer;

        // action
        private Action _action;


        public float TweenTime => _state != null ? _state.Time : 0;
        public Tweener.TweenDesc State => _state;


        public Timer Set(Tweener tweener, float delay)
        {
            _tweener = tweener;
            _delay = delay;
            return this;
        }

        public void BeginTween(Tweener.TweenDesc state)
        {
            _state = state;
            _tweenFunc = TweenSystem.GetFunc(state.Ease);

            _coroutine = StartCoroutine("TweenCoroutine");
        }

        public void BeginCallback(Action action)
        {
            _action = action;

            _coroutine = StartCoroutine("ActionCoroutine");
        }

        public void Stop()
        {
            StopCoroutine(_coroutine);
        }

        public void End()
        {
            _tweener.UnregisterTimer(this);
            Destroy(gameObject);
        }

        private IEnumerator TweenCoroutine()
        {
            // delay
            _delayTimer = 0;
            while (_delayTimer < _delay)
            {
                _delayTimer += Time.deltaTime;
                yield return null;
            }

            // get current state
            //Vector2 pos = _tweener.RectTransform.anchoredPosition;
            //float alpha = _tweener.CanvasGroup.alpha;
            //Vector2 posXBounds = new Vector2(pos.x, _state.EndPosition.x);
            //Vector2 posYBounds = new Vector2(pos.y, _state.EndPosition.y);
            //Vector2 alphaBounds = new Vector2(alpha, _state.Alpha);

            // tweening
            if(_state.Time != 0)
            {
                _tweenTimer = 0;
                do
                {
                    _tweenTimer = Mathf.Clamp(_tweenTimer + Time.deltaTime, 0, _state.Time);
                    //_tweener.ApplyTween(_tweenTimer / _state.Time, _tweenFunc, posXBounds, posYBounds, alphaBounds
                    _tweener.ApplyTween(_state, _tweenFunc, _tweenTimer / _state.Time);
                    yield return null;
                }
                while (_tweenTimer < _state.Time);
            }
            else
            {
                _tweener.ApplyTween(_state, _tweenFunc, 1);
            }

            // end
            End();

            yield break;
        }

        private IEnumerator ActionCoroutine()
        {
            // delay
            _delayTimer = 0;
            while (_delayTimer < _delay)
            {
                _delayTimer += Time.deltaTime;
                yield return null;
            }

            // action
            _action?.Invoke();

            // end
            End();

            yield break;
        }
    }
}