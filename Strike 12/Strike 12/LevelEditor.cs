using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
/// <summary>
/// Author: Copious Cats 
/// Purpose: This class will add data driven features to creating levels 
/// in order to stream line and simplify the creation of them
/// </summary>
namespace Strike_12
{
    class LevelEditor
    {
        // ----- | Fields | -----

        // File Reader Fields
        StreamReader output;
        char[,] levelLayout;
        string fileName;

        // ----- | Constructor | -----

        // ----- | Properties | -----

        // ----- | Methods | -----

        // TODO: test to see if the load method works
        // Load(int levelNum): takes in an int to and searches for a level to load up.
        public  void Load(int levelNum)
        {
            fileName = string.Format("..//Levels//Level{0}.txt", levelNum);
            string[] arenaSize = new string[2];
            string line;
            int count = -1;

            try
            {
                // Attempt to open up the StreamReader
                output = new StreamReader(fileName);

                while ((line = output.ReadLine()) != null)
                {
                    if (count == -1)
                    {
                        // At the first line get the size of the 2D array and initialize it
                        arenaSize = line.Split(',');
                        levelLayout = new char[int.Parse(arenaSize[0]), int.Parse(arenaSize[1])];
                    }
                    else
                    {
                        // For each line of the array loop through it and fill it
                        for (int i = 0; i < levelLayout.GetLength(0); i++)
                        {
                            for (int j = 0; j < levelLayout.GetLength(1); j++)
                            {
                                levelLayout[i, j] = line[j];
                            }

                            line = output.ReadLine();
                        }
                    }

                    count++;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            // If StreamReader exists then close it
            if (output != null)
            {
                output.Close();
            }
        }
    }
}
