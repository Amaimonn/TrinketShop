using UnityEngine;
using R3;

using TrinketShop.Infrastructure.Bootstrap;

namespace TrinketShop.Infrastructure.Entry
{
    public class EntryPoint
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Run()
        {
            Observable.NextFrame().Subscribe(_ => Object.FindAnyObjectByType<GameplayBootstrap>().Boot());
        }
    }
}