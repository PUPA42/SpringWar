using System;
using SpringWar.player;
using UnityEngine;
namespace SpringWar.player
{
    public sealed class Robot : BasicPlayer
    {
        protected override void TransitToStandingAnimation()
        {
            m_playerAnimator.SetInteger("State", 0);
        }
        protected override void TransitToMovingAnimation()
        {
            m_playerAnimator.SetInteger("State",1);
        }

        protected override void TransitToNormalAttackAnimation()
        {
            throw new NotImplementedException();
        }

        protected override void TransitToUsingSkill_QAnimtion()
        {
            throw new NotImplementedException();
        }

        protected override void TransitToUsingSkill_WAnimation()
        {
            throw new NotImplementedException();
        }

        protected override void TransitToUsingSkill_EAnimation()
        {
            throw new NotImplementedException();
        }

        protected override void TransitToUsingSkill_RAnimation()
        {
            throw new NotImplementedException();
        }
    }
}
