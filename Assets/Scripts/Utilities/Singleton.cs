using System;
using UnityEngine;
using KaiTool.BatchInitialization;
namespace KaiTool.Utilites
{
    public class Singleton<T> : KaiTool_BatchedInitializedBehaviour where T : Singleton<T>
    {
        protected static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] instanceArray = FindObjectsOfType<T>();
                    if (instanceArray.Length > 1)
                    {
                        throw new MoreThanOneSingletonException();
                    }
                    else {
                        try
                        {
                            _instance = instanceArray[0];
                        }
                        catch (IndexOutOfRangeException e) {
                            print(e.Message);
                        }

                    }
                }
                return _instance;
            }
            set {
                _instance = value;
            }
        }
        public void OnDestroy()
        {
            //print("Singleton of "+this.GetType()+" has been destroyed.");
        }
    }
    public class MoreThanOneSingletonException : Exception {
        public MoreThanOneSingletonException() : base("There is more than one singleton in the scene.")
        {
        }
        public MoreThanOneSingletonException(string Message) : base(Message) {

        }
    }
}
