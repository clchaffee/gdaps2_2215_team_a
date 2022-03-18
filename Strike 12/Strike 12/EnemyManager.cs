using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Strike_12
{
    class EnemyManager
    {
        public List<Enemy> Enemies { get; set; }//= new List<Enemy>()
        Texture2D sprite;
        Rectangle size;
        int wWidth;
        int wHeight;
        Random rng = new Random();


        public int WaveNum { get; set; }

        public void Initialize()
        {
            Enemies = new List<Enemy>();
        }

        public EnemyManager(Texture2D sprite, Rectangle size, int wWidth, int wHeight)
        {
            this.wWidth = wWidth;
            this.wHeight = wHeight;
            this.size = size;
            size.X = rng.Next(300, wWidth - 300);
            size.Y = rng.Next(300, wHeight - 300);
            this.sprite = sprite;
        }
        
        public void SpawnEnemy(Enemy enemy)
        {
            Enemies.Add(enemy);
        }

        public void FirstWave()
        {
            WaveNum = 1;
        }

        public void WaveProgress(Enemy enemy)
        {
            for (int i = 0; i < WaveNum; i++)
            {
                SpawnEnemy(enemy);
            }
            WaveNum++;
        }


    }
}
