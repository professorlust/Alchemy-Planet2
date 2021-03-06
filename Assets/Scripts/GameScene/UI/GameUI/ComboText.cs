﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace AlchemyPlanet.GameScene
{
    public class ComboText : MonoBehaviour
    {
        public static ComboText Instance { get; set; }

        private Text text;
        private float fadeOutSecond;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            text = GetComponent<Text>();
            fadeOutSecond = 3;
        }

        public void SetOpacity(float alpha)
        {
            alpha = Mathf.Clamp(alpha, 0, 1);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        }

        public void FadeOut()
        {
            StopCoroutine("FadeOutCoroutine");
            StartCoroutine("FadeOutCoroutine");

            Sequence sq = DOTween.Sequence();
            sq.Append(transform.DOScale(1.5f, 0.1f).SetEase(Ease.OutQuint));
            sq.Append(transform.DOScale(1, 0.2f).SetEase(Ease.InSine));
        }

        IEnumerator FadeOutCoroutine()
        {
            while (text.color.a > 0)
            {
                SetOpacity(text.color.a - (Time.deltaTime / fadeOutSecond));
                yield return new WaitForEndOfFrame();
            }

            GameManager.Instance.Combo = 0;
        }
    }
}