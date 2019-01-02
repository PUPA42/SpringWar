using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KaiTool.BatchInitialization;
using KaiTool.MouseOperations;
using System;
using UnityEngine.AI;
namespace SpringWar.player {
    public enum EnumPlayerState{
        Standing,
        Moving,
        NormalAttacking,
        UsingSkill
    }

  
 
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class BasicPlayer : KaiTool_BatchedInitializedBehaviour, ISelectedObject,IMoveableObject,INormalAttackableObject,IUsingSkillObject {
        [SerializeField]
        protected EnumPlayerState m_currentState;
        protected EnumPlayerState m_previousState;
        [SerializeField]
        protected GameObject m_currentTracingObject;
        protected GameObject m_previousTracingObject;
        [Header("MotionProperties")]
        public float m_movingSpeed = 4f;
        private float m_stopDistance = 0f;
        private NavMeshAgent m_navAgent;
        [Header("AttackingProperties")]
        [SerializeField]
        private float m_normalAttack = 10f;
        [SerializeField]
        private float m_normalAttackDistance = 1f;
        [SerializeField]
        private float m_normalAttackFrequency = 1f;
        [SerializeField]
        private float m_physicalPenetration = 5f;
        // public float normalAttackIntervalTime = 0.2f;
        [Header("AnimatorProperties")]
        [SerializeField]
        protected Animator m_playerAnimator;
        private Coroutine m_currentMotionCoroutine = null;
        [Header("IEnumeratorProperties")]
        [SerializeField]
        protected float m_ienumeratorDeltatime = 0.05f;


        public Action<UnityEngine.Object, SelectedObjectEventArgs> Selected;

        public Action<UnityEngine.Object, MoveableObjectEventArgs> StartStanding;
        public Action<UnityEngine.Object, MoveableObjectEventArgs> Standing;
        public Action<UnityEngine.Object, MoveableObjectEventArgs> StopStanding;

        public Action<UnityEngine.Object, MoveableObjectEventArgs> StartMoving;
        public Action<UnityEngine.Object, MoveableObjectEventArgs> Moving;
        public Action<UnityEngine.Object, MoveableObjectEventArgs> StopMoving;

        public Action<UnityEngine.Object, NormalAttackableObjectEventArgs> StartNormalAttacking;
        public Action<UnityEngine.Object, NormalAttackableObjectEventArgs> NormalAttacking;
        public Action<UnityEngine.Object, NormalAttackableObjectEventArgs> StopNormalAttacking;

        public Action<UnityEngine.Object, UsingSkillObjectEventArgs> StartUsingSkill;
        public Action<UnityEngine.Object, UsingSkillObjectEventArgs> UsingSkill;
        public Action<UnityEngine.Object, UsingSkillObjectEventArgs> StopUsingSkill;

        public NavMeshAgent NavAgent
        {
            get
            {
                return m_navAgent;
            }

            set
            {
                m_navAgent = value;
            }
        }

        public EnumPlayerState CurrentState
        {
            get
            {
                return m_currentState;
            }

            set
            {
                m_currentState = value;
            }
        }

        public EnumPlayerState PreviousState
        {
            get
            {
                return m_previousState;
            }

            set
            {
                m_previousState = value;
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
                if (CurrentState == EnumPlayerState.NormalAttacking)
                {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        public void OnStartStanding(UnityEngine.Object sender, MoveableObjectEventArgs e) {
            if (StartStanding != null) {
                StartStanding(sender, e);
            }
        }
        public void OnStanding(UnityEngine.Object sender, MoveableObjectEventArgs e) {
            if (Standing != null) {
                Standing(sender, e);
            }
        }
        public void OnStopStanding(UnityEngine.Object sender, MoveableObjectEventArgs e) {
            if (StopStanding != null) {
                StopStanding(sender, e);
            }
        }
        public void OnStartMoving(UnityEngine.Object sender, MoveableObjectEventArgs e) {
            if (StartMoving != null) {
                StartMoving(sender, e);
            }
        }
        public void OnMoving(UnityEngine.Object sender, MoveableObjectEventArgs e) {
            if (Moving != null) {
                Moving(sender, e);
            }
        }
        public void OnStopMoving(UnityEngine.Object sender, MoveableObjectEventArgs e) {
            if (StopMoving != null) {
                StopMoving(sender, e);
            }
        }
        public void OnStartNormalAttacking(UnityEngine.Object sender, NormalAttackableObjectEventArgs e) {
           // print("StartNormalAttacking");
            if (StartNormalAttacking != null) {
                StartNormalAttacking(sender, e);
            }
        }
        public void OnNormalAttacking(UnityEngine.Object sender, NormalAttackableObjectEventArgs e) {
            if (NormalAttacking != null) {
                NormalAttacking(sender, e);
            }
        }
        public void OnStopNormalAttacking(UnityEngine.Object sender, NormalAttackableObjectEventArgs e) {
            if (StopNormalAttacking != null) {
                StopNormalAttacking(sender, e);
            }
        }
        public void OnStartUsingSkill(UnityEngine.Object sender, UsingSkillObjectEventArgs e) {
            if (StartUsingSkill != null) {
                StartUsingSkill(sender, e);
            }
        }
        public void OnUsingSkill(UnityEngine.Object sender, UsingSkillObjectEventArgs e) {
            if (UsingSkill != null) {
                UsingSkill(sender, e);
            }
        }
        public void OnStopUsingSkill(UnityEngine.Object sender, UsingSkillObjectEventArgs e) {
            if (StopUsingSkill != null) {
                StopUsingSkill(sender, e);
            }
        }
        public void OnSelected(UnityEngine.Object sender, SelectedObjectEventArgs e)
        {
            if (Selected != null)
            {
                Selected(sender, e);
            }
        }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }

        public override void Init()
        {
            base.Init();
            InitVar();
            InitEvent();
        }



        private void InitVar() {
            if (gameObject.layer != LayerMask.NameToLayer("Player")) {
                this.gameObject.layer = LayerMask.NameToLayer("Player");
            }
            if (NavAgent == null) {
                NavAgent = GetComponent<NavMeshAgent>();
            }
        }
        private void InitEvent()
        {
            Selected += (sender, e) => {
                InputManager.Instance.SelectedObject = this;
                //print(InputManager.Instance.SelectedObject.GetGameObject().name);
            };

            StartStanding += (sender, e) => {
                // print("StartStanding");
                TransitToStandingAnimation();
                ChangeState(EnumPlayerState.Standing);
                StartMotionCoroutine(StandingCoroutine());
            };
            Standing += (sender, e) => {
                if (m_currentTracingObject != null) {
                    if (GetDistance(m_currentTracingObject.transform.position,transform.position)>NormalAttackDistance) {
                        m_navAgent.destination = m_currentTracingObject.transform.position;
                    }
                    if (m_navAgent.pathPending == false) {
                        if (m_navAgent.remainingDistance > NormalAttackDistance)
                        {
                            MoveableObjectEventArgs args = new MoveableObjectEventArgs();
                            args.navDestination = m_currentTracingObject.transform.position;
                            args.TracingObject = m_currentTracingObject;
                            OnStartMoving(this, args);
                        }
                        /*
                        if (navAgent.remainingDistance <= normalAttackDistance)
                        {
                            navAgent.isStopped = true;
                            PlayerNormalAttackingEventArgs args = new PlayerNormalAttackingEventArgs();
                            args.attackingTarget = currentTracingObject.GetComponent<IAttackedObject>();
                            OnStartNormalAttacking(this, args);
                        }
                        */
                    }
                }
            };
            StopStanding += (sender, e) => { };

            StartMoving += (sender, e) =>
            {
                ChangeState(EnumPlayerState.Moving);
                TransitToMovingAnimation();
                m_navAgent.isStopped = false;
                MoveableObjectEventArgs args = ((MoveableObjectEventArgs)e);
                NavAgent.destination = args.navDestination;
                m_previousTracingObject = m_currentTracingObject;
                m_currentTracingObject = args.TracingObject;
                StartMotionCoroutine(MovingCoroutine());
            };
            Moving += (sender, e) => {
                // print("Moving");
                if (m_navAgent.isStopped==true) {
                    TransitToStandingAnimation();
                }else 
                if (m_playerAnimator.GetInteger("State") != 1)
                {
                    TransitToMovingAnimation();
                }

                if (m_currentTracingObject != null) {
                    m_navAgent.destination = m_currentTracingObject.transform.position;
                    if (m_navAgent.pathPending == false) {
                        if (m_navAgent.remainingDistance <= NormalAttackDistance )
                        {
                            m_navAgent.isStopped = true;
                            NormalAttackableObjectEventArgs args = new NormalAttackableObjectEventArgs();
                            args.target = m_currentTracingObject.GetComponent<IAttackedObject>();
                            args.attacker = this;
                            if (args.target.IfHPGreaterThanZero()) {
                                OnStartNormalAttacking(this, args);
                            }
                        }
                    }
                }

                if (m_currentTracingObject == null)
                {
                    if (m_navAgent.pathPending==false) {

                        if (m_navAgent.remainingDistance <= m_stopDistance )
                        {
                            MoveableObjectEventArgs args1 = new MoveableObjectEventArgs();
                            OnStartStanding(this, args1);
                        }
                    }
                }

            };
            StopMoving += (sender, e) => { };

            StartNormalAttacking += (sender, e) => {
                if (((NormalAttackableObjectEventArgs)e).attacker==null) {
                    Debug.LogError("Attacker is null!");
                }
                if (((NormalAttackableObjectEventArgs)e).target == null) {
                    Debug.LogError("Target is null!!");
                }
                ChangeState(EnumPlayerState.NormalAttacking);
                TransitToNormalAttackAnimation();
               
                StartMotionCoroutine(NornalAttackingCoroutine((NormalAttackableObjectEventArgs)e));
            };
            NormalAttacking += (sender, e) => {
                //print("Attacking!");
            };
            StopNormalAttacking += (sender, e) => {

            };

            StartUsingSkill += (sender, e) => {
                ChangeState(EnumPlayerState.UsingSkill);
            };
            UsingSkill += (sender, e) => { };
            StopUsingSkill += (sender, e) => { };

        }
        private void Reset()
        {
            NavAgent = GetComponent<NavMeshAgent>();
        }
        private void OnValidate()
        {
            GetComponent<NavMeshAgent>().speed = m_movingSpeed;
        }
        private void ChangeState(EnumPlayerState state) {
            PreviousState = CurrentState;
            switch (CurrentState) {
                case EnumPlayerState.Standing:
                    MoveableObjectEventArgs args0 = new MoveableObjectEventArgs();
                    OnStopStanding(this, args0);
                    break;
                case EnumPlayerState.Moving:
                    MoveableObjectEventArgs args1 = new MoveableObjectEventArgs();
                    OnStopMoving(this, args1);
                    break;
                case EnumPlayerState.NormalAttacking:
                    NormalAttackableObjectEventArgs args2 = new NormalAttackableObjectEventArgs();
                    OnStopNormalAttacking(this, args2);
                    break;
                case EnumPlayerState.UsingSkill:
                    UsingSkillObjectEventArgs args3 = new UsingSkillObjectEventArgs();
                    OnStopUsingSkill(this, args3);
                    break;
            }
            CurrentState = state;
        }
        protected abstract void TransitToStandingAnimation();
        protected abstract void TransitToMovingAnimation();
        protected abstract void TransitToNormalAttackAnimation();
        protected abstract void TransitToUsingSkill_QAnimtion();
        protected abstract void TransitToUsingSkill_WAnimation();
        protected abstract void TransitToUsingSkill_EAnimation();
        protected abstract void TransitToUsingSkill_RAnimation();

        private void StartMotionCoroutine(IEnumerator coroutine) {
            //print("Current == null" + currentMotionCoroutine == null);
            if (m_currentMotionCoroutine != null)
            {
                StopCoroutine(m_currentMotionCoroutine);
            }
            // StopAllCoroutines();
            // print("Current == null" + currentMotionCoroutine == null);
            m_currentMotionCoroutine = StartCoroutine(coroutine);
        }
        private IEnumerator StandingCoroutine() {

            WaitForSeconds wait = new WaitForSeconds(m_ienumeratorDeltatime);
            while (true) {
                MoveableObjectEventArgs args = new MoveableObjectEventArgs();
                OnStanding(this, args);
                yield return wait;
            }
        }
        private IEnumerator MovingCoroutine() {
            WaitForSeconds wait = new WaitForSeconds(m_ienumeratorDeltatime);
            while (true)
            {
                //print("OnMoving");
                MoveableObjectEventArgs args = new MoveableObjectEventArgs();
                OnMoving(this, args);
                yield return wait;
            }
        }
        private IEnumerator NornalAttackingCoroutine(NormalAttackableObjectEventArgs args)
        {
            // transform.LookAt(args.attackingTarget.transform);//
            FaceToTargetGradually(m_currentTracingObject,0.1f);
            WaitForSeconds wait = new WaitForSeconds(m_ienumeratorDeltatime);
            AttackedObjectEventArgs attackedArgs = new AttackedObjectEventArgs();
            attackedArgs.attackDuration = 1 / NormalAttackFrequency;
            var times = (int)((1 / NormalAttackFrequency) / m_ienumeratorDeltatime);
           // args.target.OnStartBeingAttacked(this, attackedArgs);
            for (int i = 0; i < times; i++) {
                OnNormalAttacking(this, args);
                yield return wait;
            }
            //print("Damage!!");
            GameManager.Instance.CauseNormalAttackDamage(args.attacker, args.target);
            if (!args.target.IfHPGreaterThanZero()) {
                args.target.OnKilled(this, attackedArgs);
            }

            if (m_currentTracingObject != null)
            {
                if (GetDistance(m_currentTracingObject.transform.position, transform.position) <= NormalAttackDistance)
                {
                    NormalAttackableObjectEventArgs args0 = new NormalAttackableObjectEventArgs();
                    args0.target = m_currentTracingObject.GetComponent<IAttackedObject>();
                    args0.attacker = this;
                    if (args0.target.HP>0 ) {
                        OnStartNormalAttacking(this, args0);
                    }
                }
                else
                {
                    MoveableObjectEventArgs args1 = new MoveableObjectEventArgs();
                    args1.navDestination = m_currentTracingObject.transform.position;
                    args1.TracingObject = m_currentTracingObject;
                    OnStartMoving(this, args1);
                }
            }
        }
        private IEnumerator UsingSkillCoroutine()
        {
            WaitForSeconds wait = new WaitForSeconds(0.05f);
            while (true)
            {
                UsingSkillObjectEventArgs args = new UsingSkillObjectEventArgs();
                OnUsingSkill(this, args);
                yield return wait;
            }
        }
        protected virtual void FaceToTargetGradually(GameObject target, float duration) {
            StartCoroutine(FaceToTargetCoroutine(target,duration));
        }
        IEnumerator FaceToTargetCoroutine(GameObject target,float duration) {
            Vector3 targetDirection = target.transform.position - transform.position;
            targetDirection = Vector3.ProjectOnPlane(targetDirection,Vector3.up);
            Vector3 originalForward = transform.forward;
            int times = 20;
            int i = 0;
            WaitForSeconds wait = new WaitForSeconds(duration/times);
            while (i<times) {
                var tempDir = Vector3.Lerp(originalForward,targetDirection,(float)i/times);
                transform.LookAt(transform.position+tempDir);
                i++;
                yield return wait;
            }
        }
        private float GetDistance(Vector3 from ,Vector3 to) {
            return (from - to).magnitude;
        }
    }

}
