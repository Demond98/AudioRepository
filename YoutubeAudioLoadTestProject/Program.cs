// See https://aka.ms/new-console-template for more information

using Xabe.FFmpeg;
using YoutubeAudioLoadTestProject;
using YoutubeExplode;

//FileUploader.UploadFile("audios/RedRockForMotherRussia.mp3").Wait();


DirectoryExtensions.DeleteIfExist("tempVideos");
Directory.CreateDirectory("tempVideos");
await YoutubeDownloader.Load("PLOuFobkMOG2nB3BJtCoacc06ecrSpmwFA");