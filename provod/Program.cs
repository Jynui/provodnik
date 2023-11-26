using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main()
    {
        
        DriveInfo[] drives = DriveInfo.GetDrives();

  
        Console.WriteLine("Доступные диски:");
        for (int i = 0; i < drives.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {drives[i].Name}");
        }


        Console.Write("Выберите диск (введите номер): ");

        int selectedDriveIndex;
        if (!int.TryParse(Console.ReadLine(), out selectedDriveIndex) || selectedDriveIndex < 1 || selectedDriveIndex > drives.Length)
        {
            Console.WriteLine("Некорректный ввод.");
            return;
        }

        DriveInfo selectedDrive = drives[selectedDriveIndex - 1];


        Console.WriteLine("Информация о диске:");
        Console.WriteLine($"   Имя: {selectedDrive.Name}");
        Console.WriteLine($"   Тип: {selectedDrive.DriveType}");
        Console.WriteLine($"   Общий объем: {selectedDrive.TotalSize / (1024 * 1024 * 1024)} ГБ");
        Console.WriteLine($"   Доступный объем: {selectedDrive.AvailableFreeSpace / (1024 * 1024 * 1024)} ГБ");


        string currentPath = selectedDrive.RootDirectory.FullName;
        ExploreFolder(currentPath);
    }

    static void ExploreFolder(string folderPath)
    {
        int selectedOptionIndex = 0;
        bool isExitRequested = false;

        do
        {

            Console.Clear();
            Console.WriteLine($"Текущая папка: {folderPath}");
            Console.WriteLine();


            Console.WriteLine("Папки:");
            string[] folders = Directory.GetDirectories(folderPath);
            for (int i = 0; i < folders.Length; i++)
            {
                string folderName = Path.GetFileName(folders[i]);
                string indicator = i == selectedOptionIndex ? "»" : " ";
                Console.WriteLine($"{indicator} {i + 1}. {folderName}");
            }


            Console.WriteLine("Файлы:");
            string[] files = Directory.GetFiles(folderPath);
            for (int i = 0; i < files.Length; i++)
            {
                string fileName = Path.GetFileName(files[i]);
                string indicator = folders.Length + i == selectedOptionIndex ? "»" : " ";
                Console.WriteLine($"{indicator} {i + folders.Length + 1}. {fileName}");
            }


            Console.WriteLine();
            Console.WriteLine("Меню:");
            Console.WriteLine("   ↑. Перейти к предыдущей папке");
            Console.WriteLine("   ↓. Перейти к следующей папке");
            Console.WriteLine("   ←. Вернуться назад");
            Console.WriteLine("   →. Открыть выбранный файл/папку");
            Console.WriteLine("   Esc. Выйти из программы");

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedOptionIndex = Math.Max(0, selectedOptionIndex - 1);
                    break;
                case ConsoleKey.DownArrow:
                    selectedOptionIndex = Math.Min(folders.Length + files.Length - 1, selectedOptionIndex + 1);
                    break;
                case ConsoleKey.LeftArrow:
                    folderPath = Directory.GetParent(folderPath)?.FullName ?? folderPath;
                    selectedOptionIndex = 0;
                    break;
                case ConsoleKey.RightArrow:
                    string selectedPath = selectedOptionIndex < folders.Length
                        ? folders[selectedOptionIndex]
                        : files[selectedOptionIndex - folders.Length];

                    if (Directory.Exists(selectedPath))
                    {
                        folderPath = selectedPath;
                        selectedOptionIndex = 0;
                    }
                    else if (File.Exists(selectedPath))
                    {
                        Process.Start(selectedPath);
                    }
                    break;
                case ConsoleKey.Escape:
                    isExitRequested = true;
                    break;
            }
        } while (!isExitRequested);
    }
}
