using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Microsoft.Bot.Connector.DirectLine;
using System.Threading.Tasks;
using TodoList.Droid;

[assembly: Dependency(typeof(TodoList.Droid.BotConnection))]

namespace TodoList.Droid
{
    class BotConnection : IBotConnection
    {
        public DirectLineClient Client = new DirectLineClient("Direct_Line_Key_Find_it_on_Azure_Bot_Service_Page");
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

                for(int i = activitySet.Activities.Count-1; i>=0; i++)
                {
                    if (activitySet.Activities[i].From.Name == "BotSampleSshekha")
                    {
                        return activitySet.Activities[i].Text;
                    }
                }

                await Task.Delay(3000);
            }
        }
    }
}