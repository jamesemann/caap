using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DialogsDemo
{
    public class Cards
    {
        public static IMessageActivity CreateImBackHeroCard(IMessageActivity replyMessage, string title, string[] values)
        {
            replyMessage.Text = title;

            var cardButtons = new List<CardAction>();
            foreach (var value in values)
            {
                CardAction plButton = new CardAction()
                {
                    Value = value,
                    Type = "imBack",
                    Title = value
                };
                cardButtons.Add(plButton);
            }
            HeroCard plCard = new HeroCard()
            {
                Buttons = cardButtons
            };
            Attachment plAttachment = plCard.ToAttachment();
            replyMessage.Attachments.Add(plAttachment);

            return replyMessage;
        }
    }
}