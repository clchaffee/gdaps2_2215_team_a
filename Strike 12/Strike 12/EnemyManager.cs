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
        public int NumEnemies { get; set; } = 0;
        public double limitation { get; set; } = .1;


        public int WaveNum { get; set; } = 1;

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




            //when a bullet enemy spawns, removes 2 from the list
            //if (enemy is BulletEnemy && Enemies.Count > 4)
            //{
            //    NumEnemies--;
            //}
            //when a bounce enemy spawns, removes 4 from the list
            if(enemy is Enemy)
            {
                enemy.MoveSpeed = rng.Next(3, 8);
            }
            if (enemy is BounceEnemy && Enemies.Count > 8)
            {
                NumEnemies -= 2;
            }
            //when a follow enemy spawns, removes 8 from the list
            if (enemy is FollowEnemy && Enemies.Count > 12)
            {
                NumEnemies -= 5;
            }
            //when a laser enemy spawns, removes 16 from the list
            if (enemy is LaserEnemy && Enemies.Count > 16)
            {
                NumEnemies -= 10;
            }

            SpawnEnemy(enemy);


        }

        public int SpawnFormula(double dampener, int interval, int intNum)
        {
            NumEnemies += (int)Math.Ceiling(Math.Exp(interval * dampener));
            NumEnemies += (int)Math.Ceiling(Math.Exp(((Start + End) / 15) * 2 * dampener));
            

            if (intNum != 0)
            {
                //NumEnemies -= SpawnFormula(dampener, (interval - 5));

                NumEnemies -= (int)Math.Ceiling(Math.Exp((interval - 5)* dampener));
                NumEnemies -= (int)Math.Ceiling(Math.Exp(((Start + End) / 15) * 2 * dampener));

            }

            return NumEnemies;

        }

    }
}
