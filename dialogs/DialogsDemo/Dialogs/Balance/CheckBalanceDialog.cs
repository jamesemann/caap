using DialogsDemo.Dialogs.Balance.Current;
using DialogsDemo.Dialogs.Balance.Savings;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace DialogsDemo.Dialogs.Balance
{
    [Serializable]
    public class CheckBalanceDialog : IDialog<object>
    {
        // Entry point to the Dialog
        public async Task StartAsync(IDialogContext context)
        {
            var message = Cards.CreateImBackHeroCard(context.MakeMessage(), "[CheckBalanceDialog] Which account?", new string[] { "Current", "Savings" });

            await context.PostAsync(message);
            
            context.Wait(MessageReceivedOperationChoice);
        }

        public async Task MessageReceivedOperationChoice(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            if (message.Text.Equals("current", StringComparison.InvariantCultureIgnoreCase))
            {
                // State transition - add 'current account' Dialog to the stack, when done call AfterChildDialogIsDone callback
                context.Call<object>(new CheckBalanceCurrentDialog(), AfterChildDialogIsDone);
            }
            else if (message.Text.Equals("savings", StringComparison.InvariantCultureIgnoreCase))
            {
                // State transition - add 'savings account' Dialog to the stack, when done call AfterChildDialogIsDone callback
                context.Call<object>(new CheckBalanceSavingsDialog(), AfterChildDialogIsDone);
            }
            else
            {
                var reply = Cards.CreateImBackHeroCard(context.MakeMessage(), "[CheckBalanceDialog] Sorry, I don't understand! Which account?", new string[] { "Current", "Savings" });

                await context.PostAsync(reply);

                // State transition - wait for 'operation choice' message from user (loop back)
                context.Wait(MessageReceivedOperationChoice);
            }
        }

        private async Task AfterChildDialogIsDone(IDialogContext context, IAwaitable<object> result)
        {
            // State transition - complete this Dialog and remove it from the stack
            context.Done<object>(new object());
        }
    }
}