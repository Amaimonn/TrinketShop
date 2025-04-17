using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TrinketShop.Solutions.UI.MVVM
{
    public class RootUIBinder : MonoBehaviour, IRootUIBinder, IRootUI
    {
        [SerializeField] private RectTransform _canvasFirstUiRoot;
        [SerializeField] private RectTransform _canvasLastUiRoot;
        private readonly HashSet<View> _bindedViews = new();

#region IRootUIBinder
        public void SetView(View view)
        {
            ClearViews();
            AddView(view);
        }

        public void SetViews(IEnumerable<View> views)
        {
            ClearViews();
            AddViews(views);
        }

        public void SetViews(params View[] views)
        {
            ClearViews();
            AddViews(views);
        }

        public void AddView(View view)
        {
            view.Attach(this);
            _bindedViews.Add(view);
        }

        public void AddViews(params View[] views)
        {
            foreach (var view in views)
            {
                AddView(view);
            }
        }

        public void AddViews(IEnumerable<View> views)
        {
            foreach (var view in views)
            {
                AddView(view);
            }
        }

        public void ClearView(View view)
        {
            if (view != null)
            {
                view.Detach(this);
            }
            _bindedViews.Remove(view);
        }

        public void ClearViews()
        {
            var viewsToClear = _bindedViews.ToArray();
            foreach (var view in viewsToClear)
            {
                ClearView(view);
            }
        }
#endregion

#region IRootUI
/// <summary>
/// This is used by Canvas Views in terms of implementation Visitor pattern. 
/// Can also be used with UI Toolkit Views gameobjects just to hold them.
/// For the scene UI binding use SetViews or AddViews method instead.
/// </summary>
        public void Attach(GameObject gameObjectUI, CanvasOrder order = CanvasOrder.First)
        {
            switch (order)
            {
                case CanvasOrder.First: 
                    gameObjectUI.transform.SetParent(_canvasFirstUiRoot, false); break;
                case CanvasOrder.Last: 
                    gameObjectUI.transform.SetParent(_canvasLastUiRoot, false); break;
            }
        }

        public void Detach(GameObject gameObjectUI)
        {
            Destroy(gameObjectUI);
        }
#endregion
    }
}
