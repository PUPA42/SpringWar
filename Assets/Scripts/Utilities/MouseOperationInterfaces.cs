using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KaiTool.MouseOperations
{
#region ISelectedObject
    public struct SelectedObjectEventArgs {

    }
    public interface ISelectedObject {
        GameObject GetGameObject();
        void OnSelected(UnityEngine.Object sender, SelectedObjectEventArgs e);
    }
    #endregion
#region IClickedObject
    public struct ClickedObjectEventArgs {
        public Vector3 clickedPoint;
        public ClickedObjectEventArgs(Vector3 point) {
            clickedPoint = point;
        }
    }
    public interface IClickedObject {
        GameObject GetGameObject();
        void OnClicked(UnityEngine.Object sender, ClickedObjectEventArgs e);
    }
    public interface IClickableSurface : IClickedObject {
        Vector3 GetClickPoint();

    }
    #endregion
#region IHoveredObject
    public struct HoveredObjectEventArgs { }
    public interface IHoveredObject {
        void OnHoverIn(UnityEngine.Object sender, HoveredObjectEventArgs e);
        void OnHoverStay(UnityEngine.Object sender, HoveredObjectEventArgs e);
        void OnHoverOut(UnityEngine.Object sender, HoveredObjectEventArgs e);
    }
#endregion

}
namespace SpringWar
{
    #region IMoveableObject
    public struct MoveableObjectEventArgs
    {
        public Vector3 navDestination;
        public GameObject TracingObject;
    }
    public interface IMoveableObject {
        void OnStartStanding(UnityEngine.Object sender, MoveableObjectEventArgs e);

        void OnStanding(UnityEngine.Object sender, MoveableObjectEventArgs e);

        void OnStopStanding(UnityEngine.Object sender, MoveableObjectEventArgs e);

        void OnStartMoving(UnityEngine.Object sender, MoveableObjectEventArgs e);

        void OnMoving(UnityEngine.Object sender, MoveableObjectEventArgs e);

        void OnStopMoving(UnityEngine.Object sender, MoveableObjectEventArgs e);

    }
    #endregion
    #region IAttackedObject
    public struct AttackedObjectEventArgs {
        public float attackDuration;
    }
    public enum EnumDamageType {
        Physical,
        Magical,
        Real
    }
    [Serializable]
    public struct Damage {//Instant Damage
        public EnumDamageType damageType;
        public float damageValue;
        public Damage(EnumDamageType type,float value) {
            this.damageType = type;
            this.damageValue = value;
        }
    }
    public interface IAttackedObject {
        float HP { get; set; }
        float PhysicalDefense { get; set; }
        float MagicalDefense { get; set; }
        void OnStartBeingAttacked(UnityEngine.Object sender, AttackedObjectEventArgs e);
        void OnBeingAttacked(UnityEngine.Object sender, AttackedObjectEventArgs e);
        void OnStopBeingAttacked(UnityEngine.Object sender, AttackedObjectEventArgs e);
        void OnKilled(UnityEngine.Object sender, AttackedObjectEventArgs e);

        bool IfHPGreaterThanZero();
    }
    #endregion
    #region INormalAttackableObject
    public struct NormalAttackableObjectEventArgs
    {
        public IAttackedObject target;
        public INormalAttackableObject attacker;
    }
    public interface INormalAttackableObject {
        float NormalAttack { get; set; }
        float NormalAttackDistance { get; set; }
        float NormalAttackFrequency{get;set;}
        float PhysicalPenetration { get; set; }

        bool IsNormalAttacking { get;}
        void OnStartNormalAttacking(UnityEngine.Object sender, NormalAttackableObjectEventArgs e);

        void OnNormalAttacking(UnityEngine.Object sender, NormalAttackableObjectEventArgs e);

        void OnStopNormalAttacking(UnityEngine.Object sender, NormalAttackableObjectEventArgs e);

    }
    #endregion
    #region IUsingSkillObject
    public struct UsingSkillObjectEventArgs
    {

    }
    public interface IUsingSkillObject {
        void OnStartUsingSkill(UnityEngine.Object sender, UsingSkillObjectEventArgs e);
        void OnUsingSkill(UnityEngine.Object sender, UsingSkillObjectEventArgs e);
        void OnStopUsingSkill(UnityEngine.Object sender, UsingSkillObjectEventArgs e);
    }
#endregion
}
