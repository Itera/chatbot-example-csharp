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
    public class PeacePrizeWinnersDialog : IDialog<UserData>
    {
        public Task StartAsync(IDialogContext context)
        {
            return InitialPrompt(context);
        }

        private async Task InitialPrompt(IDialogContext context)
        {
            var reply = context.MakeMessage();

            var options = new[]
            {
                Resources.PeacePrizeWinnersDialog_AskForUserData_Choice1,
                Resources.PeacePrizeWinnersDialog_AskForUserData_Choice2
            };

            reply.AddHeroCard(Resources.PeacePrizeWinnersDialog_AskForUserData_Text, "", options);

            await context.PostAsync(reply);

            context.Wait(OnOptionSelected);
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message.Text == Resources.PeacePrizeWinnersDialog_AskForUserData_Choice1)
            {
                context.Call(new UserInformationDialog(), UserInfoDialogDone);
            }
            else if (message.Text == Resources.PeacePrizeWinnersDialog_AskForUserData_Choice2)
            {
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

        public async Task UserInfoDialogDone(IDialogContext context, IAwaitable<UserData> result)
        {
            var user = await result;

            var prizeByYear = new PeacePrizeWinnerService().GetPrizeByYear(user.BirthYear);
            await context.PostAsync($"Visste du at {prizeByYear.Laureates.FirstOrDefault()?.Surname} vant nobels fredspris det året du ble født?");

            context.Done(user);
        }
    }
}