using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Клас для опису адреси
public class Address
{
    public string PostalCode { get; set; }
    public string Country { get; set; }
    public string Region { get; set; }
    public string District { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string House { get; set; }
    public string Apartment { get; set; }

    public override string ToString()
    {
        return $"Адреса: {PostalCode}, {Country}, {City}, {Street} буд. {House}";
    }
}

// Клас для опису об'єкта "Зоопарк"
public class Zoo
{
    // Оскільки вхідні дані описують тварину та її кількість у конкретному зоопарку,
    // і ми зчитуємо їх для *зоопарку*, то для кожного зоопарку ми можемо мати список тварин.
    // Але за умовою "розмір масиву статичний та рівний кількості параметрів, які описують об’єкт",
    // ми будемо вважати, що кожен блок даних - це інформація про *зоопарк*, де перші два поля 
    // - це *один вид* тварини в ньому.

    public string AnimalName { get; set; }      // Назва тварини (для фільтрації)
    public int SpeciesCount { get; set; }       // Кількість цього виду
    public Address Location { get; set; }
    public int TotalAnimals { get; set; }       // Загальна кількість тварин
    public int EmployeeCount { get; set; }      // Кількість працівників

    // Статичний розмір масиву для зчитування (використовується в логіці ReadData)
    // 1 (AnimalName) + 1 (SpeciesCount) + 8 (Address fields) + 1 (TotalAnimals) + 1 (EmployeeCount) = 12
    public const int ParameterCount = 12;

    public override string ToString()
    {
        return $"Тварина: {AnimalName}, Кількість: {SpeciesCount}\n" +
               $"{Location.ToString()}\n" +
               $"Загалом тварин: {TotalAnimals}, Працівників: {EmployeeCount}\n";
    }
}

public class DataProcessor
{
    private const string InputFileName = "Input Data.txt";
    private const string OutputFileName = "Output Data.txt";

    /// <summary>
    /// Зчитує дані про зоопарки з файлу.
    /// Використовує метод послідовного зчитування даних (StreamReader.ReadLine).
    /// </summary>
    public static List<Zoo> ReadData()
    {
        var zoos = new List<Zoo>();

        if (!File.Exists(InputFileName))
        {
            Console.WriteLine($"Помилка: Файл '{InputFileName}' не знайдено. Створіть його!");
            return zoos;
        }

        try
        {
            using (var sr = new StreamReader(InputFileName))
            {
                // Для відповідності вимозі "Організувати читання даних із файлу в двовимірний масив"
                // ми будемо зчитувати кожен об'єкт як масив рядків, а потім парсити його.
                // Розмір масиву: [Кількість об'єктів][Кількість параметрів]
                
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    // Це початок нового об'єкта Zoo, зчитуємо 12 послідовних рядків
                    string[] rawData = new string[Zoo.ParameterCount];
                    rawData[0] = line.Trim(); // AnimalName

                    for (int i = 1; i < Zoo.ParameterCount; i++)
                    {
                        string nextLine = sr.ReadLine();
                        if (nextLine == null)
                        {
                            Console.WriteLine("Помилка: Неповний набір даних в кінці файлу.");
                            return zoos; // Повертаємо те, що вже зчитали
                        }
                        rawData[i] = nextLine.Trim();
                    }

                    // Парсинг даних з масиву
                    var zoo = new Zoo
                    {
                        AnimalName = rawData[0],
                        SpeciesCount = int.Parse(rawData[1]),
                        Location = new Address
                        {
                            PostalCode = rawData[2],
                            Country = rawData[3],
                            Region = rawData[4],
                            District = rawData[5],
                            City = rawData[6],
                            Street = rawData[7],
                            House = rawData[8],
                            Apartment = rawData[9]
                        },
                        TotalAnimals = int.Parse(rawData[10]),
                        EmployeeCount = int.Parse(rawData[11])
                    };

                    zoos.Add(zoo);
                }
            }
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Помилка формату даних (очікувалося число): {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сталася помилка при читанні файлу: {ex.Message}");
        }

        return zoos;
    }

    /// <summary>
    /// Проводить обробку даних: вибір зоопарків з уссурійськими тиграми.
    /// </summary>
    public static List<Zoo> ProcessData(List<Zoo> allZoos)
    {
        // Завдання: Вивести відомості про зоопарки, де є уссурійські тигри.
        var filteredZoos = allZoos
            .Where(z => z.AnimalName.Equals("Уссурійський тигр", StringComparison.OrdinalIgnoreCase))
            .ToList();

        return filteredZoos;
    }

    /// <summary>
    /// Записує результат обробки у вихідний файл.
    /// </summary>
    public static void WriteData(List<Zoo> results)
    {
        try
        {
            // true в конструкторі StreamWriter означає додавання даних до існуючого файлу, 
            // але для лабораторної роботи зазвичай файл перезаписується, тому не передаємо другий аргумент.
            using (var sw = new StreamWriter(OutputFileName, false)) 
            {
                if (results.Any())
                {
                    sw.WriteLine($"=== ЗОНА РЕЗУЛЬТАТІВ: ЗОБАРКИ З УССУРІЙСЬКИМИ ТИГРАМИ (КІЛЬКІСТЬ: {results.Count}) ===");
                    foreach (var zoo in results)
                    {
                        sw.WriteLine("-------------------------------------");
                        sw.WriteLine(zoo.ToString());
                    }
                }
                else
                {
                    sw.WriteLine("Жодного зоопарку з уссурійськими тиграми не знайдено.");
                }
            }
            Console.WriteLine($"\nОбробку завершено. Результати записано у файл '{OutputFileName}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сталася помилка при записі файлу: {ex.Message}");
        }
    }
}

// Головна програма (імітація роботи GUI)
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== ЗАПУСК ЛАБОРАТОРНОЇ РОБОТИ: ОБРОБКА ДАНИХ ЗООПАРКІВ ===");

        // 1. Зчитування даних
        Console.WriteLine("\nЕтап 1: Читання даних з файлу...");
        var allZoos = DataProcessor.ReadData();

        if (!allZoos.Any())
        {
            Console.WriteLine("Немає даних для обробки. Завершення.");
            return;
        }

        // Відображення користувачу (імітація GUI)
        Console.WriteLine("\n--- ЗЧИТАНІ ДАНІ (ІМІТАЦІЯ ВІДОБРАЖЕННЯ НА GUI) ---");
        allZoos.ForEach(z => Console.WriteLine(z.ToString()));
        Console.WriteLine("----------------------------------------------------\n");

        // 2. Обробка даних
        Console.WriteLine("Етап 2: Обробка даних (пошук зоопарків з уссурійськими тиграми)...");
        var filteredZoos = DataProcessor.ProcessData(allZoos);

        // 3. Відображення та запис результатів
        Console.WriteLine("\n--- РЕЗУЛЬТАТИ ОБРОБКИ (ІМІТАЦІЯ ВІДОБРАЖЕННЯ НА GUI) ---");
        if (filteredZoos.Any())
        {
            filteredZoos.ForEach(z => Console.WriteLine(z.ToString()));
        }
        else
        {
            Console.WriteLine("Жодного зоопарку з уссурійськими тиграми не знайдено.");
        }
        Console.WriteLine("----------------------------------------------------------");

        DataProcessor.WriteData(filteredZoos);
        
        // Тут має бути код для запуску GUI додатку, якщо ви його використовуєте.
    }
}