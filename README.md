# Zaaby.DFS.FastDfsProvider

[FastDFS](https://github.com/happyfish100/fastdfs) is an open source high performance distributed file system (DFS). It's major functions include: file storing, file syncing and file accessing, and design for high capacity and load balance.

## QuickStart

### Build Project

#### Create a asp.net core project and add the register code in ConfigureServices method(Startup.cs).192.168.5.61 is the ip of FastDfsProvider.Mongo.Repository and 192.168.78.152 is of the FastDFS tracker.The repostory is not necessary

    //Register the FastDfsClient repostory
    services.AddSingleton<Zaaby.DFS.Core.IRepository, Zaaby.DFS.FastDfsProvider.Mongo.Repository>(p =>
        new Zaaby.DFS.FastDfsProvider.Mongo.Repository(
            new Zaaby.DFS.FastDfsProvider.Mongo.MongoDbConfiger(new List<string> {"192.168.5.61:27017"},
                "FlytOaData", "FlytOaDev", "2016")));

    //Register the FastDfsClient
    services.AddSingleton<Zaaby.DFS.Core.IHandler, Zaaby.DFS.FastDfsProvider.ZaabyFastDfsClient>(p =>
        new Zaaby.DFS.FastDfsProvider.ZaabyFastDfsClient(
            new List<IPEndPoint> {new IPEndPoint(IPAddress.Parse("192.168.78.152"), 22122)},
            "group1", services.BuildServiceProvider().GetService<Zaaby.DFS.Core.IRepository>()));

#### Create a controller named DfsDemoController like this(add a gif file in the project named 3.gif) and run the web application

    [Route("api/[controller]/[action]")]
    public class DfsDemoController : Controller
    {
        private readonly Zaaby.DFS.Core.IHandler _dfsHandler;

        public DfsDemoController(Zaaby.DFS.Core.IHandler dfsHandler)
        {
            _dfsHandler = dfsHandler;
        }

        [HttpGet]
        [HttpPost]
        public string UploadFile()
        {
            var fileName = "3.gif";

            var uploadFile = System.IO.File.ReadAllBytes(fileName);

            return _dfsHandler.UploadFile(uploadFile, fileName);
        }

        [HttpGet]
        [HttpPost]
        public FileResult DownloadFile(string dfsFileName)
        {
            return File(_dfsHandler.DownloadFile(dfsFileName),"text/plain","test.gif");
        }

        [HttpGet]
        [HttpPost]
        public void RemoveFile(string dfsFileName)
        {
            _dfsHandler.RemoveFile(dfsFileName);
        }
    }

### Request the webapi

#### Upload file

Now you can access <http://localhost:5000/api/DfsDemo/UploadFile> in a browser and it will return a DFS filename in the response like "M00/00/00/wKhOmVpHXsyAZbMZAAJ7jVznybQ162.gif".You can access the storage server like <http://192.168.78.155:8080/group1/M00/00/00/wKhOmVpHW6CANytuAAJ7jVznybQ240.gif>(not the tracker server) to check the file whether is uploaded

#### Download file

Access <http://localhost:5000/api/DfsDemo/DownloadFile?dfsFileName=M00/00/00/wKhOmVpHW6CANytuAAJ7jVznybQ240.gif> in the browser and download the file

#### Remove file

Request <http://localhost:5000/api/DfsDemo/RemoveFile?dfsFileName=M00/00/00/wKhOmVpHkBiAK2AdAAJ7jVznybQ650.gif> to remove the file in FastDFS and the file data in resposity(in this example is mongo)