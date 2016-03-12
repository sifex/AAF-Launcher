using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AAF_Launcher
{
    class ArchivedCode
    {
        /* public void downloadFile(string sSourceURL, string sDestinationPath, double percent, int fileList)
        {
            long iFileSize = 0;
            int iBufferSize = 1024;
            iBufferSize *= 1000;
            long iExistLen = 0;
            long num = 0;

            System.IO.FileStream saveFileStream;
            if (System.IO.File.Exists(sDestinationPath))
            {
                System.IO.FileInfo fINfo =
                   new System.IO.FileInfo(sDestinationPath);
                iExistLen = fINfo.Length;
            }
            if (iExistLen > 0)
            {
                 saveFileStream = new System.IO.FileStream(sDestinationPath,
                  System.IO.FileMode.Append, System.IO.FileAccess.Write,
                  System.IO.FileShare.ReadWrite);
            }
            else
            {
                saveFileStream = new System.IO.FileStream(sDestinationPath,
                  System.IO.FileMode.Create, System.IO.FileAccess.Write,
                  System.IO.FileShare.ReadWrite);
            }

            System.Net.HttpWebRequest hwRq;
            System.Net.HttpWebResponse hwRes;
            hwRq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(sSourceURL);
            hwRq.AddRange((int)iExistLen);
            System.IO.Stream smRespStream;
            hwRes = (System.Net.HttpWebResponse)hwRq.GetResponse();
            smRespStream = hwRes.GetResponseStream();

            iFileSize = hwRes.ContentLength;

            int iByteSize;
            byte[] downBuffer = new byte[iBufferSize];

            while ((iByteSize = smRespStream.Read(downBuffer, 0, downBuffer.Length)) > 0)
            {
                num += iByteSize;
                saveFileStream.Write(downBuffer, 0, iByteSize);
                updateStatus("Downloading Mods (" + ((iExistLen + num) / 1024).ToString("N0") + " KB / " + ((iExistLen + iFileSize) / 1024).ToString("N0") + " KB)");
            }
        } */
    }
}
