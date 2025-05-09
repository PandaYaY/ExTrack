using ExTrack.Api;

var startUp = new StartUp();
var app     = startUp.InitApplication();
await app.RunAsync();
