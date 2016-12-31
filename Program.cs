using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{

    /// <summary>
    /// Pass in the web page to extract new items that have been announced in a year
    /// e.g. AwsWhatsNew https://aws.amazon.com/about-aws/whats-new/2016/
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        MainAsync(args);
        Console.ReadLine();
    }

    static async void MainAsync(string[] args)
    {
        // Setup the configuration to support document loading
        var config = Configuration.Default.WithDefaultLoader();

        // Load the web page to parse
        var address = args[0];
        IHtmlCollection<IElement> newItems = await GetNewItemsAsync(config, address);

        var items = GetItems(newItems);
        foreach (var item in items)
        {
            Console.WriteLine(item);
        }
    }

    private static async Task<IHtmlCollection<IElement>> GetNewItemsAsync(IConfiguration config, string address)
    {
        // Asynchronously get the document in a new context using the configuration
        var document = await BrowsingContext.New(config).OpenAsync(address);
        var newItems = document.QuerySelectorAll("li.directory-item.text.whats-new");
        return newItems;
    }
    private static IList<string> GetItems(IHtmlCollection<IElement> newItems)
    {
        var releaseDate = string.Empty;
        var releaseTitle = string.Empty;
        var itemCount = 0;

        IList<string> items = new List<string>();
        foreach (var newItem in newItems)
        {
            itemCount++;
            releaseDate = newItem.QuerySelector("div.date").TextContent.Trim().Substring(11); // Strip "Posted On:"
            releaseTitle = newItem.QuerySelector("a").TextContent.Trim();
            items.Add(string.Format("{0},\"{1}\",\"{2}\"", itemCount, releaseDate, releaseTitle));
        }
        return items;
    }


}