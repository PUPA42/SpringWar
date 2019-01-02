using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpringWar.Enemy
{
    public class Zombie : BasicEnemy
    {
        public override void Init()
        {
            base.Init();
            InitEvent();
        }

        protected override void TransitToTargetAnimationState(EnumEnemyState targetState) {
            m_PreviousState = m_CurrentState;
            m_CurrentState = targetState;
            if (m_AnimationTransitionCoroutine!=null) {
                StopCoroutine(m_AnimationTransitionCoroutine);
            }
            m_CharacterAnimator.SetInteger("State",g_EnermyAnimationStateDic[m_CurrentState]);
        }
    

        private void InitEvent() {
           
        }

     
    }
}