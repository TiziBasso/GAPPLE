using GAPPLE.Client.Shared;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace GAPPLE.Client.Helpers
{
    public class RadzenCustom
    {
        [Inject] DialogService DialogService { get; set; }
        [Inject] TooltipService TooltipService { get; set; }

        public RadzenCustom(DialogService dialogService, TooltipService tooltipService)
        {
            DialogService = dialogService;
            TooltipService = tooltipService;
        }

        public void OpenBusyWithLoader(string text = "Guardando...")
        {
            DialogService.Open("",
               ds =>
               {
                   RenderFragment content = b =>
                   {
                       b.OpenComponent(0, typeof(Busy));
                       b.AddAttribute(1, "Text", text);
                       b.CloseComponent();
                   };
                   return content;
               },
               new DialogOptions()
               {
                   ShowTitle = false,
                   Style = "min-height: auto; min-width: auto; width: auto",
                   CloseDialogOnEsc = false
               });
        }

        public void CloseDialog()
        {
            DialogService.Close();
        }
    }
}
