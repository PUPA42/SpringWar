using KaiTool.Utilites;
using UnityEngine;

namespace SpringWar
{
    public sealed class SuperUIManager : Singleton<SuperUIManager>
    {
        [Header("Cursors")]
        public Texture2D normalCursorTexture;
        [Header("RouteSign")]
        public GameObject RouteSign;
        private void Update()
        {
            Cursor.SetCursor(normalCursorTexture,Vector2.zero,CursorMode.Auto);
        }


    }
}