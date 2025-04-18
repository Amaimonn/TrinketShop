using UnityEngine;

namespace TrinketShop.Game.World.GameField
{
    public class GameFieldBounds : MonoBehaviour, IGameField
    {
        [SerializeField] private Vector3 _size;

        private Vector2 _minBounds;
        private Vector2 _maxBounds;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            _minBounds = transform.position - _size / 2;
            _maxBounds = transform.position + _size / 2;
        }

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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(transform.position, _size);
        }
    }
}