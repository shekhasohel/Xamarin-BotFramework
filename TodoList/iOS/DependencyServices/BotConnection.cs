using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Microsoft.Bot.Connector.DirectLine;
using System.Threading.Tasks;
using TodoList.iOS;

[assembly: Dependency(typeof(TodoList.iOS.BotConnection))]

namespace TodoList.iOS
{
    class BotConnection : IBotConnection
    {
        public DirectLineClient Client = new DirectLineClient("6SH5WlNuNbY.cwA.j6k.2aDu-YJQISTpJlMNFME4hTZS8fPsXCrZSdi7iF7Il-8");
        public Conversation MainConversation;
        public ChannelAccount Account;

        public BotConnection()
        {
            MainConversation = Client.Conversations.StartConversation();
            Account = new ChannelAccount() { Id = "Sohel", Name = "Sohel" };
        }

        public async Task SendMessageAsync(string message)
        {
            Microsoft.Bot.Connector.DirectLine.Activity activity = new Microsoft.Bot.Connector.DirectLine.Activity
            {
                From = Account,
                Text = message,
                Type = Microsoft.Bot.Connector.DirectLine.ActivityTypes.Message
            };
            await Client.Conversations.PostActivityAsync(MainConversation.ConversationId, activity);
        }

        public async Task<string> GetMessagesAsync()
        {
            string watermark = null;

            while (true)
            {
                //Debug.WriteLine("Reading message every 3 seconds");

                //var activitySet = await Client.Conversations.GetActivitiesAsync(MainConversation.ConversationId, watermark);
                var activitySet = Client.Conversations.GetActivities(MainConversation.ConversationId, watermark);
                watermark = activitySet?.Watermark;

                foreach (Microsoft.Bot.Connector.DirectLine.Activity activity in activitySet.Activities)
                {
                    if (activity.From.Name == "BotSampleSshekha")
                    {
                        return activity.Text;
                    }
                }

                await Task.Delay(3000);
            }
        }
    }
}