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
        public double Count { get; set; }
        Texture2D sprite;
        Rectangle size;
        int wWidth;
        int wHeight;
        Random rng = new Random();
        public int Start { get; set; } = 0;
        public int End { get; set; } = 30;
        public List<decimal> numEnemies = new List<decimal>();
        public double limitation { get; set; } = .1;


        public int WaveNum { get; set; }

        public void Initialize()
        {
            Enemies = new List<Enemy>();
            Count = 0;
        }

        public EnemyManager(int wWidth, int wHeight)
        {
            this.wWidth = wWidth;
            this.wHeight = wHeight;
        }
        
        public void SpawnEnemy(Enemy enemy)
        {
            Enemies.Add(enemy);
        }

        public void FirstWave()
        {
            WaveNum = 1;
        }

        public void WaveProgress(Enemy enemy, int interval)
        {
            //for (int i = 0; i < WaveNum; i++)
            //{
            //    SpawnEnemy(enemy);
            //}
            //WaveNum++;
            //SpawnFormula();

            SpawnEnemy(enemy);

            

        }

        public List<decimal> SpawnFormula(double dampener)
        {
            for (int i = Start; i < End; i++)
            {
                if (i % 5 == 0)
                {

                    decimal value = Math.Ceiling((decimal)Math.Exp(i * dampener));

                    numEnemies.Add(value); 

                }
            }
            return numEnemies;

        }

    }
}
