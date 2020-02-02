
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public static class Utility
{
    public static Tweener DOTextInt (this Text text, int initialValue, int finalValue, float duration, Func<int, string> convertor) {
        return DOTween.To (
            () => initialValue,
            it => text.text = convertor (it),
            finalValue,
            duration
        );
    }

    public static Tweener DOTextInt (this Text text, int initialValue, int finalValue, float duration) {
        return Utility.DOTextInt (text, initialValue, finalValue, duration, it => it.ToString ());
    }
}
