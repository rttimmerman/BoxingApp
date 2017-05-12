using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BoxingApp.Helpers
{
    public static class DialogHelper
    {
        public static void DisplayMessage(Activity activity, string message)
        {
            new AlertDialog.Builder(activity).SetMessage(message).Show();
        }

        /// <summary>
        /// Returns true if user clicks yes.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="onYes">Action to perform on yes clicked</param>
        /// <param name="onNo">Action to perform on no clicked</param>
        /// <returns></returns>
        public static void ShowConfirmationDialog(Activity context, string title, string message, Action onYes)
        {
            //set alert for executing the task
            var alert = new AlertDialog.Builder(context);
            alert.SetCancelable(true);

            alert.SetTitle(title);

            alert.SetPositiveButton("Yes", (senderAlert, args) => onYes.Invoke());

            context.RunOnUiThread(() =>
            {
                alert.Show();
            });
        }

        
    }
}