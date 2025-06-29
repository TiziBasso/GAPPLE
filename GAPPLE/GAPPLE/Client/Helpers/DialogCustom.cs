using GAPPLE.Client.Shared;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace GAPPLE.Client.Helpers
{
    public class DialogCustom
    {
        [Inject] DialogService DialogService { get; set; }
        [Inject] NotificationService NotificationService { get; set; }

        public DialogCustom(DialogService dialogService, NotificationService notificationService)
        {
            DialogService = dialogService;
            NotificationService = notificationService;
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

        public void OpenBusy(string message)
        {
            DialogService.Open("",
                ds =>
                {
                    RenderFragment content = b =>
                    {
                        b.OpenElement(0, "RadzenRow");
                        b.OpenElement(1, "RadzenColumn");
                        b.AddAttribute(2, "Size", "12");
                        b.AddContent(3, message);
                        b.CloseElement();
                        b.CloseElement();
                    };

                    return content;
                },
                new DialogOptions()
                {
                    ShowTitle = false,
                    Style = "min-height: auto; min-width: auto; width: auto;",
                    CloseDialogOnEsc = false
                });
        }

        public void ShowErrors(string error, string titulo = "Compruebe los datos ingresados")
        {
            DialogService.Open(titulo,
                ds =>
                {
                    RenderFragment content = b =>
                    {
                        b.OpenComponent(0, typeof(ShowErrors));
                        b.AddAttribute(1, "Errors", new List<string>() { error });
                        b.CloseComponent();
                    };
                    return content;
                },
                new DialogOptions()
                {
                    Width = "500px",
                    Height = "auto",
                    Style = "max-height: 600px;",
                    Resizable = false,
                    Draggable = false
                });
        }

        public void ShowErrors(List<string> errors, string titulo = "Compruebe los datos ingresados")
        {
            DialogService.Open(titulo,
                ds =>
                {
                    RenderFragment content = b =>
                    {
                        b.OpenComponent(0, typeof(ShowErrors));
                        b.AddAttribute(1, "Errors", errors);
                        b.CloseComponent();
                    };
                    return content;
                },
                new DialogOptions()
                {
                    Width = "500px",
                    Height = "auto",
                    Style = "max-height: 600px;",
                    Resizable = false,
                    Draggable = false
                });
        }

        public void ShowErrors(Dictionary<string, List<string>> lstErrors, string titulo = "Compruebe los datos ingresados")
        {
            DialogService.Open(titulo,
                ds =>
                {
                    RenderFragment content = b =>
                    {
                        b.OpenComponent(0, typeof(ShowErrors));
                        b.AddAttribute(1, "lstErrores", lstErrors);
                        b.CloseComponent();
                    };
                    return content;
                },
                new DialogOptions()
                {
                    Width = "500px",
                    Height = "auto",
                    Style = "max-height: 600px;",
                    Resizable = false,
                    Draggable = false
                });
        }

        public void CloseDialog()
        {
            DialogService.Close();
        }

        public void OpenProgressBar(string text = "Procesando...")
        {
            DialogService.Open<ProgressBar>("", new() { { "Text", text } }, new DialogOptions()
            {
                ShowTitle = false,
                Style = "min-height: auto; min-width: auto; width: auto",
                CloseDialogOnEsc = false
            });
        }

        public void Notify(NotificationSeverity severity = NotificationSeverity.Info, string summary = "", string detail = "", double duration = 3000,
           Action<NotificationMessage> click = null, bool closeOnClick = false, object payload = null, Action<NotificationMessage> close = null)
        {
            NotificationService.Notify(severity, summary, detail, duration, click, closeOnClick, payload, close);
        }

        public void Notify(NotificationSeverity severity, string summary, string detail, TimeSpan duration, Action<NotificationMessage> click = null)
        {
            Notify(severity, summary, detail, duration.TotalMilliseconds, click);
        }

        public void NotifySuccess()
        {
            Notify(NotificationSeverity.Success, MensajesHelper.SaveSuccess(), duration: 2000);
        }

        public void NotifySuccess(string value)
        {
            Notify(NotificationSeverity.Success, MensajesHelper.SaveSuccess(value), duration: 2000);
        }

        public void NotifySuccess(string value, string operacion)
        {
            Notify(NotificationSeverity.Success, MensajesHelper.SaveSuccess(value, operacion), duration: 2000);
        }

        public void NotifyErrorModel()
        {
            Notify(NotificationSeverity.Error, MensajesHelper.ErrorSummaryModel, MensajesHelper.ErrorDetailModel, 3000);
        }

        public void NotifyInternalServerError()
        {
            Notify(NotificationSeverity.Error, MensajesHelper.ErrorSummary500, MensajesHelper.ErrorDetail500, 3000);
        }
    }
}
