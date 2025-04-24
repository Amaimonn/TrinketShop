using UnityEngine;

namespace TrinketShop.Game.World
{
    public interface IPointerControllable
    {
        public void OnPointerDown(int pointerId, Vector2 position);
        public void OnPointerUp(int pointerId, Vector2 position);
        public void OnPointerEnter();
        public void OnPointerExit();
        public void OnPointerMove(Vector2 position);
        public void BeginDrag();
        public void EndDrag();
    }
}