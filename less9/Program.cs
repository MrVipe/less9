using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyApp
{

    internal class Program
    {
        static void Main(string[] args)
        {

            string path = @"C:\Program Files (x86)\";
            int indexRow = 0;
            Manager.Paint myManager = new Manager.Paint(path, indexRow);

       

            if (Directory.Exists(path))
            {
                while (true)
                {
                    myManager.RePaintAll(path, indexRow);
                    ConsoleKeyInfo myCommand = Console.ReadKey();
                    switch (myCommand.Key)
                    {
                        case ConsoleKey.UpArrow:
                            {
                                if (indexRow >= 0) { indexRow--; }
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            {
                                if (indexRow < myManager.MaxLevel) { indexRow++; }
                            }
                            break;
                        case ConsoleKey.Enter:
                            {
                                path = myManager.GetNewCatalog(path, indexRow);
                                indexRow = -1;
                            }
                            break;
                    }
                }
            }
            Console.ReadKey();
        }


    

        // string[] getFile = Directory.GetFiles(oldPath).Select(Path.GetFileName).ToArray();
 
        static string getParentPath(string pathChild, int thislvl)
        {
            int lvl = 0;
            string path = "";
            for (int i = 0; i < pathChild.Length; i++)
            {
                path += pathChild[i];
                if ((Directory.Exists(path)) & (thislvl == lvl))
                {
                    return path + @"\";
                }
            }
            return "";
        }
        static string StrProbel(int lvl) //красота
        {
            string probel = "";
            for (int i = 0; i < lvl; i++)
            {
                probel += " ";
            }

            return probel;
        }


        //static void GetNewPath(int lvl, string oldPath)
        //{
        //    File.AppendAllText("recurDir.txt", StrProbel(lvl) + oldPath + Environment.NewLine);// Записываем директорию
        //    string[] getDir = Directory.GetDirectories(oldPath); //получаем директорию
        //    string[] getFile = Directory.GetFiles(oldPath).Select(Path.GetFileName).ToArray(); //получаем файлы в тек. директории
        //    for (int i = 0; i < getFile.Length; i++)
        //    {
        //        getFile[i] = StrProbel(lvl + 5) + getFile[i];
        //    }
        //    File.AppendAllLines("recurDir.txt", getFile); //записываем файлы

        //    if (getDir.Length != 0)
        //    {
        //        foreach (string item in getDir) //проходы по директориям
        //        {
        //            GetNewPath(lvl + 3, item);
        //        }
        //    }
        //}

    }
}