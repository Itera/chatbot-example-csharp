using System;
using System.Linq;
using System.Threading.Tasks;
using Alfred.Models;
using Alfred.Properties;
using Alfred.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using BotAssets.Extentions;

namespace Alfred.Dialogs
{
    [Serializable]
    public class AlfredNobelDialog : IDialog<UserData>
    {
        public Task StartAsync(IDialogContext context)
        {
            return InitialPrompt(context);
        }

        private async Task InitialPrompt(IDialogContext context)
        {
            await context.PostAsync(Resources.AlfredNobelDialog_InformationText);
            await OnInitialInformationShown(context);
        }

        private async Task OnInitialInformationShown(IDialogContext context)
        {
            var reply = context.MakeMessage();

            var options = new[]
            {
                Resources.AlfredNobelDialog_WantMore_Choice1,
                Resources.AlfredNobelDialog_WantMore_Choice2
            };

            reply.AddHeroCard(Resources.AlfredNobelDialog_WantMore_Text, "", options);

            await context.PostAsync(reply);

            context.Wait(OnOptionSelected);
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message.Text == Resources.AlfredNobelDialog_WantMore_Choice1)
            {
                await context.PostAsync(Resources.AlfredNobelDialog_AdditionalInformation);
                await OnInitialInformationShown(context);
            }
            else if (message.Text == Resources.AlfredNobelDialog_WantMore_Choice2)
            {
                context.Done<object>(null);
            }
            else
            {
                await StartOverAsync(context, Resources.RootDialog_Welcome_Error);
            }
        }

        private async Task StartOverAsync(IDialogContext context, string text)
        {
            var message = context.MakeMessage();
            message.Text = text;
            await StartOverAsync(context, message);
        }

        private async Task StartOverAsync(IDialogContext context, IMessageActivity message)
        {
            await context.PostAsync(message);
            await InitialPrompt(context);
        }
    }
}