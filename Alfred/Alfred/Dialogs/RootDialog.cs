using System;
using System.Threading.Tasks;
using Alfred.Models;
using Alfred.Properties;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using BotAssets.Extentions;

namespace Alfred.Dialogs
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
            await WelcomeMessageAsync(context);
        }

        private async Task WelcomeMessageAsync(IDialogContext context)
        {
            var reply = context.MakeMessage();

            var options = new[]
            {
                Resources.RootDialog_Welcome_Choice1,
                Resources.RootDialog_Welcome_Choice2
            };
            reply.AddHeroCard(
                Resources.RootDialog_Welcome_QuestionTitle,
                Resources.RootDialog_Welcome_QuestionText,
                options);

            await context.PostAsync(reply);

            context.Wait(OnOptionSelected);
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message.Text == Resources.RootDialog_Welcome_Choice1)
            {
                context.Call(new AlfredNobelDialog(), AlfredNobelDialogDone);

            }
            else if (message.Text == Resources.RootDialog_Welcome_Choice2)
            {
                context.Call(new PeacePrizeWinnersDialog(), PeacePrizeWinnersDialogDone);
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
            await WelcomeMessageAsync(context);
        }

        public async Task PeacePrizeWinnersDialogDone(IDialogContext context, IAwaitable<UserData> input)
        {
            var user = await input;

            if (user != null)
            {
                await context.PostAsync($"Takk {user.Name} for at du var interessert!");
            }
            context.Done<object>(null);
        }

        public async Task AlfredNobelDialogDone(IDialogContext context, IAwaitable<object> input)
        {
            context.Done<object>(null);
        }
    }
}