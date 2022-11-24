using Microsoft.AspNetCore.Components;
using static MudBlazor.CategoryTypes;

namespace IF.Manager.Blazor.Client.Components
{
    public partial class IFExpansionList<T>
    {

        [Parameter]
        public RenderFragment<T> Title { get; set; }

        [Parameter]
        public RenderFragment<T> Data { get; set; }

        [Parameter]
        public IEnumerable<T> Items { get; set; }
        
            
        
    }
}
