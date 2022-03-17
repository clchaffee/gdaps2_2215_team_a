﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        Tile[,] tileLayout;
        string fileName;

        // ----- | Properties | -----
        public Tile this[int i, int j]
        {
            get { return tileLayout[i, j]; }
        }

        public int LayoutColumns
        {
            get { return tileLayout.GetLength(1); }
        }

        public int LayoutRows
        {
            get { return tileLayout.GetLength(0); }
        }

        // ----- | Constructor | -----

        // ----- | Properties | -----

        // ----- | Methods | -----

        // TODO: test to see if the load method works
        // Load(int levelNum): takes in an int to and searches for a level to load up.
        public void Load(int levelNum, Texture2D tileSprites, int windowWidth, int windowHeight)
        {
            fileName = string.Format("..//..//..//Levels//Level{0}.txt", levelNum);
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
                        //levelLayout = new char[int.Parse(arenaSize[0]), int.Parse(arenaSize[1])];
                        tileLayout = new Tile[int.Parse(arenaSize[1]), int.Parse(arenaSize[0])];
                    }
                    else
                    {
                        // For each line of the array loop through it and fill it
                        for (int i = 0; i < tileLayout.GetLength(0); i++)
                        {
                            for (int j = 0; j < tileLayout.GetLength(1); j++)
                            {
                                switch (line[j])
                                {
                                    case 'w':
                                        tileLayout[i, j] = new Tile(tileSprites, 
                                                                    new Rectangle(128 * j, 128 * i, 128, 128),
                                                                    windowWidth,
                                                                    windowHeight,
                                                                    "wall");
                                        break;

                                    case 'x':
                                        tileLayout[i, j] = new Tile(tileSprites,
                                                                    new Rectangle(128 * j, 128 * i, 128, 128),
                                                                    windowWidth,
                                                                    windowHeight,
                                                                    "temp");
                                        break;

                                    default:
                                        tileLayout[i, j] = null;
                                        break;
                                }
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

        // Draw
        public void Draw(SpriteBatch _spriteBatch, Texture2D tileTexture)
        {

            for (int i = 0; i < tileLayout.GetLength(0); i++)
            {
                for (int j = 0; j < tileLayout.GetLength(1); j++)
                {
                    if (tileLayout[i, j] != null)
                    {
                        switch (tileLayout[i, j].Type)
                        {
                            case "wall":
                                _spriteBatch.Draw(tileTexture, tileLayout[i, j].Size, Color.White);
                                break;

                            case "temp":
                                _spriteBatch.Draw(tileTexture, tileLayout[i, j].Size, Color.White);
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}
