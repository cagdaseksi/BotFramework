using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Linq;
using System;

namespace Bot_Application1
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                //await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
                await Conversation.SendAsync(activity, () => new ServiceManagerDialog());
            }
            else
            {
                await HandleSystemMessageAsync(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task<Activity> HandleSystemMessageAsync(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                if (message.MembersAdded.Any(o => o.Id == message.Recipient.Id))
                {
                    var reply = message.CreateReply("Merhaba, benim adım Dinabot. Sayfama Hoşgeldiniz.");

                    ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));

                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                //if (message.MembersAdded.Any(o => o.Id == message.Recipient.Id))
                //{
                var reply = message.CreateReply("Yaz dostum selam almayana yiğit denir mi");

                ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));

                await connector.Conversations.ReplyToActivityAsync(reply);
                //}
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}