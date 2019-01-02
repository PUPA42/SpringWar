using SpringWar.player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Yuka : BasicPlayer
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
        int randomNum = UnityEngine.Random.Range(2, 5);
        if (m_playerAnimator.GetInteger("State")==randomNum) {
            randomNum += 2;
            randomNum %= 3;
            randomNum += 2;
        }
        m_playerAnimator.SetInteger("State", randomNum);
    }
    protected override void TransitToUsingSkill_EAnimation()
    {
        throw new NotImplementedException();
    }

    protected override void TransitToUsingSkill_QAnimtion()
    {
        throw new NotImplementedException();
    }

    protected override void TransitToUsingSkill_RAnimation()
    {
        throw new NotImplementedException();
    }

    protected override void TransitToUsingSkill_WAnimation()
    {
        throw new NotImplementedException();
    }
}
