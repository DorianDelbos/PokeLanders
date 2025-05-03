using Landers;
using Landers.API;
using UnityEngine;

namespace LanderFighter
{
    public class BattleAttackListInfo : MonoBehaviour
    {
        [SerializeField] private Transform AttackListTransform;
        [SerializeField] private BattleAttackInfo AttackInfoPrefab;

        public bool Enable
        {
            get => AttackListTransform.gameObject.activeSelf;
            set => AttackListTransform.gameObject.SetActive(value);
        }

        public void InitializeAttacks(LanderData data)
        {
            foreach (Transform item in AttackListTransform)
            {
                Destroy(item.gameObject);
            }

            foreach (ushort moveId in data.Moves)
            {
                if (moveId <= 0) break;

                Move move = MoveRepository.Instance.GetById(moveId);

                BattleAttackInfo attackInfo = Instantiate(AttackInfoPrefab, AttackListTransform);
                attackInfo.InitializeData(data, move);
            }
        }
    }
}
