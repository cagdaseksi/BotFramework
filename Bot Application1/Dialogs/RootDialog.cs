using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Linq;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            string reply = string.Empty;

            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            Relpy(connector, activity, "Düşünüyorum. Lütfen bekleyin!");

            System.Threading.Thread.Sleep(3000);

            switch (activity.Text)
            {
                case "Hi":
                    reply = "Hi, welcome.";
                    break;
                case "Hello":
                    reply = "Hello, welcome.";
                    break;
                case "Selam":
                    reply = "Selam kardeş, hoş geldin.";
                    break;
                case "Merhaba":
                    reply = "Merhaba kardeş, hoş geldin.";
                    break;

                default:
                    reply = "Üzgünüm bunu anlayamadım...";
                    break;
            }

            //// return our reply to the user
            await context.PostAsync($"CEVAP : " + reply + "");

            context.Wait(MessageReceivedAsync);
        }

        private void Relpy(ConnectorClient connector, Activity activity, string message)
        {
            if (connector != null && activity != null && !string.IsNullOrEmpty(activity.Id))
            {
                var reply = activity.CreateReply(message);
                connector.Conversations.ReplyToActivity(reply);
            }
        }

        //private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        //{
        //    var activity = await result as Activity;

        //    // calculate something for us to return
        //    int length = (activity.Text ?? string.Empty).Length;

        //    // return our reply to the user
        //    await context.PostAsync($"You sent {activity.Text} which was {length} characters");

        //    context.Wait(MessageReceivedAsync);
        //}
    }
}