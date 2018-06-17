namespace DirectLineBot
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class DirectLineBotDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            var reply = context.MakeMessage();

            reply.Text = $"I am the Boring Bot. I heard you say '{message.Text}'";
         //   reply.Text = "Do I look omniscient? How should I know?"; 
            await context.PostAsync(reply);
            context.Wait(this.MessageReceivedAsync);
        }
    }
}
