//#define Debug
using KaiTool.Utilites;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KaiTool.BatchInitialization {
    public class KaiTool_BatchedInitializationManager : Singleton<KaiTool_BatchedInitializationManager> {
        [Header("BatchedInitializationManager")]
        public int numEachIntervalTime=20;
        private bool _isStimulated = false;
        private WaitForSeconds wait = new WaitForSeconds(0.1f);
        public List<KaiTool_BatchedInitializedBehaviour> batchedBehavioutList = new List<KaiTool_BatchedInitializedBehaviour>();
        /*
        [RuntimeInitializeOnLoadMethod]
        static void InvokdeOnLoad() {
            GameObject.DontDestroyOnLoad(new GameObject("BatchedInitializationManager",typeof(KaiTool_BatchedInitializationManager)));
            print("RumtimeInitializeOnLoadMethod");
        }
        */
        public bool IsStimulated
        {
            get
            {
                return _isStimulated;
            }

            set
            {
                _isStimulated = value;
            }
        }
        public void TurnOn() {
#if Debug
            print("Turn On");
#endif
            IsStimulated = true;
            StartToWork();
        }
        public void TurnOff() {
#if Debug
            print("Turn Off");
#endif
            IsStimulated = false;
            StopAllCoroutines();
        }
        private void StartToWork() {
            StartCoroutine(WorkCoroutine());
        }
        IEnumerator WorkCoroutine() {
            while (batchedBehavioutList.Count>0) {
                var min = Mathf.Min(numEachIntervalTime,batchedBehavioutList.Count);
                for (int i=0;i<min;i++)
                {
                    var count = batchedBehavioutList.Count;
                    batchedBehavioutList[count-1].Init();
                    batchedBehavioutList.Remove(batchedBehavioutList[count-1]);//
                }
                yield return wait;
            }
            TurnOff();
        }

    }
}