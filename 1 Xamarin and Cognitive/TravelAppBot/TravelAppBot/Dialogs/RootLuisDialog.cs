using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Builder.Resource;
using Microsoft.Bot.Connector;


namespace TravelAppBot.Dialogs
{
    /// <summary>
    /// you can get modelid and subscriptionkey from luis.ai
    /// 
    /// </summary>
    [Serializable]
    [LuisModel("24474ca3-51d5-4338-ba05-f6aac8c1e7c4", "ed802dde7db34d20894d6a552cd1d028")]
    public class RootLuisDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry, I did not understand '{result.Query}'. Type 'help' if you need assistance.";

            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hi! Try asking me things like 'book me a flight from london to paris' or 'what is the weather like in new york'");

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("BookFlight")]
        public async Task BookFlight(IDialogContext context, LuisResult result)
        {
            GetEntityRecommendation("Location::FromLocation", "FromLocation", result);
            GetEntityRecommendation("Location::ToLocation", "ToLocation", result);
            GetEntityRecommendation("builtin.datetime.date", "DepartureDate", result);

            await context.PostAsync("booking you flight...");

            var form = new BookFlightForm();
            var hotelsFormDialog = new FormDialog<BookFlightForm>(form, this.BuildHotelsForm, FormOptions.PromptInStart, result.Entities);
            context.Call(hotelsFormDialog, this.ResumeAfterFlightFormDialog);
        }

        private static void GetEntityRecommendation(string fromType, string toType, LuisResult result)
        {
            EntityRecommendation dateEntityRecommendation;
            if (result.TryFindEntity(fromType, out dateEntityRecommendation))
            {
                dateEntityRecommendation.Type = toType;
            }
        }

        private IForm<BookFlightForm> BuildHotelsForm()
        {
            OnCompletionAsyncDelegate<BookFlightForm> processBooking = async (context, state) =>
            {
                var message = "Thank you for completing the query, we are about to book you a flight...";
                await context.PostAsync(message);
            };

            return new FormBuilder<BookFlightForm>()
                .Field(nameof(BookFlightForm.FromLocation), (state) => string.IsNullOrEmpty(state.FromLocation))
                .Field(nameof(BookFlightForm.ToLocation), (state) => string.IsNullOrEmpty(state.ToLocation))
                .Field(nameof(BookFlightForm.DepartureDate), (state) => string.IsNullOrEmpty(state.DepartureDate))
                .OnCompletion(processBooking)
                .Build();
        }

        private async Task ResumeAfterFlightFormDialog(IDialogContext context, IAwaitable<BookFlightForm> result)
        {
            try
            {
                var searchQuery = await result;

                var fromLocation = searchQuery.FromLocation;
                var toLocation = searchQuery.ToLocation;
                var scheduledDate = searchQuery.DepartureDate;

                ////EXECUTE  BOOKFLIGHT

                var message = $"Your flight has been booked from {fromLocation} to {toLocation} scheduled {scheduledDate}";
                await context.PostAsync(message);
            }
            catch (FormCanceledException ex)
            {
                string reply;

                if (ex.InnerException == null)
                {
                    reply = "You have canceled the operation.";
                }
                else
                {
                    reply = $"Oops! Something went wrong :( Technical Details: {ex.InnerException.Message}";
                }

                await context.PostAsync(reply);
            }
            finally
            {
                context.Done<object>(null);
            }
        }

        [Serializable]
        public class BookFlightForm
        {
            [Prompt("From which city do you want to leave from? {||}", AllowDefault = BoolDefault.True)]
            [Describe("Location, example: Amsterdam")]
            public string FromLocation { get; set; }

            [Prompt("To which city you want to fly to? {||}", AllowDefault = BoolDefault.True)]
            [Describe("Location, example: Las Vegas")]

            public string ToLocation { get; set; }

            [Prompt("When do you want to leave? {||}", AllowDefault = BoolDefault.True)]
            [Describe("Departure date, example: tomorrow, next week or any date like 12-06-2016")]
            public string DepartureDate { get; set; }
        }
    }
}