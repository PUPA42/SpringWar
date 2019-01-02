using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KaiTool.BatchInitialization;
using KaiTool.Utilites;

namespace SpringWar
{
    public sealed class CameraManager : Singleton<CameraManager>
    {
        [Tooltip("The direction which the MainCameraIsLookingAt")]
        public Vector3 cameraDirection;
        public float cameraTranslateSpeed = 1f;
        public float distanceToParent = 4f;
        private Coroutine currentCameraMotionCoroutine;
        private Vector3 topDirection;
        private Vector3 rightDirection;
        int topTag = 8;
        int leftTag = 4;
        int botTag = 2;
        int rightTag = 1;

        public override void Init()
        {
            base.Init();
            InitVar();
            InitComponent();
        }
        private void InitVar()
        {
            topDirection = Camera.main.transform.parent.forward;
            rightDirection = Camera.main.transform.parent.right;
            Camera.main.transform.localPosition = Camera.main.transform.localPosition.normalized * distanceToParent;
        }
        private void InitComponent()
        {
            // Camera.main.transform.LookAt(transform.parent.position);
        }

        private void Start()
        {
            FoucuseOnPlayer();
            currentCameraMotionCoroutine = StartCoroutine(CameraTranslateCoroutine());
        }
        IEnumerator CameraTranslateCoroutine()
        {
            var intervalTime = 0.01f;
            WaitForSeconds wait = new WaitForSeconds(intervalTime);
            while (true)
            {
                var msPos = Input.mousePosition;
                var widthBorder = Screen.width / 20;
                var heightBorder = Screen.height / 20;
                Vector3 deltaVec = Vector3.zero;

                if (msPos.y > Screen.height - heightBorder)
                {
                    deltaVec += (msPos.y - (Screen.height - heightBorder)) * topDirection;
                }
                if (msPos.y < heightBorder)
                {
                    deltaVec += (msPos.y - heightBorder) * topDirection;
                }
                if (msPos.x > Screen.width - widthBorder)
                {
                    deltaVec += (msPos.x - (Screen.width - widthBorder)) * rightDirection;
                }
                if (msPos.x < widthBorder)
                {
                    deltaVec += (msPos.x - widthBorder) * rightDirection;
                }

                deltaVec *= cameraTranslateSpeed * intervalTime;
                //   Camera.main.transform.position += deltaVec;
                Camera.main.transform.parent.position = Vector3.Lerp(Camera.main.transform.parent.position, Camera.main.transform.parent.position + deltaVec, 0.8f);

                yield return wait;
            }
        }
        public void StartCameraHeightControll()
        {
            StartCoroutine(CameraHeightControllCoroutine());
        }
        IEnumerator CameraHeightControllCoroutine()
        {
            WaitForSeconds wait = new WaitForSeconds(0.02f);
            while (true)
            {
                var mainCameraParentTrans = Camera.main.transform.parent;
                mainCameraParentTrans.position = new Vector3(mainCameraParentTrans.position.x, GameManager.Instance.player.transform.position.y,
                    mainCameraParentTrans.position.z);
                LayerMask layer = LayerMask.NameToLayer("Floor");
                var searchingRadius = 2f;
                if (Physics.OverlapSphere(GameManager.Instance.player.transform.position, searchingRadius, 1 << layer).Length == 0)
                {
                    break;
                }
                yield return wait;
            }

        }
        public void FoucuseOnPlayer()
        {
            Camera.main.transform.parent.position = GameManager.Instance.player.transform.position;
        }
    }
}