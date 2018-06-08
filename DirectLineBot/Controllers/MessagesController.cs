namespace DirectLineBot
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Microsoft.ApplicationInsights.TraceListener;
    using System.Diagnostics;

    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Microsoft.Bot.Connector.Activity activity)
        {
            Trace.TraceInformation($"MessagesController.Post received activity with text {activity.Text}");
            if (activity.Type == ActivityTypes.Message)
            {
                Trace.TraceInformation($"MessageController.Post Calling SendAsync with {activity.Text} because activityType == ActivityType.Messages is true");
                await Conversation.SendAsync(activity, () => new DirectLineBotDialog());
            }
            else
            {
                Trace.TraceInformation($"MessageController.Post Calling HandleSystemMessage with {activity.Text} because activityType is {activity.Type}");
                await this.HandleSystemMessage(activity);
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task HandleSystemMessage(Microsoft.Bot.Connector.Activity message)
        {
            Trace.TraceInformation($"HandleSystemMessage received message.text {message.Text}");
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                if (message.MembersAdded.Any(o => o.Id == message.Recipient.Id))
                {
                    ConnectorClient client = new ConnectorClient(new Uri(message.ServiceUrl));

                    var reply = message.CreateReply();

                    reply.Text = $"You have reached the DirectLine bot with Message of type {message.Type}. ";
                    reply.Text += (message.Text.Length == 0) ? "Message is empty." : message.Text;

                    await client.Conversations.ReplyToActivityAsync(reply);
                    return;
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

                    
        }
    }
}