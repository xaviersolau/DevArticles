// See https://aka.ms/new-console-template for more information
using MyEmbeddedLoadingProject;


var loader = new Loader(typeof(Program).Assembly);

await loader.LoadAndDisplay("global.json");


await loader.LoadAndDisplay("global.fr-FR.json");
