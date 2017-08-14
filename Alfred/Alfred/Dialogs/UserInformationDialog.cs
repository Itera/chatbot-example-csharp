using System;
using System.Threading.Tasks;
using Alfred.Models;
using Alfred.Properties;
using Microsoft.Bot.Builder.Dialogs;

namespace Alfred.Dialogs
{
    [Serializable]
    public class UserInformationDialog : IDialog<UserData>
    {
        private UserData _user;

        public Task StartAsync(IDialogContext context)
        {
            _user = new UserData();
            return InitialPrompt(context);
        }

        public Task InitialPrompt(IDialogContext context)
        {
            PromptDialog.Text(context, AfterNameQuestion, Resources.UserInformationDialog_NameQuestion);
            return Task.CompletedTask;
        }

        public async Task AfterNameQuestion(IDialogContext context, IAwaitable<string> result)
        {
            var name = await result;
            _user.Name = name;

            PromptDialog.Number(context, AfterBirthYearQuestion, string.Format(Resources.UserInformationDialog_BirthYearQuestion, name));
        }

        public async Task AfterBirthYearQuestion(IDialogContext context, IAwaitable<long> result)
        {
            var birthYear = await result;

            _user.BirthYear = birthYear;

            PromptDialog.Text(context, AfterCountryQuestion, Resources.UserInformationDialog_CountryQuestion);
        }

        public async Task AfterCountryQuestion(IDialogContext context, IAwaitable<string> result)
        {
            var country = await result;
            _user.Country = country;
            context.Done(_user);
        }
    }
}