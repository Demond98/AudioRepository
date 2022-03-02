namespace YoutubeConsoleUploader
{
	public static class DirectoryExtensions
	{
		public static void CreateDirectoryIfNotExist(string directory)
		{
			if (!Directory.Exists(directory))
				Directory.CreateDirectory(directory);
		}
	}
}
