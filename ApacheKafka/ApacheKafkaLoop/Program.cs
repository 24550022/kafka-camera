// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");
int count = 100;
var list = new List<Process>();
for (int i = 0; i < count; i++)
{
    list.Add(Process.Start("ApacheKafka.exe", $"{i}"));
}

var readKey = Console.ReadLine();
foreach (var item in list)
    item.Dispose();