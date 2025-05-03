using Landers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LanderFighter
{
    public class LanderDamageEventArgs : EventArgs
    {
        public ushort PvBefore { get; }
        public ushort PvAfter { get; }

        public LanderDamageEventArgs(ushort pvBefore, ushort pvAfter)
        {
            PvBefore = pvBefore;
            PvAfter = pvAfter;
        }
    }

    public static class BattleLanderUtils
    {
        private static readonly Dictionary<string, EventHandler> OnLanderTakeDamage = new Dictionary<string, EventHandler>();

        public static void TakeDamage(this LanderData data, ushort damages)
        {
            if (data == null) return;

            ushort pvBefore = data.Pv;
            data.Pv = (ushort)Mathf.Max(data.Pv - damages, 0);

            if (OnLanderTakeDamage.ContainsKey(data.Tag))
            {
                OnLanderTakeDamage[data.Tag]?.Invoke(data, new LanderDamageEventArgs(pvBefore, data.Pv));
            }
        }

        public static bool IsAlive(this LanderData data)
        {
            return data.Pv > 0;
        }

        public static void SubscribeLanderTakeDamage(this LanderData data, EventHandler eventHandler)
        {
            if (data == null || eventHandler == null) return;

            if (!OnLanderTakeDamage.ContainsKey(data.Tag))
            {
                OnLanderTakeDamage.Add(data.Tag, eventHandler);
            }
            else
            {
                OnLanderTakeDamage[data.Tag] += eventHandler;
            }
        }

        public static void UnsubscribeLanderTakeDamage(this LanderData data, EventHandler eventHandler)
        {
            if (data == null || eventHandler == null) return;

            if (OnLanderTakeDamage.ContainsKey(data.Tag))
            {
                OnLanderTakeDamage[data.Tag] -= eventHandler;

                if (OnLanderTakeDamage[data.Tag] == null)
                {
                    OnLanderTakeDamage.Remove(data.Tag);
                }
            }
        }
    }
}
