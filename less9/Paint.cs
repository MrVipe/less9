using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Manager
{
    internal class Paint
    {

        private static string? path;
        private static int level = 0;
        private static int maxLevel = 0;
        private static int catalogPerStage = 15;
        private static int itemInDirector = 0;

        public Paint(string path, int islevel)
        {
            MyPath = path;
            Islevel = islevel;
        }
        public int ItemInDirector
        {
            get => itemInDirector;
            set => itemInDirector = value;
        }

        public string? MyPath
        {
            get => path;
            set => path = value;
        }

        public int CatalogPerStage
        {
            get => catalogPerStage;
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

        public void RePaintAll(string path, int index, int stage) //перерисовываем все
        {
            Console.Clear();
            RePaintTop(path, index);
            RePaintStructure(path, index, stage);
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

        static void RePaintStructure(string oldPath, int index, int stage) //перерисовываем каталоги
        {
            if (Directory.Exists(oldPath) )
            {
                if (stage == 0) { ConsColr("..", index == -1); }
                string[] getDir = Directory.GetDirectories(oldPath); //получаем директории
                string[] getFile = Directory.GetFiles(oldPath); //получаем файлы
                string[] fullItem = getDir.Concat(getFile).ToArray();
                Manager.Paint.itemInDirector = fullItem.Length;
                maxLevel = fullItem.Length - 1;
                if (itemInDirector != 0)
                {
                    for (int i = 0 + stage * catalogPerStage; i < itemInDirector && i < (stage + 1) * catalogPerStage; i++)
                    {
                        ConsColr(Path.GetFileName(fullItem[i]), (i == index));
                    }
                    ConsStagePaint(itemInDirector, stage);
                }
            }
            for (int i = 0; i < 100; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine("");
        }

        private static void ConsStagePaint(int length, int stage)
        {
            int cur = ((stage + 1) * catalogPerStage);
            cur = (cur > length) ? length : cur;
            if (length > catalogPerStage)
            {
                Console.WriteLine("//--------------------------//");
                Console.WriteLine($"Показано запись {cur} из {length}");
                Console.WriteLine("//--------------------------//");
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
                return GetUpPath(oldPath); //получаем директории
            }
        }

        public string GetUpPath(string oldpath) //На папку назад
        {
            string? newPath;
            newPath = Path.GetDirectoryName(oldpath);

            return (newPath != null) ? newPath : oldpath;

        }

        static void ConsColr(string curPath, bool isMyRow) //отрисовка выбранного каталога
        {

            if (isMyRow)
            {
                Console.WriteLine(curPath + @" < 0-----<");
            }
            else
            {
                Console.WriteLine(curPath);
            }

        }

    }
}
