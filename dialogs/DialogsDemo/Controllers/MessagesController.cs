using DialogsDemo.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace DialogsDemo
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                await SendTypingActivity(activity);

                // State transition - Set Root Dialog
                await Conversation.SendAsync(activity, () => new MainDialog());
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }

        private static async Task SendTypingActivity(Activity activity)
        {
            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            Activity isTypingReply = activity.CreateReply();
            isTypingReply.Type = ActivityTypes.Typing;
            await connector.Conversations.ReplyToActivityAsync(isTypingReply);

            
            Thread.Sleep(1000); // guarantee that the typing activity is shown
        }
    }
}