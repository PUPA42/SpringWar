using UnityEngine;
using KaiTool.BatchInitialization;
using KaiTool.MouseOperations;
using System;
using SpringWar.player;
using System.Collections;

namespace SpringWar {
    public class Floor : KaiTool_BatchedInitializedBehaviour, IClickableSurface {
        private const float durationBeforeDestroyRoutineSign=0.6f;

        public Action<UnityEngine.Object, ClickedObjectEventArgs> Clicked;
        private Vector3 _clickedPoint;
        private GameObject _currentRouteSign=null;
        private Coroutine _showRoutineSignCoroutine;
        public override void Init()
        {
            base.Init();
            InitVar();
            InitEvent();
        }
        private void InitVar() {
        }
        private void InitEvent() {
            Clicked += (sender, e) => {
                this._clickedPoint = ((ClickedObjectEventArgs)e).clickedPoint;
                ShowRouteSign(_clickedPoint,durationBeforeDestroyRoutineSign);
                MoveableObjectEventArgs args = new MoveableObjectEventArgs();
                args.navDestination = _clickedPoint;
                args.TracingObject = null;
                GameManager.Instance.player.OnStartMoving(this,args);
                //print("Floor is Clicked!! " + " ClickedPoint is " + GetClickPoint());
            };
        }

        public Vector3 GetClickPoint()
        {
            return _clickedPoint;
        }

        public void OnClicked(UnityEngine.Object sender, ClickedObjectEventArgs e)
        {
            if (Clicked != null) {
                Clicked(sender, e);
            }
        }
        private void ShowRouteSign(Vector3 pos, float duration) {
            if (_showRoutineSignCoroutine!=null) {
                StopCoroutine(_showRoutineSignCoroutine);
            }
            _showRoutineSignCoroutine=StartCoroutine(ShowRouteSignCoroutine(pos,duration));
        }
        IEnumerator ShowRouteSignCoroutine(Vector3 pos, float duration) {
            if (_currentRouteSign == null)
            {
                _currentRouteSign = GameObject.Instantiate(SuperUIManager.Instance.RouteSign, pos, Quaternion.identity);
            }
            else {
                _currentRouteSign.SetActive(false);
                _currentRouteSign.transform.position = pos;
                _currentRouteSign.SetActive(true);
            }
            yield return new WaitForSeconds(durationBeforeDestroyRoutineSign);
            _currentRouteSign.SetActive(false);
        }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }
    }
}
