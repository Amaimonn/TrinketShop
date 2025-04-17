using UnityEngine;

namespace TrinketShop.Game.World.GameField
{
    public class GameFieldBounds : MonoBehaviour
    {
        [SerializeField] private Vector2 _minBounds;
        [SerializeField] private Vector2 _maxBounds;

        public bool IsInsideBounds(Vector2 position)
        {
            return position.x >= _minBounds.x && position.x <= _maxBounds.x &&
                   position.y >= _minBounds.y && position.y <= _maxBounds.y;
        }

        public Vector2 GetRandomPosition()
        {
            return new Vector2(
                Random.Range(_minBounds.x, _maxBounds.x),
                Random.Range(_minBounds.y, _maxBounds.y)
            );
        }
    }
}