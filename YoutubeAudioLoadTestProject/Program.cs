// See https://aka.ms/new-console-template for more information

using Xabe.FFmpeg;
using YoutubeAudioLoadTestProject;
using YoutubeExplode;

FFmpeg.SetExecutablesPath("ffmpeg/");

if (!Directory.Exists("audios"))
    Directory.CreateDirectory("audios");

if (!Directory.Exists("tempVideos"))
    Directory.CreateDirectory("tempVideos");

var youtube = new YoutubeClient();
new YoutubeDownloader(youtube).DownloadVideos("PLOuFobkMOG2nB3BJtCoacc06ecrSpmwFA").Wait();

if (Directory.Exists("tempVideos"))
    Directory.Delete("tempVideos", true);