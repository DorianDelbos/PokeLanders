using Landers;
using Landers.Utils;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LanderFighter
{
    public class BattleInfo : MonoBehaviour, IDisposable
    {
        [SerializeField] private TMP_Text nameMesh;
        [SerializeField] private TMP_Text levelMesh;
        [SerializeField] private TMP_Text pvMesh;
        [SerializeField] private GameObject maleImage;
        [SerializeField] private GameObject femaleImage;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private Slider xpBar;

        private LanderData m_data;
        private Coroutine m_routine;

        public void IntializeData(LanderData data)
        {
            if (data == null) return;
            m_data = data;

            m_data.SubscribeLanderTakeDamage(OnTakeDamage);

            nameMesh.text = m_data.Name;
            levelMesh.text = $"Lv.{m_data.Level}";
            maleImage.SetActive(m_data.IsMale);
            femaleImage.SetActive(!m_data.IsMale);
            int xpFrom = LanderUtils.GetXpByLevel(m_data.Level, m_data.BaseXp);
            int xpTo = LanderUtils.GetXpByLevel((byte)(m_data.Level + 1), m_data.BaseXp);
            xpBar.value = xpTo - xpFrom;

            UpdatePvDisplay(m_data.Pv, m_data.MaxHp);
        }

        private IEnumerator UpdatePvDisplay(ushort pvFrom, ushort pvTo, ushort pvMax, float duration = 2.0f)
        {
            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                elapsed = Mathf.Clamp(elapsed + Time.deltaTime, 0, duration);
                float factor = elapsed / duration;
                ushort result = (ushort)Mathf.Lerp(pvFrom, pvTo, factor);
                UpdatePvDisplay(result, pvMax);

                yield return null;
            }

            m_routine = null;
        }

        private void UpdatePvDisplay(ushort pv, ushort pvMax)
        {
            pvMesh.text = $"{pv}/{pvMax}";
            healthBar.SetHealth(pv, pvMax);
        }

        private void OnTakeDamage(object sender, EventArgs e)
        {
            if (sender is not LanderData data || e is not LanderDamageEventArgs DamageEventArgs) return;

            if (m_routine != null)
            {
                StopCoroutine(m_routine);
                UpdatePvDisplay(DamageEventArgs.PvBefore, data.MaxHp);
            }

            m_routine = StartCoroutine(UpdatePvDisplay(DamageEventArgs.PvBefore, DamageEventArgs.PvAfter, data.MaxHp));
        }

        public void Dispose()
        {
            m_data.UnsubscribeLanderTakeDamage(OnTakeDamage);
        }
    }
}
