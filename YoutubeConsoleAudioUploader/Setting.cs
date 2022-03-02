using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeConsoleUploader
{
	public class Setting
	{
		public bool IsAudio { get; set; }
		public int BatchSize { get; set; }
		public string[] YoutubeLinks { get; set; }
	}
}
