using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KaiTool.BatchInitialization;
using KaiTool.MouseOperations;
using System;
using SpringWar.player;
using UnityEngine.AI;

namespace SpringWar.Enemy{ 
    [RequireComponent(typeof(NavMeshAgent))]
public abstract class BasicEnemy : KaiTool_BatchedInitializedBehaviour,IClickedObject,ISelectedObject,IAttackedObject,INormalAttackableObject{
        protected static Dictionary<EnumEnemyState,int> g_EnermyAnimationStateDic = new Dictionary<EnumEnemyState, int>() {
            { EnumEnemyState.Idle,0},{ EnumEnemyState.Walk,1},{ EnumEnemyState.Run,2},{ EnumEnemyState.AttackA,3},
            { EnumEnemyState.AttackB,4},{ EnumEnemyState.Damaged,5},{ EnumEnemyState.Down,6},
            { EnumEnemyState.Death,7}
        };
        [SerializeField]
        protected Animator m_CharacterAnimator;
        
        [SerializeField]
        protected EnumEnemyState m_CurrentState=EnumEnemyState.Idle;
        protected EnumEnemyState m_PreviousState;
        

        [Header("AttackingProperties")]
        [SerializeField]
        protected float m_normalAttack = 10f;
        [SerializeField]
        protected float m_normalAttackDistance = 1f;
        [SerializeField]
        protected float m_normalAttackFrequency = 1f;
        [SerializeField]
        protected float m_physicalPenetration = 5f;
        [Header("DefenseProperties")]
        [SerializeField]
        protected float m_HP=100f;
        [SerializeField]
        protected float m_physicalDefense=10f;
        [SerializeField]
        protected float m_magicalDefense = 10f;
        [Header("IEnumeratorProperties")]
        [SerializeField]
        protected float m_ienumeratorDeltatime = 0.05f;
        //****************************//
        protected Coroutine m_AnimationTransitionCoroutine;
        protected Coroutine m_CurrentMotionCoroutine;
        public override void Init()
        {
            base.Init();
            InitVar();
            InitEvent();
        }
        private void InitVar() {
        }
        private void InitEvent() {
            Clicked += (sender,e) =>{
                MoveableObjectEventArgs args = new MoveableObjectEventArgs();
                args.navDestination = transform.position;
                args.TracingObject = gameObject;
                GameManager.Instance.player.OnStartMoving(this,args);
            };
            Selected += (sender, e) => { };
            StartBeingAttacked += (sender,e) => {
              //  print("Attacked!!");
                StartCoroutine(BeingAttackedIEnumerator(sender,e));
            };
            BeingAttacked += (sender, e) => {
                // TransitToTargetAnimationState(EnumEnemyAnimationState.Damaged);
              //  TransitToTargetStateThenTransitToIdle(EnumEnemyAnimationState.Damaged,2f);
            };
            StopBeingAttacked += (sender,e) => {
               // print("StopBeingAttacked!");
                ChangeState(EnumEnemyState.Idle);
                TransitToTargetAnimationState(m_CurrentState);
            };
            Killed += (sender, e) => {
                //print("Killed!!");
                StartCoroutine(KilledIEnumerator(sender,e));
            };

            StartNormalAttacking += (sender,e) => { };
            NormalAttacking += (sender, e) => { };
            StopNormalAttacking += (sender, e) => { };
        }
        public GameObject GetGameObject()
        {
            return gameObject;
        }
        public Action<UnityEngine.Object,AttackedObjectEventArgs> StartBeingAttacked;
        public Action<UnityEngine.Object, AttackedObjectEventArgs> BeingAttacked;
        public Action<UnityEngine.Object, AttackedObjectEventArgs> StopBeingAttacked;
        public Action<UnityEngine.Object, AttackedObjectEventArgs> Killed;
        public Action<UnityEngine.Object, ClickedObjectEventArgs> Clicked;
        public Action<UnityEngine.Object, SelectedObjectEventArgs> Selected;

        public Action<UnityEngine.Object, NormalAttackableObjectEventArgs> StartNormalAttacking;
        public Action<UnityEngine.Object, NormalAttackableObjectEventArgs> NormalAttacking;
        public Action<UnityEngine.Object, NormalAttackableObjectEventArgs> StopNormalAttacking;

        public float HP
        {
            get
            {
                return m_HP;
            }

            set
            {
                m_HP = value;
            }
        }

        public float PhysicalDefense
        {
            get
            {
                return m_physicalDefense;
            }

            set
            {
                m_physicalDefense = value;
            }
        }

        public float MagicalDefense
        {
            get
            {
                return m_magicalDefense;
            }

            set
            {
                m_magicalDefense = value;
            }
        }

        public float NormalAttack
        {
            get
            {
                return m_normalAttack;
            }

            set
            {
                m_normalAttack = value;
            }
        }

