using DialogsDemo.Dialogs.Balance;
using DialogsDemo.Dialogs.Payment;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace DialogsDemo.Dialogs
{
    [Serializable]
    public class MainDialog : IDialog<object>
    {
        // Entry point to the Dialog
        public async Task StartAsync(IDialogContext context)
        {
            // State transition - wait for 'start' message from user
            context.Wait(MessageReceivedStart);
        }

        public async Task MessageReceivedStart(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            await context.PostAsync($"[MainDialog] I'm banking 🤖{Environment.NewLine}Would you like to:{Environment.NewLine}- Check balance 💰{Environment.NewLine}- Make payment 💸");

            // State transition - wait for 'operation choice' message from user
            context.Wait(MessageReceivedOperationChoice);
        }

        public async Task MessageReceivedOperationChoice(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            if (message.Text.ToLower().Equals("check balance", StringComparison.InvariantCultureIgnoreCase))
            {
                // State transition - add 'check balance' Dialog to the stack, when done call AfterChildDialogIsDone callback
                context.Call<object>(new CheckBalanceDialog(), AfterChildDialogIsDone);
            }
            else if (message.Text.ToLower().Equals("make payment", StringComparison.InvariantCultureIgnoreCase))
            {
                // State transition - add 'make payment' Dialog to the stack, when done call AfterChildDialogIsDone callback
                context.Call<object>(new MakePaymentDialog(), AfterChildDialogIsDone);
            }
            else
            {
                // State transition - wait for 'start' message from user (loop back)
                context.Wait(MessageReceivedStart);
            }
        }

        private async Task AfterChildDialogIsDone(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync($"[MainDialog] Anything else I can help with?:{Environment.NewLine}- Check balance 💰{Environment.NewLine}- Make payment 💸");

            // State transition - wait for 'operation choice' message from user (loop back)
            context.Wait(MessageReceivedOperationChoice);
        }
    }
}