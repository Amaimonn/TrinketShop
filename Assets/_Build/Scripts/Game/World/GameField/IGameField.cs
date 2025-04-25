using UnityEngine;

namespace TrinketShop.Game.World.GameField
{
    public interface IGameField
    {
        public bool IsInsideBounds(Vector2 position);
        public Vector3 GetClosestInsideBounds(Vector3 position);
        public Vector2 GetRandomPosition();
    }
}