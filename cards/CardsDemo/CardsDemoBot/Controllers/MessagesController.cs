using CardsDemoBot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CardsDemoBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new CardsDemoDialog());
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }
    }
}