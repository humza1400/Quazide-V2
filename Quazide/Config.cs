using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Quazide
{
	// Token: 0x02000003 RID: 3
	public class Config
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000027C0 File Offset: 0x000009C0
		// (set) Token: 0x0600000D RID: 13 RVA: 0x000027C8 File Offset: 0x000009C8
		[JsonProperty("token")]
		public string Token { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000027D1 File Offset: 0x000009D1
		// (set) Token: 0x0600000F RID: 15 RVA: 0x000027D9 File Offset: 0x000009D9
		[JsonProperty("create_channels_amount")]
		public int CreateChannelsAmount { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000027E2 File Offset: 0x000009E2
		// (set) Token: 0x06000011 RID: 17 RVA: 0x000027EA File Offset: 0x000009EA
		[JsonProperty("create_channels_name")]
		public string CreateChannelsName { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000027F3 File Offset: 0x000009F3
		// (set) Token: 0x06000013 RID: 19 RVA: 0x000027FB File Offset: 0x000009FB
		[JsonProperty("cmd_mode")]
		public bool UseCommand { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002804 File Offset: 0x00000A04
		// (set) Token: 0x06000015 RID: 21 RVA: 0x0000280C File Offset: 0x00000A0C
		[JsonProperty("prefix")]
		public string Prefix { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002815 File Offset: 0x00000A15
		// (set) Token: 0x06000017 RID: 23 RVA: 0x0000281D File Offset: 0x00000A1D
		[JsonProperty("command")]
		public string Command { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002826 File Offset: 0x00000A26
		// (set) Token: 0x06000019 RID: 25 RVA: 0x0000282E File Offset: 0x00000A2E
		[JsonProperty("cmd_whitelist")]
		public List<ulong> CmdWhitelist { get; private set; }

		// Token: 0x0600001A RID: 26 RVA: 0x00002838 File Offset: 0x00000A38
		public static Config Load()
		{
			return JsonConvert.DeserializeObject<Config>(File.ReadAllText("Config.json"));
		}
	}
}
