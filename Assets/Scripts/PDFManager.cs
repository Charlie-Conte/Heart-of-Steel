using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// script to navigate through the images of a pdf
/// </summary>
public class PDFManager : MonoBehaviour
{
    public RawImage viewImage;
    /// <summary>
    /// all the pdf images
    /// </summary>
    public Texture[] pages = new Texture[1];
    Dictionary<int, Texture> reference = new Dictionary<int, Texture>();
    int currentPage = 0;
    private void Awake()
    {
        int i = 0;
        foreach(Texture t in pages)
        {
            reference.Add(i, t);
            i++;
        }
    }
    public void nextPage()
    {
        if (currentPage + 1 < reference.Count)
        {
            currentPage++;
            viewImage.texture = reference[currentPage];
        }
    }
    public void previousPage()
    {
        if (currentPage - 1 >= 0)
        {
            currentPage--;
            viewImage.texture = reference[currentPage];
        }
    }
    public void firstPage()
    {

            currentPage = 0;
            viewImage.texture = reference[currentPage];
        
    }
    public void lastPage()
    {
        currentPage = reference.Count - 1;
        viewImage.texture = reference[currentPage];
    }

}
