using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ResumptionCookieDemo
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        // Credit: Robin Osborne
        // http://robinosborne.co.uk/2017/01/02/sending-proactive-botframework-messages
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.GetActivityType() == "message")
            {
                var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                Activity reply = activity.CreateReply($"I don't know how to automatically deal with your query - so I've passed it on to a 👨🏽 who will get back to you!");
                await connector.Conversations.ReplyToActivityAsync(reply);

                var resumptionCookie = new ResumptionCookie(activity);
                var data = JsonConvert.SerializeObject(resumptionCookie);

                // For demonstration - save the cookie to disk.  For a real application
                // save to your persistent store - e.g. blob storage, table storage, document db, etc
                File.WriteAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/cookie.json"), data);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}