        public float NormalAttackDistance
        {
            get
            {
                return m_normalAttackDistance;
            }

            set
            {
                m_normalAttackDistance = value;
            }
        }

        public float NormalAttackFrequency
        {
            get
            {
                return m_normalAttackFrequency;
            }

            set
            {
                m_normalAttackFrequency = value;
            }
        }

        public float PhysicalPenetration
        {
            get
            {
                return m_physicalPenetration;
            }

            set
            {
                m_physicalPenetration = value;
            }
        }

        public bool IsNormalAttacking
        {
            get
            {
                if ((m_CurrentState & (EnumEnemyState.AttackA | EnumEnemyState.AttackB)) != 0)
                {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        public void OnStartBeingAttacked(UnityEngine.Object sender, AttackedObjectEventArgs e)
        {
            if (StartBeingAttacked != null)
            {
                StartBeingAttacked(sender, e);
            }
        }
        public void OnBeingAttacked(UnityEngine.Object sender, AttackedObjectEventArgs e)
        {
            if (BeingAttacked!=null) {
                BeingAttacked(sender,e);
            }
        }
        public void OnStopBeingAttacked(UnityEngine.Object sender, AttackedObjectEventArgs e)
        {
            //throw new NotImplementedException();
            if (StopBeingAttacked!=null) {
                StopBeingAttacked(sender,e);
            }
        }

        public void OnClicked(UnityEngine.Object sender, ClickedObjectEventArgs e)
        {
            if (Clicked!=null) {
                Clicked(sender,e);
            }
        }

        public void OnSelected(UnityEngine.Object sender, SelectedObjectEventArgs e)
        {
            if (Selected!=null) {
                Selected(sender,e);
            }
        }

        public void OnStartNormalAttacking(UnityEngine.Object sender, NormalAttackableObjectEventArgs e)
        {
            if (StartNormalAttacking!=null) {
                StartNormalAttacking(sender,e);
            }
        }

        public void OnNormalAttacking(UnityEngine.Object sender, NormalAttackableObjectEventArgs e)
        {
            if (NormalAttacking!=null) {
                NormalAttacking(sender, e);
            }
        }

        public void OnStopNormalAttacking(UnityEngine.Object sender, NormalAttackableObjectEventArgs e)
        {
            if (StopNormalAttacking!=null) {
                StopNormalAttacking(sender,e);
            }
        }

        public void OnKilled(UnityEngine.Object sender, AttackedObjectEventArgs e)
        {
            if (Killed!=null) {
                Killed(sender,e);
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            INormalAttackableObject attacker = other.GetComponentInParent<INormalAttackableObject>();
            AttackedObjectEventArgs attackArgs = new AttackedObjectEventArgs();
            attackArgs.attackDuration = 1 / attacker.NormalAttackFrequency;
            if (attacker!=null&&attacker.IsNormalAttacking) {
                if (IfHPGreaterThanZero())
                {

                    if ((m_CurrentState & (EnumEnemyState.Idle)) != 0)
                    {
                        OnStartBeingAttacked(this, attackArgs);
                    }
                }
              
            }
        }

        protected abstract void TransitToTargetAnimationState(EnumEnemyState targetState);
        private void ChangeState(EnumEnemyState targetState) {
            m_PreviousState = m_CurrentState;
            m_CurrentState = targetState;
        }
        public bool IfHPGreaterThanZero()
        {
            if (m_HP > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }

     

        private IEnumerator BeingAttackedIEnumerator(UnityEngine.Object sender,AttackedObjectEventArgs e) {
            WaitForSeconds wait = new WaitForSeconds(m_ienumeratorDeltatime);
            var time = 0f;
            var hasTransitToDamaged = false;
            while (time<e.attackDuration/2) {
                if (time>e.attackDuration/10&&!hasTransitToDamaged) {
                    ChangeState(EnumEnemyState.Damaged);
                    TransitToTargetAnimationState(m_CurrentState);
                    hasTransitToDamaged = true;
                }
                time += m_ienumeratorDeltatime;
                OnBeingAttacked(sender,e);
                yield return wait;
            }
            OnStopBeingAttacked(sender,e);
        }

        private IEnumerator KilledIEnumerator(UnityEngine.Object sender,AttackedObjectEventArgs e) {
            ChangeState(EnumEnemyState.Down);
            TransitToTargetAnimationState(m_CurrentState);
            yield return new WaitForSeconds(0.5f);
            ChangeState(EnumEnemyState.Death);
            TransitToTargetAnimationState(m_CurrentState);
        }
    }

    [Flags]
    public enum EnumEnemyState {
        Idle=1,
        Walk=2,
        Run=4,
        AttackA=8,
        AttackB=16,
        Damaged=32,
        Down=64,
        Death=128
    }
}