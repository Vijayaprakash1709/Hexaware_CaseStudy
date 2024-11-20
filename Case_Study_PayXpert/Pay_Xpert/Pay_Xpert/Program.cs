using Pay_Xpert.Models;
using System.Diagnostics.Metrics;
using Pay_Xpert.Services.Interfaces;
using Pay_Xpert.Repository.Implementations;

namespace Pay_Xpert
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dashboard d= new Dashboard();
            d.dashboard();
        }
    }
}


