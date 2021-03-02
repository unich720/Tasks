using System;
using System.Collections.Generic;
using System.Text;

namespace test
{
    class Task_1
    {


        public interface IDistanceUnits
        {
            public int milesPath { get; set; }
            public int KmPath { get; set; }
        }

        public class Units: IDistanceUnits
        {
            public int milesPath { get; set; }
            public int KmPath { get; set; }
            public Units(int KmPath,int milesPath)
            {
                this.KmPath = KmPath;
                this.milesPath = milesPath;
            }
        }

        public static void SavePathToDB(string cargoID, Units m)
        {
            
        }
        static void Main(string[] args)
        {
            var id = "test";
            Units m = new Units(100,80);
            SavePathToDB(id, m);

        }
    }
}
