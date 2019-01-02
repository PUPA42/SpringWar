using UnityEngine;
using KaiTool.Utilites;
using System;
using SpringWar;

namespace KaiTool.MouseOperations { 
public class InputManager : Singleton<InputManager>{
        [Header("PlayerOperationManager")]
        [SerializeField]
        private ISelectedObject _selectedObject;
        private LayerMask OperatedLayer;

        public ISelectedObject SelectedObject
        {
            get
            {
                return _selectedObject;
            }

            set
            {
                _selectedObject = value;
            }
        }

        public override void Init()
        {
            base.Init();
            InitVar();
            InitComponent();
            InitEvent();
        }

        private void InitComponent()
        {
        }
        private void InitVar() { }
        private void InitEvent() { }
        private void Update()
    {
            GetMouseInput();
    }
    public void GetMouseInput() {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray ,out hit)) {
                    ISelectedObject selectableObject = hit.collider.GetComponent<ISelectedObject>();
                    if (selectableObject!=null) {
                        selectableObject.OnSelected(this,new SelectedObjectEventArgs());
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray,out hit)) {
                    IClickedObject clickableObject = hit.collider.GetComponent<IClickedObject>();
                    if (clickableObject!=null) {
                        clickableObject.OnClicked(this,new ClickedObjectEventArgs(hit.point));
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Space)) {
                CameraManager.Instance.FoucuseOnPlayer();
            }
    }

}
    }