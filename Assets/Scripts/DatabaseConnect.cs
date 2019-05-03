using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;
using DigitalRuby.Threading;
using System.Text;

public class DatabaseConnect : MonoBehaviour
{
    private FtpWebRequest request;
    private string savePath = null;
    private byte[] downloadedData = new byte[1] { 0 };

    private string userName = "unity";
    private string password = "amrc";
    private string fileAddress = "ftp://unity@127.0.0.1/Current_Database.csv";

    /// <summary>
    /// either returns a byte array containing the file data or a failiur code
    /// </summary>
    /// <returns></returns>
    public byte[] getDatabase()
    {
        //local save path
        string path = Path.Combine(Application.persistentDataPath, "FTP Files");
        path = Path.Combine(path, "data.csv");
        savePath = path; // whether or not the file is saved locally

        GetDatabaseLoginDetails();
        Debug.Log(fileAddress + userName + password);

        byte[] yourFile = downloadWithFTP(fileAddress,userName,password);
        return yourFile;
    }

    /// <summary>
    /// main method for all ftp communication
    /// </summary>
    /// <param name="ftpUrl">file url in ftp server</param>
    /// <param name="userName">ftp username</param>
    /// <param name="password">account password</param>
    /// <returns></returns>
    private byte[] downloadWithFTP(string ftpUrl, string userName = "", string password = "")
    {
        request = (FtpWebRequest)WebRequest.Create(new Uri(ftpUrl));

        //request settings
        request.UsePassive = true;
        request.UseBinary = true;
        request.KeepAlive = true;
        request.Timeout = 300;

        //If username or password is NOT null then use Credential
        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
        {
            request.Credentials = new NetworkCredential(userName, password);
        }


        request.Method = WebRequestMethods.Ftp.DownloadFile;


        //If savePath is NOT null, we want to save the file to path
        //If path is null, we just want to return the file as array
        if (!string.IsNullOrEmpty(savePath))
        {
            //on a background thread downloadAndSave
            EZThread.ExecuteInBackground(downloadAndSave);


            return downloadedData;
        }
        else
        {
            //on a background thread downloadAsbyteArray
            EZThread.ExecuteInBackground(downloadAsbyteArray);

            return downloadedData;
        }
    }

    /// <summary>
    /// either returns a byte array of the file data or 0 when cannot connect
    /// </summary>
    private void downloadAsbyteArray()
    {
        WebResponse response = null;

        //check ftp server with your login details exists. if it does download to memory
        try
        {

            response = request.GetResponse();
        }
        catch (Exception)
        {
            //no connection
            downloadedData = new byte[1] { 0 };
            return;

        }

        //convert it to byte array
        using (Stream input = response.GetResponseStream())
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while (input.CanRead && (read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                downloadedData = ms.ToArray();
            }
        }
        return;

    }

    /// <summary>
    /// either returns a byte array of {0} when cannot connect or saves the download data to persistant memory
    /// </summary>
    private void downloadAndSave()
    {
        WebResponse response = null;
        //check ftp server with your login details exists. if it does download to memory
        try
        {
            response = request.GetResponse();
        }
        catch (Exception)
        {
            //no connection
            downloadedData = new byte[1] { 0 };
            return;

        }
        Stream reader = response.GetResponseStream();

        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(savePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
        }

        FileStream fileStream = new FileStream(savePath, FileMode.Create);


        //convert it to byte array and save to file
        int bytesRead = 0;
        byte[] buffer = new byte[2048];

        while (true)
        {
            bytesRead = reader.Read(buffer, 0, buffer.Length);

            if (bytesRead == 0)
                break;

            fileStream.Write(buffer, 0, bytesRead);
        }
        fileStream.Close();
        downloadedData = new byte[1] { 1 };
        return;
    }

    private void GetDatabaseLoginDetails()
    {
        string detailPath = Path.Combine(Application.streamingAssetsPath,"FTPDetails.csv");
        if (!File.Exists(detailPath))
        {
            Debug.Log("Not here " + detailPath);
            return;
        }
        else
        {
            string line = "";
            StreamReader theReader = new StreamReader(detailPath, Encoding.Default);
            using (theReader)
            {
                do
                {
                    line = theReader.ReadLine();

                    if (line != null && line != "" && !line.StartsWith("//"))
                    {
                        string[] values = line.Split(',');
                        fileAddress = values[0];
                        userName = values[1];
                        password = values[2];
                    }
                }
                while (line != null);
                theReader.Close();
            }
        }
    }
}
