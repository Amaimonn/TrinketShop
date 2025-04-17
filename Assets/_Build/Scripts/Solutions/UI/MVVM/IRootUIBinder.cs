using System.Collections.Generic;

namespace TrinketShop.Solutions.UI.MVVM
{
    public interface IRootUIBinder
    {
        public void SetView(View view);

        public void SetViews(IEnumerable<View> views);

        public void SetViews(params View[] views);

        public void AddView(View view);

        public void AddViews(params View[] views);

        public void AddViews(IEnumerable<View> views);
        
        public void ClearView(View view);

        public void ClearViews();
    }
}