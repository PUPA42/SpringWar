using UnityEngine;
using KaiTool.Utilites;
using SpringWar.player;

namespace SpringWar {
    public class GameManager : Singleton<GameManager> {
        public const float unitSpeed = 3f;
        private const float m_JudgeHealthInterval = 0.1f;
        private const float m_JudgeHealthDuration = 10f;
        [Header("SubManager")]
        public BasicPlayer player;
        public void CauseNormalAttackDamage(INormalAttackableObject attacker,IAttackedObject target) {
            target.HP -= (attacker.NormalAttack - target.PhysicalDefense + attacker.PhysicalPenetration);
        }
    }
}