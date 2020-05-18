using System;
using System.Threading;

namespace Quazide
{
	// Token: 0x02000004 RID: 4
	internal class Program
	{
		// Token: 0x0600001C RID: 28 RVA: 0x00002864 File Offset: 0x00000A64
		private static void Main(string[] args)
		{
			Console.Title = "GDK - By Brazy";
			Console.WriteLine("Loading config...");
			Bot.Config = Config.Load();
			Console.WriteLine("Config loaded successfully! logging into account...");
			Bot.Login();
			Console.WriteLine("Logged in successfully!");
			bool flag = !Bot.Config.UseCommand;
			if (!flag)
			{
				Console.WriteLine("Command mode activated, type " + Bot.Config.Prefix + Bot.Config.Command + " to nuke! :)");
				Thread.Sleep(-1);
				return;
			}
			for (;;)
			{
				Console.Clear();
				Console.WriteLine("Please specify an action");
				Console.WriteLine("1 - Nuke all servers");
				Console.WriteLine("2 - Nuke specific server");
				Console.Write("Action: ");
				int num = int.Parse(Console.ReadLine());
				int num2 = num;
				if (num2 != 1)
				{
					if (num2 != 2)
					{
						Console.WriteLine("Unknown mode.");
					}
					else
					{
						Console.Write("Server ID: ");
						long guildId = (long)ulong.Parse(Console.ReadLine());
						string text = "";
						bool botAccount = Bot.BotAccount;
						if (botAccount)
						{
							Console.Write("DM message (keep empty for none): ");
							text = Console.ReadLine();
						}
						string dm = text;
						Bot.NukeGuild((ulong)guildId, dm);
					}
				}
				else
				{
					string dm2 = "";
					bool botAccount2 = Bot.BotAccount;
					if (botAccount2)
					{
						Console.Write("DM message (hi): ");
						dm2 = Console.ReadLine();
					}
					Bot.NukeGuild(dm2);
				}
			}
		}
	}
}
