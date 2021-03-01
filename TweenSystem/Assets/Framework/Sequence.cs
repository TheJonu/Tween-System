using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TweenSystem
{
    public class Sequence
    {
        private Tweener _tweener;
        private float _delay;


        public Sequence(Tweener tweener)
        {
            _tweener = tweener;
            _delay = 0;
        }


        //public float Delay => _delay;


        public Sequence Set() => Tween(Tweener.TweenName.Set);

        public Sequence Show() => Tween(Tweener.TweenName.Show);

        public Sequence Hide() => Tween(Tweener.TweenName.Hide);

        public Sequence Tween(Tweener.TweenName type)
        {
            _tweener.Tween(type, _delay);
            _delay += _tweener.GetState(type).Time;
            return this;
        }

        public Sequence Call(Action action)
        {
            _tweener.Call(action, _delay);
            return this;
        }

        public Sequence Wait(float time)
        {
            _delay += time;
            return this;
        }
    }
}