using System.Collections.Generic;
using System.IO;
using System.Net;
using Xunit;
using Zaaby.DFS.FastDfsProvider;

namespace UnitTest
{
    public class ZaabyFastDfsClientUnitTest
    {
        private readonly ZaabyFastDfsClient _client =
            new ZaabyFastDfsClient(new List<IPEndPoint> {new IPEndPoint(IPAddress.Parse("192.168.78.152"), 22122)},
                "group1");

        [Theory]
        [InlineData("1.jpg")]
        [InlineData("3.gif")]
        [InlineData("A000091712080DOF001.pdf")]
        public void FastDfsOperation(string fileName)
        {
            var uploadFile = File.ReadAllBytes(fileName);

            var dfsfileName = _client.UploadFile(uploadFile, fileName);

            var downloadFile = _client.DownloadFile(dfsfileName);

            _client.RemoveFile(dfsfileName);

            Assert.Equal(Md5Helper.Get32Md5(uploadFile), Md5Helper.Get32Md5(downloadFile));
        }
    }
}