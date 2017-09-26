using System;
using System.Configuration;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(ConfigurationManager.AppSettings["LuisAppId"], ConfigurationManager.AppSettings["LuisAPIKey"])))
        {
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"You have reached the none intent. You said: {result.Query}"); //
            context.Wait(MessageReceived);
            //some code
        }
        
        [LuisIntent("Note.AddToNote")]
        public async Task AddToNote(IDialogContext context, LuisResult result)
        {
            if (result.Entities != null && result.Entities.Count > 0)
            {
                foreach (var entity in result.Entities)
                {
                    await context.PostAsync(entity.Entity);
                }
            }
            else
            {
                //added
                await context.PostAsync("none");
            }
            context.Wait(MessageReceived);
        }

        [LuisIntent("Note.Create")]
        public async Task CreateNote(IDialogContext context, LuisResult result)
        {
            if (result.Entities != null && result.Entities.Count > 0)
            {
                foreach (var entity in result.Entities)
                {
                    await context.PostAsync(entity.Entity);
                }
            }
            else
            {
                //added
                await context.PostAsync("none");
            }
            context.Wait(MessageReceived);
        }

        [LuisIntent("MyIntent")]
        public async Task MyIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"You have reached the MyIntent intent. You said: {result.Query}"); //
            context.Wait(MessageReceived);
        }
    }
}