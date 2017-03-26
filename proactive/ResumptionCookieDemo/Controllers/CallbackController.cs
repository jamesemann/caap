using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ResumptionCookieDemo.Controllers
{
    public class CallbackController : ApiController
    {
        // This HTTP Endpoint simulates a long running Proactive callback.  Use a HTTP client to simulate:
        // 
        // POST http://localhost:3979/api/callback
        // Content-Type: application/json
        // 
        // { "Text" :"this is a proactive message!" }
        //
        // in reality, this code may be initiated by an event, a long running background task, or a schedule
        //
        // Credit: Robin Osborne
        // http://robinosborne.co.uk/2017/01/02/sending-proactive-botframework-messages
        public async Task<HttpResponseMessage> Post([FromBody] ProactiveMessage message)
        {
            // For demonstration - read the cookie from disk.  For a real application
            // read from your persistent store - e.g. blob storage, table storage, document db, etc
            var filepath = System.Web.Hosting.HostingEnvironment.MapPath("~/cookie.json");
            if (File.Exists(filepath))
            {
                var resumeJson = File.ReadAllText(filepath);

                var resumeData = JsonConvert.DeserializeObject< ConversationReference>(resumeJson);

                // this is the key bit, we are creating a resumption cookie then using it to create a reply
                //var resume = new ResumptionCookie(
                //    (string)resumeData.userId, 
                //    (string)resumeData.botId, 
                //    (string)resumeData.conversationId, 
                //    (string)resumeData.channelId,
                //    (string)resumeData.serviceUrl,
                //    "en");


                //var messageactivity = (Activity)resume.GetMessage();
                //var reply = messageactivity.CreateReply();
                //reply.Text = $"{message.Text}";
                //var resumeActivity = new Activity(ActivityTypes.Event, conversation: new ConversationAccount() {  },

                var trigger = new Activity(ActivityTypes.Message) // TODO ActivityTypes.Event doesnt exist
                {
                    ChannelId = resumeData.ChannelId,
                    Conversation = resumeData.Conversation,
                    ReplyToId = resumeData.ActivityId,
                    From = resumeData.Bot,
                    Id = Guid.NewGuid().ToString(),
                    Recipient = resumeData.User,
                    ServiceUrl = resumeData.ServiceUrl,
                    Text = message.Text,
                };
               

                var client = new ConnectorClient(new Uri(trigger.ServiceUrl));
                await client.Conversations.ReplyToActivityAsync(trigger);


                var result = resumeData.GetPostToBotMessage().CreateReply(message.Text);
                await client.Conversations.ReplyToActivityAsync(result);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }

    public class ProactiveMessage
    {
        public string Text { get; set; }
    }
}