using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Text;

public class DatabaseLookup : MonoBehaviour
{

    public DatabaseConnect databaseConnect;
    private Engraving lastPlate = null;
    private List<Engraving> names = new List<Engraving>();
    public PlateManager plateManager;

    private float databaseUpdateInterval = 60f*30;
    public class maxColumnRow
    {
        public int maxColumn { get; set; }
        public int maxRow { get; set; }
        public maxColumnRow(int column, int row)
        {
            maxRow = row;
            maxColumn = column;
        }
    }
    Dictionary<string, maxColumnRow> maxColumnRows = new Dictionary<string, maxColumnRow>();

    public List<Engraving> Names
    {
        get
        {
            return names;
        }

        set
        {
            names = value;
        }
    }

    public Dictionary<string, maxColumnRow> MaxColumnRows
    {
        get
        {
            return maxColumnRows;
        }

        set
        {
            maxColumnRows = value;
        }
    }

    public Engraving LastPlate
    {
        get
        {
            return lastPlate;
        }

        set
        {
            lastPlate = value;
        }
    }

    private void Awake()
    {
        // start requests to the ftp server
        InvokeRepeating("ReadCSV", 2f, databaseUpdateInterval);
    }

    /// <summary>
    /// search for the first plate containing that name. if no name exists do nothing
    /// </summary>
    /// <param name="name">can take in any string case sensitive.</param>
    public void SearchName(string name)
    {

        LastPlate = Names.Find(value => value.GetName().ToUpper() == name.ToUpper());

        if (LastPlate == null)
        {
            Debug.Log("No such name as: " + name);
            return;
        }


        plateManager.SearchForPlate(LastPlate.GetPanel());

    }

    /// <summary>
    /// reads the csv in the best format it can find and populates the Names engraving list with its data
    /// </summary>
    public void ReadCSV()
    {


        //defaults
        byte[] databaseConnectResult = new byte[1] { 0 };
        string path = "";
        string line = "";

        path = Application.streamingAssetsPath + @"/Current_Database.csv";
        string testPath = Path.Combine(Path.Combine(Application.persistentDataPath, "FTP Files"), "data.csv");

        StreamReader tempReader = new StreamReader(path, Encoding.Default);


        try
        {
            databaseConnectResult = databaseConnect.getDatabase();
        }
        catch (System.Exception e)
        {
            databaseConnectResult = new byte[1] { 0 };
            Debug.Log(e);

        }

        //no connection
        if (databaseConnectResult[0] == 0)
        {
            //has connected and downloaded in the past
            if (File.Exists(testPath))
            {
                tempReader = new StreamReader(testPath, Encoding.Default);
            }
            else
            {
                Debug.Log("default .csv file");
            }
            //else default .csv file
        }

        // connected data saved
        else if (databaseConnectResult[0] == 1)
        {
            tempReader = new StreamReader(testPath, Encoding.Default);
        }

        // connected data not saved
        else if (databaseConnectResult.Length > 1)
        {
            Stream stream = new MemoryStream(databaseConnectResult);
            tempReader = new StreamReader(stream);
        }

        //resets the names list
        Names = new List<Engraving>();

        StreamReader theReader = tempReader;

        //populate names list
        using (theReader)
        {

            do
            {
                line = theReader.ReadLine();
                if (line != null)
                {
                    string[] values = line.Split(',');
                    try
                    {
                        Names.Add(new Engraving(int.Parse(values[0]), values[1], values[2], values[3], int.Parse(values[4]), int.Parse(values[5])));
                        Debug.Log("Engraving Checked");
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("Somthing is wrong with\n" + line);

                    }
                    //Names.Add(new Engraving(int.Parse(values[0]), values[1], values[2], values[3], int.Parse(values[4]), int.Parse(values[5])));

                    //sets the maximum of each plates colums and rows
                    if (MaxColumnRows.ContainsKey(values[3]))
                    {
                        if (MaxColumnRows[values[3]].maxColumn < int.Parse(values[4]))
                        {
                            MaxColumnRows[values[3]].maxColumn = int.Parse(values[4]);
                        }
                        if (MaxColumnRows[values[3]].maxRow < int.Parse(values[5]))
                        {
                            MaxColumnRows[values[3]].maxRow = int.Parse(values[5]);
                        }
                    }
                    else MaxColumnRows.Add(values[3], new maxColumnRow(int.Parse(values[4]), int.Parse(values[5])));

                }
            }
            while (line != null);
            theReader.Close();
        }




    }



}




public class Engraving
{
    private int nameNumber;

    public int GetNameNumber()
    {
        return nameNumber;
    }

    public void SetNameNumber(int value)
    {
        nameNumber = value;
    }

    private string name;

    public string GetName()
    {
        return name;
    }

    public void SetName(string value)
    {
        name = value;
    }

    private bool marked;

    public bool GetMarked()
    {
        return marked;
    }

    public void SetMarked(bool value)
    {
        marked = value;
    }

    private string panel;

    public string GetPanel()
    {
        return panel;
    }

    public void SetPanel(string value)
    {
        panel = value;
    }

    private int column;

    public int GetColumn()
    {
        return column;
    }

    public void SetColumn(int value)
    {
        column = value;
    }

    private int row;

    public int GetRow()
    {
        return row;
    }

    public void SetRow(int value)
    {
        row = value;
    }

    /// <summary>
    /// class of each engravin on the plates
    /// </summary>
    public Engraving(int nameNumber, string name, string marked, string panel, int column, int row)
    {
        SetNameNumber(nameNumber);
        SetName(name);
        if (marked == "true" || marked == "yes" || marked == "Yes" || marked == "y" || marked == "Y" || marked == "1")
        {
            SetMarked(true);
        }
        else SetMarked(false);
        SetPanel(panel);
        SetColumn(column);
        SetRow(row);
    }


}

