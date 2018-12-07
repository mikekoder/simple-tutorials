using ClosedXML.Excel;
using HtmlAgilityPack;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;

namespace NordnetStocksToExcel
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://www.nordnet.fi/mux/web/marknaden/kurslista/aktier.html?marknad=Finland&lista=1_1&large=on&mid=on&small=on&sektor=0&subtyp=price&sortera=aktie&sorteringsordning=stigande";
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(url).Result;
                var body = response.Content.ReadAsStringAsync().Result;

                var doc = new HtmlDocument();
                doc.LoadHtml(body);

                var table = doc.GetElementbyId("kurstabell");
                var rows = table.SelectNodes("tbody/tr");
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Nasdaq Helsinki");
                    var line = 0;
                    foreach (var row in rows)
                    {
                        if(row.ChildNodes.Count < 12)
                        {
                            continue;
                        }
                        try
                        {
                            var name = row.SelectSingleNode("td[2]").InnerText;
                            var value = decimal.Parse(row.SelectSingleNode("td[3]").InnerText, new CultureInfo("fi-FI"));

                            line++;
                            worksheet.Cell(line, 1).Value = name;
                            worksheet.Cell(line, 2).Value = value;
                        }
                        catch
                        {

                        }
                    }

                    var filename = Path.Combine(Directory.GetCurrentDirectory(), "stocks.xlsx");
                    workbook.SaveAs(filename);
                }
            }

            Console.Read();
        }
    }
}
