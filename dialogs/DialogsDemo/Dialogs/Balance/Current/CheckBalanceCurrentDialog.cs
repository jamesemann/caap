using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;

namespace DialogsDemo.Dialogs.Balance.Current
{
    [Serializable]
    public class CheckBalanceCurrentDialog : IDialog<object>
    {
        // Entry point to the Dialog
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("[CheckBalanceCurrentDialog] ...");

            // State transition - complete this Dialog and remove it from the stack
            context.Done<object>(new object());
        }
    }
}