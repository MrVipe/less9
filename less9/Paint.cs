using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Manager
{
    internal class Paint
    {

        private static string? path;
        private static int level = 0;
        private static int maxLevel = 0;
        private static int catalogPerStage = 15;
        private static int itemInDirector = 0;

        public Paint(string? path, int islevel)
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
            string? myFile = RePaintStructure(path, index, stage);
            RePaintBot(myFile);
        }

        private void RePaintBot(string? file)
        {
            Console.WriteLine($"");
            Console.WriteLine($"Информация:   ");

            if (file != null && file != "..")
            {
                if (File.Exists(file))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(file);
                    Console.WriteLine($"Имя файла: {fileInfo.Name}");
                    Console.WriteLine($"Версия файла: {myFileVersionInfo.FileVersion}");
                    Console.WriteLine($"Дата создания файла: {File.GetCreationTime(file)}");
                    Console.WriteLine($"Дата изменение файла: {File.GetLastWriteTime(file)}");
                    Console.WriteLine($"Размер файла: {GetRazmerMB(fileInfo.Length)} Мбайт");
                    Console.WriteLine($"Провообладатель: {myFileVersionInfo.LegalCopyright}");
                }
                else
                {
                    Console.WriteLine($"Имя директории: {Path.GetFileName(file)}");
                    Console.WriteLine($"Дата создания директории: {Directory.GetCreationTime(file)}");
                    Console.WriteLine($"Дата изменение директории: {Directory.GetLastWriteTime(file)}");
                    double catalogSize = 0;
                    Console.WriteLine($"Общий размер директории: {GetRazmerMB(SizeOfFolder(file, ref catalogSize))} Мбайт");
                }
            }
            Console.WriteLine($"");

            for (int i = 0; i < 100; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine("");
        }

        static double SizeOfFolder(string folder, ref double catalogSize)
        {
            try
            {
                //В переменную catalogSize будем записывать размеры всех файлов, с каждым
                //новым файлом перезаписывая данную переменную
                DirectoryInfo di = new DirectoryInfo(folder);
                DirectoryInfo[] diA = di.GetDirectories();
                FileInfo[] fi = di.GetFiles();
                //В цикле пробегаемся по всем файлам директории di и складываем их размеры
                foreach (FileInfo f in fi)
                {
                    //Записываем размер файла в байтах
                    catalogSize = catalogSize + f.Length;
                }
                //В цикле пробегаемся по всем вложенным директориям директории di 
                foreach (DirectoryInfo df in diA)
                {
                    //рекурсивно вызываем наш метод
                    SizeOfFolder(df.FullName, ref catalogSize);
                }
                return catalogSize;
            }
            catch
            {
                return 0;
            }
        }

        private object GetRazmerMB(double length)
        {
            if (length != 0)
            {
                return Math.Round((double)(length / 1024 / 1024));
            }
            return 0;
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

        static string? RePaintStructure(string oldPath, int index, int stage) //перерисовываем каталоги
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

                for (int i = 0; i < 100; i++)
                {
                    Console.Write("-");
                }
                Console.WriteLine("");

                if (index != -1)
                {
                    return (fullItem[index]);
                }
                else
                {
                    return null;
                }    
                    
            }

            return null;
        }

        private static void ConsStagePaint(int length, int stage) //Разграниченный список
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
