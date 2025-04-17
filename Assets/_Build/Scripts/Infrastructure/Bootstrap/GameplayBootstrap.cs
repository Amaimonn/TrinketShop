using UnityEngine;

using TrinketShop.Solutions.UI.MVVM;

namespace TrinketShop.Infrastructure.Bootstrap
{
    public class GameplayBootstrap : MonoBehaviour
    {
        [SerializeField] private RootUIBinder _rootUIBinder;

        public void Boot ()
        {
            Debug.Log("Gameplay Boot");
        }
    }
}