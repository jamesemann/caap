using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ProactiveBot.Controllers
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
        // http://robinosborne.co.uk/2017/01/02/sending-proactive-botframework-messages
        public async Task<HttpResponseMessage> Post([FromBody] ProactiveMessage message)
        {
            // For demonstration - read the cookie from disk.  For a real application
            // read from your persistent store - e.g. blob storage, table storage, document db, etc
            var filepath = System.Web.Hosting.HostingEnvironment.MapPath("~/resume.json");
            if (File.Exists(filepath))
            {
                var resumeJson = File.ReadAllText(filepath);

                var resumeData = JsonConvert.DeserializeObject< ConversationReference>(resumeJson);

                var client = new ConnectorClient(new Uri(resumeData.ServiceUrl));

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