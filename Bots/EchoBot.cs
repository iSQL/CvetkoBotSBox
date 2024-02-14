// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.22.0

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CvetkoBot.Bots
{
	public class EchoBot : ActivityHandler
	{
		string RunBashCommand(string command)
		{
			// On Windows, if using WSL:
			// var bashPath = "wsl";
			// command = $"bash -c \"{command.Replace("\"", "\\\"")}\"";

			// For Linux or macOS, or if using Git Bash on Windows:
			var bashPath = "/bin/bash";
			var process = new Process()
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = bashPath,
					Arguments = $"-c \"{command.Replace("\"", "\\\"")}\"",
					RedirectStandardOutput = true,
					UseShellExecute = false,
					CreateNoWindow = true,
				}
			};

			process.Start();

			string result = process.StandardOutput.ReadToEnd();
			process.WaitForExit();
			return result;
			//Console.WriteLine(result);
		}
		protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
		{
			//CvetkoBotSBox
			var _telegramBotClient = new TelegramBotClient("6768910090:AAHhvivOvX5Kt8F7y0lLaGj9tMWN1br0Yes");
			var chatId = new ChatId(long.Parse(turnContext.Activity.Conversation.Id));

			// Check if the channel is Telegram
			if (turnContext.Activity.ChannelId.Equals("telegram", StringComparison.OrdinalIgnoreCase))
			{
				if (turnContext.Activity.Text.ToLower().Contains("cvetko"))
				{
					string result = RunBashCommand("oasis --help");
					await _telegramBotClient.SendTextMessageAsync(chatId, "Jel to mene neko pominje? " + result, cancellationToken: cancellationToken);
					return;
				}

				// Use Telegram.Bot client to send a Telegram-specific message
				//await _telegramBotClient.SendTextMessageAsync(chatId, "Pozz ja sam SBox Cvetko", cancellationToken: cancellationToken);
			}
			else
			{
				// Use the Bot Builder SDK for other channels
				var replyText = $"Echo BotSDK: {turnContext.Activity.Text}";
				await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
			}
		}
		protected override async Task OnConversationUpdateActivityAsync(ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
		{
			if (turnContext.Activity.MembersAdded != null && turnContext.Activity.MembersAdded.Any())
			{
				foreach (var member in turnContext.Activity.MembersAdded)
				{
					// Don't include the bot itself in the welcome message
					if (member.Id != turnContext.Activity.Recipient.Id)
					{
						await turnContext.SendActivityAsync(MessageFactory.Text("Zdravo i dobrodosao!"), cancellationToken);
					}
				}
			}
		}

		protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
		{
			var welcomeText = "Hej ti, cao!";
			foreach (var member in membersAdded)
			{
				if (member.Id != turnContext.Activity.Recipient.Id)
				{
					await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
				}
			}
		}
		public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
		{
			//This method should catch every type of events
			Debug.WriteLine($"Incoming activity type: {turnContext.Activity.Type}");
			await base.OnTurnAsync(turnContext, cancellationToken);
		}


	}
}
