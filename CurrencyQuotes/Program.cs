using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

class Program
{
    static async Task Main(string[] args)
    {
        string currencyCode = "USD"; // Here, enter the code of the currency you want to get a quote for
        string url = "https://www.cbr.ru/scripts/XML_daily.asp";

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var encoding1251 = Encoding.GetEncoding("windows-1251");

        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(url);
            var responseBytes = await response.Content.ReadAsByteArrayAsync();
            var content = encoding1251.GetString(responseBytes);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(content);

            XmlNodeList currencyNodes = xmlDoc.GetElementsByTagName("Valute");

            foreach (XmlNode node in currencyNodes)
            {
                XmlNode codeNode = node.SelectSingleNode("CharCode");
                if (codeNode != null && codeNode.InnerText == currencyCode)
                {
                    XmlNode valueNode = node.SelectSingleNode("Value");
                    if (valueNode != null)
                    {
                        Console.WriteLine($"Course {currencyCode}: {valueNode.InnerText}");
                        break;
                    }
                }
            }
        }
    }
}