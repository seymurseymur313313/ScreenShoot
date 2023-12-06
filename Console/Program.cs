using System.Drawing.Imaging;
using System.Drawing;
using System.Net.Sockets;
using System.Net;

using System.Drawing.Imaging;

using System.Drawing;

using System.Net.Sockets;

using System.Net;
 
 
#pragma warning disable

byte[] CaptureAndSaveScreenshot()

{

    int width = 1920;

    int height = 1080;

    using (Bitmap bmp = new Bitmap(width, height))

    {

        using (Graphics g = Graphics.FromImage(bmp))

        {

            g.CopyFromScreen(0, 0, 0, 0, bmp.Size);

        }

        using (MemoryStream stream = new MemoryStream())

        {

            bmp.Save(stream, ImageFormat.Jpeg);

            return stream.ToArray();

        }

    }

}


UdpClient udpClient = new UdpClient(27001);

var remoteEP = new IPEndPoint(IPAddress.Any, 0);

while (true)

{

    var result = await udpClient.ReceiveAsync();

    remoteEP = result.RemoteEndPoint;

    new Task(async () =>

    {

        await Console.Out.WriteLineAsync(remoteEP.ToString());

        var screen = CaptureAndSaveScreenshot();

        var chunks = screen.Chunk(ushort.MaxValue - 29);

        foreach (var chunk in chunks)

        {

            await udpClient.SendAsync(chunk, chunk.Length, remoteEP);

        }

    }).Start();


}
