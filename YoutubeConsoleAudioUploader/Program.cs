using Newtonsoft.Json;
using YoutubeConsoleUploader;

Directory.CreateDirectory(YoutubeUploader.VideosPath);
var text = File.ReadAllText("setting.json");
var setting = JsonConvert.DeserializeObject<Setting>(text);

var youtubeUploader = new YoutubeUploader(setting!);
await youtubeUploader.Upload();