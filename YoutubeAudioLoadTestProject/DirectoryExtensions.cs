using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeAudioLoadTestProject
{
	public static class DirectoryExtensions
	{
		public static void DeleteIfExist(string directory)
		{
			if (Directory.Exists(directory))
				Directory.Delete(directory, true);
		}
	}
}
