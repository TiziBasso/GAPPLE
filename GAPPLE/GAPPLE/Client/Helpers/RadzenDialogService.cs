using GAPPLE.Client.ComponentsUI;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace GAPPLE.Client.Helpers
{
    public class RadzenDialogService(DialogService dialogService, NotificationService notificationService, ContextMenuService contextMenuService)
    {
        #region UI
        public void OpenBusyWithLoader(string text = "Guardando...")
        {
            dialogService.Open("",
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
                   Style = "min-height: auto; min-width: auto; width: auto; inset-block: unset !important;",
                   CloseDialogOnEsc = false
               });
        }

        public void OpenBusy(string message)
        {
            dialogService.Open("",
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

        public void ShowAlert(string message, string titulo = "Compruebe los datos ingresados")
        {
            dialogService.Open(titulo,
                ds =>
                {
                    RenderFragment content = b =>
                    {
                        b.OpenComponent(0, typeof(ShowAlert));
                        b.AddAttribute(1, "Messages", new List<string>() { message });
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

        public async Task ShowAlertAsync(string message, string titulo = "Compruebe los datos ingresados")
        {
            await dialogService.OpenAsync(titulo,
                ds =>
                {
                    RenderFragment content = b =>
                    {
                        b.OpenComponent(0, typeof(ShowAlert));
                        b.AddAttribute(1, "Messages", new List<string>() { message });
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

        public void ShowAlert(List<string> messages, string titulo = "Compruebe los datos ingresados", string width = "500px")
        {
            dialogService.Open(titulo,
                ds =>
                {
                    RenderFragment content = b =>
                    {
                        b.OpenComponent(0, typeof(ShowAlert));
                        b.AddAttribute(1, "Messages", messages);
                        b.CloseComponent();
                    };
                    return content;
                },
                new DialogOptions()
                {
                    Width = width,
                    Height = "auto",
                    Style = "max-height: 600px;",
                    Resizable = false,
                    Draggable = false
                });
        }

        public async Task ShowAlertAsync(List<string> messages, string titulo = "Compruebe los datos ingresados", string width = "500px")
        {
            await dialogService.OpenAsync(titulo,
                ds =>
                {
                    RenderFragment content = b =>
                    {
                        b.OpenComponent(0, typeof(ShowAlert));
                        b.AddAttribute(1, "Messages", messages);
                        b.CloseComponent();
                    };
                    return content;
                },
                new DialogOptions()
                {
                    Width = width,
                    Height = "auto",
                    Style = "max-height: 600px;",
                    Resizable = false,
                    Draggable = false
                });
        }

        public void ShowAlert(Dictionary<string, List<string>> dictionary, string titulo = "Compruebe los datos ingresados")
        {
            dialogService.Open(titulo,
                ds =>
                {
                    RenderFragment content = b =>
                    {
                        b.OpenComponent(0, typeof(ShowAlert));
                        b.AddAttribute(1, "Dictionary", dictionary);
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

        public async Task ShowAlertAsync(Dictionary<string, List<string>> dictionary, string titulo = "Compruebe los datos ingresados")
        {
            await dialogService.OpenAsync(titulo,
                ds =>
                {
                    RenderFragment content = b =>
                    {
                        b.OpenComponent(0, typeof(ShowAlert));
                        b.AddAttribute(1, "Dictionary", dictionary);
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

        public void ShowAlert(string message, AlertStyle alertStyle, Variant variant, Shade shade, string titulo)
        {
            dialogService.Open(titulo,
                ds =>
                {
                    RenderFragment content = b =>
                    {
                        b.OpenComponent(0, typeof(ShowAlert));
                        b.AddAttribute(1, "Messages", new List<string>() { message });
                        b.AddAttribute(2, "AlertStyle", alertStyle);
                        b.AddAttribute(3, "Variant", variant);
                        b.AddAttribute(4, "Shade", shade);
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

        public async Task ShowAlertAsync(string message, AlertStyle alertStyle, Variant variant, Shade shade, string titulo)
        {
            await dialogService.OpenAsync(titulo,
                ds =>
                {
                    RenderFragment content = b =>
                    {
                        b.OpenComponent(0, typeof(ShowAlert));
                        b.AddAttribute(1, "Messages", new List<string>() { message });
                        b.AddAttribute(2, "AlertStyle", alertStyle);
                        b.AddAttribute(3, "Variant", variant);
                        b.AddAttribute(4, "Shade", shade);
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

        public void OpenProgressBar(string text = "Procesando...")
        {
            dialogService.Open<ProgressBar>("", new() { { "Text", text } }, new()
            {
                ShowTitle = false,
                Style = "min-height: auto; min-width: auto; width: auto",
                CloseDialogOnEsc = false
            });
        }

        public async Task<bool> ShowConfirmCustomAsync(Dictionary<string, List<string>> dictionary, string detail, string titulo = "Compruebe los datos ingresados", AlertStyle? alertStyle = null, Variant? variant = null, Shade? shade = null)
        {
            return await dialogService.OpenAsync(titulo,
                ds =>
                {
                    RenderFragment content = b =>
                    {
                        b.OpenComponent(0, typeof(ConfirmCustom));
                        b.AddAttribute(1, "Dictionary", dictionary);
                        b.AddAttribute(2, "AlertStyle", alertStyle);
                        b.AddAttribute(3, "Variant", variant);
                        b.AddAttribute(4, "Shade", shade);
                        b.AddAttribute(4, "Detail", detail);
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
                    Draggable = false,
                    ShowClose = false,
                    CloseDialogOnEsc = false,
                    CloseDialogOnOverlayClick = false
                });
        }

        public async Task<bool> ShowConfirmCustomAsync(List<string> errors, string detail, string titulo = null, AlertStyle? alertStyle = null, Variant? variant = null, Shade? shade = null) =>
            await ShowConfirmCustomAsync(new Dictionary<string, List<string>>() { { "", errors } }, detail, titulo, alertStyle, variant, shade);

        public async Task<bool> ShowContirmCustomAsync(string error, string detail, string titulo = null, AlertStyle? alertStyle = null, Variant? variant = null, Shade? shade = null) =>
            await ShowConfirmCustomAsync([error], detail, titulo, alertStyle, variant, shade);
        #endregion

        #region Dialog
        public async Task<dynamic> OpenAsync<T>(string title, Dictionary<string, object> parameters = null, DialogOptions options = null) where T : ComponentBase
        {
            return await dialogService.OpenAsync<T>(title, parameters, options);
        }

        public async Task<dynamic> OpenAsync(string title, RenderFragment<DialogService> childContent, DialogOptions options = null, CancellationToken? cancellationToken = null)
        {
            return await dialogService.OpenAsync(title, childContent, options, cancellationToken);
        }

        public void CloseDialog(dynamic result = null)
        {
            dialogService.Close(result);
        }

        public void OpenFileInputDialog(string title, EventCallback<string> onFileSelected, string accept = ".json", string chooseText = "Seleccionar archivo", string width = "200px")
        {
            dialogService.Open(title,
                ds =>
                {
                    RenderFragment content = b =>
                    {
                        b.OpenComponent(0, typeof(RadzenFileInput<string>));
                        b.AddAttribute(2, "Change", onFileSelected);
                        b.AddAttribute(3, "Accept", accept);
                        b.AddAttribute(4, "ChooseText", chooseText);
                        b.CloseComponent();
                    };
                    return content;
                },
                new DialogOptions()
                {
                    Width = width
                });
        }

        public async ValueTask<string> OpenDialogInput(string label)
        {
            return await dialogService.OpenAsync<DialogInput>(MensajesHelper.Title, new() { { "Label", label } });
        }
        #endregion

        #region Confirm
        public async Task<bool?> Confirm(string message, string title = null, ConfirmOptions options = null)
        {
            title ??= MensajesHelper.Title;
            options ??= new ConfirmOptions() { OkButtonText = "Sí", CancelButtonText = "No", ShowClose = false };
            return await dialogService.Confirm(message, title, options);
        }

        public static ConfirmOptions ConfirmOptions()
        {
            return new()
            {
                CancelButtonText = "Sí",
                OkButtonText = "No",
                AutoFocusFirstElement = false,
                ShowClose = false,
                Width = "700px"
            };
        }

        public async Task<bool?> Confirm(RenderFragment message, string title, ConfirmOptions options = null)
        {
            options ??= new ConfirmOptions() { OkButtonText = "Sí", CancelButtonText = "No", ShowClose = false };
            return await dialogService.Confirm(message, title, options);
        }

        public static ConfirmOptions ConfirmOptions(string okButtonText, string cancelButtonText)
        {
            return new()
            {
                CancelButtonText = cancelButtonText,
                OkButtonText = okButtonText,
                AutoFocusFirstElement = false,
                ShowClose = false,
                Width = "700px"
            };
        }

        public async Task<bool> CombinarComprobantes()
        {
            return (bool)await Confirm(MensajesHelper.Confirm("combinar", "los comprobantes"), MensajesHelper.Title, new ConfirmOptions() { OkButtonText = "Sí", CancelButtonText = "No", ShowClose = false, CloseDialogOnEsc = false });
        }

        /// <summary>
        /// "¿Desea confirmar la operación?"
        /// </summary>
        public async Task<bool> ConfirmOperation()
        {
            return (bool)await Confirm(MensajesHelper.Confirm(), MensajesHelper.Title, new ConfirmOptions() { OkButtonText = "Sí", CancelButtonText = "No", ShowClose = false, CloseDialogOnEsc = false });
        }

        /// <summary>
        /// "¿Desea {operacion} la operación?"
        /// </summary>
        /// <param name="operacion"></param>
        public async Task<bool> ConfirmOperation(string operation)
        {
            return (bool)await Confirm(MensajesHelper.Confirm(operation), MensajesHelper.Title, new ConfirmOptions() { OkButtonText = "Sí", CancelButtonText = "No", ShowClose = false, CloseDialogOnEsc = false });
        }

        /// <summary>
        /// "¿Desea {operacion} {value}?"
        /// </summary>
        /// <param name="operacion"></param>
        /// <param name="value"></param>
        public async Task<bool> ConfirmOperation(string operation, string value)
        {
            return (bool)await Confirm(MensajesHelper.Confirm(operation, value), MensajesHelper.Title, new ConfirmOptions() { OkButtonText = "Sí", CancelButtonText = "No", ShowClose = false, CloseDialogOnEsc = false });
        }

        public async Task<bool> CancelOperation()
        {
            return (bool)await Confirm(MensajesHelper.Cancel(), MensajesHelper.Title, new ConfirmOptions() { OkButtonText = "Sí", CancelButtonText = "No", ShowClose = false, CloseDialogOnEsc = false });
        }

        public async ValueTask<bool> ConfirmBackup()
        {
            return (bool)await Confirm("Hay una operación en proceso ¿Desea recuperarla?", MensajesHelper.Title, new ConfirmOptions { CancelButtonText = "NO", OkButtonText = "Sí", CloseDialogOnEsc = false, ShowClose = false });
        }
        #endregion

        #region Notify
        public void Notify(NotificationSeverity severity = NotificationSeverity.Info, string summary = "", string detail = "", double duration = 3000,
           Action<NotificationMessage> click = null, bool closeOnClick = false, object payload = null, Action<NotificationMessage> close = null)
        {
            notificationService.Notify(severity, summary, detail, duration, click, closeOnClick, payload, close);
        }

        public void Notify(NotificationMessage notificationMessage)
        {
            notificationService.Notify(notificationMessage);
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

        public void NotifyDelete(string value)
        {
            Notify(NotificationSeverity.Success, MensajesHelper.Delete(value), duration: 2000);
        }

        /// <summary>
        /// "No se puede completar la operación" \n "Compruebe todos los datos ingresados!"
        /// </summary>
        public void NotifyErrorModel()
        {
            Notify(NotificationSeverity.Error, MensajesHelper.ErrorSummaryModel, MensajesHelper.ErrorDetailModel, 3000);
        }

        public void NotifyInternalServerError()
        {
            Notify(NotificationSeverity.Error, MensajesHelper.ErrorSummary500, MensajesHelper.ErrorDetail500, 3000);
        }

        public void NotifyTokenCanceled()
        {
            Notify(NotificationSeverity.Warning, MensajesHelper.TokenCanceled, duration: 3000);
        }

        public void NotifyEmptyResponse()
        {
            Notify(NotificationSeverity.Info, MensajesHelper.EmptyResponse, duration: 2000);
        }
        #endregion

        #region Alert
        public async Task<bool?> Alert(string message, string title = null, AlertOptions options = null)
        {
            title ??= MensajesHelper.Title;
            options ??= new AlertOptions() { CloseDialogOnEsc = false, OkButtonText = "Aceptar" };

            return await dialogService.Alert(message, title, options);
        }

        public async Task<bool?> Alert(RenderFragment message, string title = "", AlertOptions options = null)
        {
            return await dialogService.Alert(message, title, options);
        }

        public async Task AlertDeleteWMS(string value) =>
            await Alert(RadzenHelper.GetRenderFragment($"<strong>Recuerde eliminar {value} desde el sistema del WMS</strong>"), MensajesHelper.Title);

        #endregion

        #region ContextMenu
        public void OpenContextMenu(MouseEventArgs args, IEnumerable<ContextMenuItem> items, Action<MenuItemEventArgs> click = null)
        {
            contextMenuService.Open(args, items, click);
        }

        public void CloseContextMenu()
        {
            contextMenuService.Close();
        }
        #endregion
    }
}
