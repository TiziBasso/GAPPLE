using GAPPLE.Client.Tools;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections;
using System.Reflection;

namespace GAPPLE.Client.Helpers
{
    public class ToolsHelpers
    {
        [Inject] NavigationManager NavigationManager { get; set; }

        public ToolsHelpers(NavigationManager navigationManager)
        {
            NavigationManager = navigationManager;
            operaciones = Enum.GetValues(typeof(Operaciones)).Cast<Operaciones>().ToList();
        }

        private readonly List<Operaciones> operaciones;

        public bool Validate(string operation, int? value)
        {
            operation = operation.ToLower();

            if (operation == "ver") operation = "lectura";

            if (operaciones.Exists(x => x.ToString().ToLower() == operation))
            {
                if (((operation == "lectura" || operation == "edicion" || operation == "clonar" || operation == "importar") && value == null) || (operation == "alta" && value != null))
                {
                    NavigationManager.NavigateTo(Variables.ErrorPages.Invalido);
                    return true;
                }
            }
            else
            {
                NavigationManager.NavigateTo(Variables.ErrorPages.Invalido);
                return true;
            }
            return false;
        }

        public Operaciones ObtenerOperacion(string operation)
        {
            operation = operation.ToLower();
            if (operation == "ver") operation = "lectura";
            return operaciones.Find(x => x.ToString().Equals(operation, StringComparison.CurrentCultureIgnoreCase));
        }

        public void ClearValidationMessages(EditContext editContext, bool revalidate = false, bool markAsUnmodified = false)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            object GetInstanceField(Type type, object instance, string fieldName)
            {
                var fieldInfo = type.GetField(fieldName, bindingFlags);
                return fieldInfo.GetValue(instance);
            }

            var fieldStates = GetInstanceField(typeof(EditContext), editContext, "_fieldStates");
            var clearMethodInfo = typeof(HashSet<ValidationMessageStore>).GetMethod("Clear", bindingFlags);

            foreach (DictionaryEntry kv in (IDictionary)fieldStates)
            {
                var messageStores = GetInstanceField(kv.Value.GetType(), kv.Value, "_validationMessageStores");
                if (messageStores is HashSet<ValidationMessageStore> stores)
                {
                    clearMethodInfo.Invoke(stores, null);
                }
                //clearMethodInfo.Invoke(messageStores, null);
            }

            if (markAsUnmodified)
                editContext.MarkAsUnmodified();

            if (revalidate)
                editContext.Validate();
        }
    }

}
