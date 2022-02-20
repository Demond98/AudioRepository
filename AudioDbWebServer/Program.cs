using Dapper;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using AudioDbWebServer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

var connectString = "Data Source=C:\\Users\\Lenovo\\source\\repos\\Demond98\\AudioRepository\\AudioDbWebServer\\bin\\Debug\\net6.0\\data\\db.db";
using var db = new SqliteConnection(connectString);

app.MapGet("/test", async () => await db.QueryAsync<Dog>(@"SELECT * FROM Dogs"));

app.Run();