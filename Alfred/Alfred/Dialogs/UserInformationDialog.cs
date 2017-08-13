using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Alfred.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

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
            PromptDialog.Text(context, AfterNameQuestion, "Hva heter du?");
            return Task.CompletedTask;
        }

        public async Task AfterNameQuestion(IDialogContext context, IAwaitable<string> result)
        {
            var name = await result;
            _user.Name = name;

            PromptDialog.Number(context, AfterBirthYearQuestion, $"Hei {name}, hvilket år ble du født?");
        }

        public async Task AfterBirthYearQuestion(IDialogContext context, IAwaitable<long> result)
        {
            var birthYear = await result;

            _user.BirthYear = birthYear;

            PromptDialog.Text(context, AfterCountryQuestion, "Hvilket land kommer du fra?");
        }

        public async Task AfterCountryQuestion(IDialogContext context, IAwaitable<string> result)
        {
            var country = await result;
            _user.Country = country;
            context.Done(_user);
        }
    }
}