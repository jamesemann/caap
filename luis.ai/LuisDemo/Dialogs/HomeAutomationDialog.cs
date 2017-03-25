using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LuisDemo.Dialogs
{
    // Specify the Model ID and Subscription Key for the Home Automation LUIS Model
    [LuisModel("0325b8a2-40e4-4fbd-890d-4db8badbe1b7", "53aed799d50f4779aaf39f4657b59a1c")]
    [Serializable]
    public class HomeAutomationDialog : LuisDialog<object>
    {
        // Each intent in LUIS is mapped to a method on the Dialog using Attributes
        [LuisIntent("settemperature")]
        public async Task IntentSetTemperature(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Setting the temperature ... 🔥🌡☀🌶");
            context.Wait(MessageReceived);
        }

        [LuisIntent("opengaragedoor")]
        public async Task IntentOpenGarageDoor(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Opening the garage door ... 🚗");
            context.Wait(MessageReceived);
        }

        [LuisIntent("closegaragedoor")]
        public async Task IntentCloseGarageDoor(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Closing the garage door ... 🚗");
            context.Wait(MessageReceived);
        }

        // Add turnlightsoff Turning the lights off ... 💡

        [LuisIntent("")]
        public async Task IntentNone(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sorry, I don't know what you mean 😕");
            context.Wait(MessageReceived);
        }

        [LuisIntent("turnlightson")]
        public async Task IntentTurnLightsOn(IDialogContext context, LuisResult result)
        {
            // LuisDialog passes in a LuisResult which contains
            // - The possible Intents and their confidence
            // - Entities recognised by LUIS categorised by type
            // - The original query (the users message)
            var room = (from entity in result.Entities where entity.Type == "room" select entity).FirstOrDefault();

            await context.PostAsync($"Turning the {(room != null ? room.Entity : string.Empty)} lights on ... 💡");
            context.Wait(MessageReceived);
        }
    }
}