using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace MyApp
{

    internal class Program
    {
        static void Main(string[] args)
        {

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            string? path = @"";
            try
            {
                string loadJson = File.ReadAllText("data.json");
                path = JsonConvert.DeserializeObject<String>(loadJson);
            }
            catch (Exception)
            {
                path = @"C:\";
            }
            int indexRow = 0;
            Manager.Paint myManager = new Manager.Paint(path, indexRow);
            int currentStage = 0;



            if (Directory.Exists(path))
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(100);
                    myManager.RePaintAll(path, indexRow, currentStage);
                    ConsoleKeyInfo myCommand = Console.ReadKey();
                    if ((myCommand.Key == ConsoleKey.UpArrow) || (myCommand.Key == ConsoleKey.DownArrow) ||
                            (myCommand.Key == ConsoleKey.LeftArrow) || (myCommand.Key == ConsoleKey.RightArrow) ||
                            (myCommand.Key == ConsoleKey.Enter))
                    {
                        switch (myCommand.Key)
                        {
                            case ConsoleKey.UpArrow:
                                {
                                    if (indexRow == 0 || indexRow > (currentStage) * myManager.CatalogPerStage) { indexRow--; }
                                }
                                break;
                            case ConsoleKey.DownArrow:
                                {
                                    if ((indexRow < (currentStage + 1) * myManager.CatalogPerStage - 1 || (indexRow == -1)) && indexRow < myManager.MaxLevel) { indexRow++; }
                                }
                                break;
                            case ConsoleKey.RightArrow:
                                {
                                    if ((currentStage + 1) * myManager.CatalogPerStage < myManager.ItemInDirector)
                                    {
                                        currentStage++;
                                        indexRow = currentStage * myManager.CatalogPerStage;
                                    }
                                }
                                break;
                            case ConsoleKey.LeftArrow:
                                {
                                    if (currentStage > 0)
                                    {
                                        currentStage--;
                                        indexRow = currentStage * myManager.CatalogPerStage;
                                    }
                                }
                                break;
                            case ConsoleKey.Enter:
                                {
                                    bool isErrorAccess = false;
                                    try
                                    {
                                        string[] getDir = Directory.GetDirectories(myManager.GetNewCatalog(path, indexRow));
                                        isErrorAccess = false;
                                    }
                                    catch (Exception err)
                                    {
                                        if (err.HResult == -2146233080)
                                        {
                                            isErrorAccess = true;
                                        }
                                        else
                                        { 
                                            Console.WriteLine("Отказано в доуступе: " + err.Message);
                                            isErrorAccess = true;
                                            Console.ReadKey();
                                        }
                                    }

                                    if (isErrorAccess == false)
                                    {
                                        currentStage = 0;
                                        path = myManager.GetNewCatalog(path, indexRow);
                                        string json = JsonConvert.SerializeObject(path); //заносим через Json в файл
                                        File.WriteAllText("data.json", json);
                                        indexRow = -1;
                                    }

                                }
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("");
                        string? myCommand2 = Console.ReadLine();
                        if (myCommand2[0] != '-')
                        {
                            Console.WriteLine("Неизвестная команда. Для отображение доступных команд наберите -hp");
                            Console.ReadKey();
                        }
                        else
                        {
                            string[]? regOut = RegularGo(myCommand2);

                            switch (regOut[0])
                            {
                                case "hp":
                                    {
                                        Console.WriteLine("Перейти в другой каталог -cd <путь>");
                                        Console.WriteLine("Создать директорию в текущем каталоге -md <название>");
                                        Console.WriteLine("Удалить директорию -kd <путь>");
                                        Console.WriteLine("Удалить файл в текущем каталоге -kf <название>");
                                        Console.WriteLine("Копировать директорию -cd <откуда> <куда>");
                                        Console.WriteLine("Копировать файл -cf <откуда> <куда>");
                                        Console.ReadKey();
                                    }
                                    break;
                                case "cd":
                                    {

                                        if (Directory.Exists(regOut[1]) == false)
                                        {
                                            Console.WriteLine($"Такого пути не существует: {regOut[1]}");
                                            Console.ReadKey();
                                        }
                                        else
                                        {
                                            path = regOut[1];
                                            indexRow = 0;
                                            currentStage = 0;
                                        }
                                    }
                                    break;
                                case "md":
                                    {

                                        if (Directory.Exists(path + regOut[1] + @"\") == true)
                                        {
                                            Console.WriteLine($"Такой каталог уже существует: {path + regOut[1]}");
                                            Console.ReadKey();
                                        }
                                        else
                                        {
                                            try
                                            {
                                                Directory.CreateDirectory(path + regOut[1] + @"\");
                                                path = path + regOut[1];
                                                indexRow = -1;
                                                currentStage = 0;
                                            }
                                            catch (Exception err)
                                            {
                                                Console.WriteLine("Упс. Произошла ошибка: " + err.Message);
                                            }

                                        }
                                    }
                                    break;
                                case "kd":
                                    {

                                        if (Directory.Exists(regOut[1]) == false)
                                        {
                                            Console.WriteLine($"Такого каталога не существует: {path + regOut[1]}");
                                            Console.ReadKey();
                                        }
                                        else
                                        {
                                            try
                                            {
                                                Directory.Delete(regOut[1], true);
                                                Console.WriteLine($"Каталог удален: {regOut[1]}");
                                                Console.ReadKey();
                                            }
                                            catch (Exception err)
                                            {
                                                Console.WriteLine("Упс. Произошла ошибка: " + err.Message);
                                            }

                                        }
                                    }
                                    break;
                                case "kf":
                                    {
                                        if (File.Exists(path + regOut[1]) == false)
                                        {
                                            Console.WriteLine($"Такого файла не существует: {path + regOut[1]}");
                                            Console.ReadKey();
                                        }
                                        else
                                        {
                                            try
                                            {
                                                Directory.Delete(path + regOut[1], true);
                                                Console.WriteLine($"Файл удален: {path + regOut[1]}");
                                                Console.ReadKey();
                                            }
                                            catch (Exception err)
                                            {
                                                Console.WriteLine("Упс. Произошла ошибка: " + err.Message);
                                            }

                                        }
                                    }
                                    break;
                                default:
                                    {
                                        Console.WriteLine("Неизвестная команда. Для отображение доступных команд наберите -hp");
                                        Console.ReadKey();
                                    }
                                    break;
                            }
                        }
                    }

                }
            }
            Console.ReadKey();
        }


        static string[]? RegularGo(string text)
        {
            string[]? outadata = new string[3];
            Match match = Regex.Match(text, "\\-([a-z]{2})\\s?([a-z.\\:]*)?\\s?([a-z\\.:]*)?", RegexOptions.IgnoreCase);

            outadata[0] = match.Groups[1].Value;
            outadata[1] = match.Groups[2].Value;
            outadata[2] = match.Groups[3].Value;

            return outadata;
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