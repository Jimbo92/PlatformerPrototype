using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

// 1  = Wall;
// 2 = Ladder;
// 3 = Water;
// 4 = Death Barrier;



namespace Platformer_Prototype
{
    public class MapLoader
    {
        public static int[,] LoadMapData(string MapDataFile)
        {
            int[,] loadMap;
            string path = AppDomain.CurrentDomain.BaseDirectory + "maps/" + MapDataFile + ".txt";
            var data = File.ReadAllLines(@path);
            loadMap = new int[data.Length, (data[0].Length+1)/2];
            for (int i = 0; i < data.Length; i++)
            {
                string line = data[i];
                string[] charr = line.Split(',');
                for (int j = 0; j < charr.Length; j++)
                    loadMap[i, j] = Convert.ToInt32(charr[j]);
            }
            return loadMap;
        }
    }
}

