using UnityEngine;

namespace TrinketShop.Game.World.Trinkets
{
    [CreateAssetMenu(fileName = "TrinketTweensConfigSO", menuName = "Scriptable Objects/Entities/Trinkets/TrinketTweensConfigSO")]
    public class TrinketTweensConfigSO : ScriptableObject
    {
        [field: SerializeField] public float HoverScale { get; private set; } = 1.1f;
        [field: SerializeField] public float HoverShakeStrength { get; private set; } = 10f;
        [field: SerializeField] public int HoverShakeVibrato { get; private set; } = 15;
        [field: SerializeField] public float HoverAnimDuration { get; private set; } = 0.2f;
        [field: SerializeField] public float HoverMaxTilt { get; private set; } = 20f;
        [field: SerializeField] public float HoverTiltSpeed { get; private set; } = 5f;

        [field: SerializeField] public float DragScale { get; private set; } = 1.15f;
        [field: SerializeField] public float DragShakeStrength { get; private set; } = 10f;
        [field: SerializeField] public float DragLerpSpeed { get; private set; } = 25f;

        [field: SerializeField] public float ClickPunchStrength { get; private set; } = 7f;
        [field: SerializeField] public float ClickPunchDuration { get; private set; } = 0.2f;
        [field: SerializeField] public int ClickPunchVibrato { get; private set; } = 20;
    }
}