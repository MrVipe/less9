using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    internal class Paint
    {

        private static string? path;
        private static int level = 0;
        private static int maxLevel = 0;

        public Paint(string path, int islevel)
        {
            Path = path;
            Islevel = islevel;
        }

        public string? Path
        {
            get => path;
            set => path = value;
        }

        public int Islevel
        {
            get => level;
            set => level = value;
        }

        public int MaxLevel
        {
            get => maxLevel;
            set => maxLevel = value;
        }

        public void RePaintAll(string path, int index) //перерисовываем все
        {
            Console.Clear();
            RePaintTop(path, index);
            RePaintStructure(path, index);
        }

        private static void RePaintTop(string path, int index) //перерисовываем верх
        {
            for (int i = 0; i < 100; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine("");
            Console.WriteLine(path);
            for (int i = 0; i < 100; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine("");
        }

        static void RePaintStructure(string oldPath, int index) //перерисовываем каталоги
        {
            if (Directory.Exists(oldPath))
            {
                ConsColr("..", index == -1);
                string[] getDir = Directory.GetDirectories(oldPath); //получаем директории
                maxLevel = getDir.Length - 1;
                if (getDir.Length != 0)
                {
                    int curIndex = 0;
                    foreach (string item in getDir) //проходы по директориям
                    {
                        for (int j = 0; j < item.Length; j++)
                        {
                            string pathSmall = "";
                            for (int k = item.Length - 1; k > 0; k--)
                            {
                                if (item[k] != Convert.ToChar(@"\"))
                                {
                                    pathSmall = item[k] + pathSmall;
                                }
                                else break;

                            }
                            ConsColr(pathSmall, (curIndex == index));
                            break;
                        }
                        curIndex++;
                    }
                }
            }

        }

        public string GetNewCatalog(string oldPath,int index)//переходим в новый каталог
        {
            if (index != -1)
            {
                string[] getDir = Directory.GetDirectories(oldPath); //получаем директории
                return getDir[index];
            }
            else
            {
                return getDir[index];
            }
        }

        static void ConsColr(string curPath, bool isMyRow) //отрисовка выбранного каталога
        {

            if (isMyRow)
            {
                Console.WriteLine(curPath + @"         <0-----<");
            }
            else
            {
                Console.WriteLine(curPath);
            }

        }

    }
}
