// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using YoutubeConsoleUploader;

DirectoryExtensions.CreateDirectoryIfNotExist(YoutubeUploader.VideosPath);
var text = File.ReadAllText("setting.json");
var setting = JsonConvert.DeserializeObject<Setting>(text);

var youtubeUploader = new YoutubeUploader(setting!);
await youtubeUploader.Upload();
