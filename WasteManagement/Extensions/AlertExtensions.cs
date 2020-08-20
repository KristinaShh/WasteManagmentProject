using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WasteManagement.Extensions
{
    /// <summary>
    /// Display a message to the user via Toastr.
    /// Command is the toastr action, success, info, etc. and 
    /// Message is the text to display in the alert.
    /// </summary>
    public class Alert
    {
        public string Command { get; set; }
        public string Message { get; set; }

        public Alert(string command, string message)
        {
            Command = command;
            Message = message;
        }
    }

    /// <summary>
    /// A strongly-type extension method for accessing TempData which
    /// is used to store our Alerts.
    /// </summary>
    public static class AlertExtensions
    {
        private const string Alerts = "_Alerts";

        public static List<Alert> GetAlerts(this ITempDataDictionary tempData)
        {
            object o;
            tempData.TryGetValue(Alerts, out o);

            try
            {
                return o == null ? null : JsonConvert.DeserializeObject<List<Alert>>((string)o);
            }
            catch
            {
                return null;
            }
        }

        public static void SetAlerts(this ITempDataDictionary tempData, List<Alert> alerts)
        {
            tempData[Alerts] = JsonConvert.SerializeObject(alerts);
        }

        public static ActionResult WithSuccess(this ActionResult result, string message)
        {
            return new AlertDecoratorResult(result, "success", message);
        }

        public static ActionResult WithInfo(this ActionResult result, string message)
        {
            return new AlertDecoratorResult(result, "info", message);
        }


        public static ActionResult WithWarning(this ActionResult result, string message)
        {
            return new AlertDecoratorResult(result, "warning", message);
        }

        public static ActionResult WithError(this ActionResult result, string message)
        {
            return new AlertDecoratorResult(result, "error", message);
        }
    }


    /// <summary>
    /// Adds the alerts to an existing ActionResult
    /// </summary>
    public class AlertDecoratorResult : ActionResult
    {
        public IActionResult InnerResult { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }


        public AlertDecoratorResult(ActionResult innerResult, string command, string message)
        {
            InnerResult = innerResult;
            Command = command;
            Message = message;
        }



        /// <summary>
        /// Uses the extension method to get the list of alerts from temp data
        /// add a new alert to this list and then hand the execution off to
        /// the innerResult
        /// </summary>
        /// <param name="context"></param>
        public override async Task ExecuteResultAsync(ActionContext context)
        {
            ITempDataDictionaryFactory factory = context.HttpContext.RequestServices.GetService(typeof(ITempDataDictionaryFactory)) as ITempDataDictionaryFactory;
            ITempDataDictionary tempData = factory.GetTempData(context.HttpContext);

            var alerts = tempData.GetAlerts() ?? new List<Alert>();

            alerts.Add(new Alert(Command, Message));
            tempData.SetAlerts(alerts);

            await InnerResult.ExecuteResultAsync(context);
        }

    }
}