using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

// Клас для структури даних "Телефонна мережа"
public class RepairApplication
{
    // Поля для введення даних
    public string FullName { get; set; }        // ПІБ абонента
    public string PhoneNumber { get; set; }     // Номер телефону
    public string BreakDownDateStr { get; set; } // Дата поломки (ДДММРРРР)
    public string BreakDownTimeStr { get; set; } // Час поломки (ГГ:ХХ:СС)
    public string RepairDateStr { get; set; }    // Дата усунення (ДДММРРРР)
    public string RepairTimeStr { get; set; }    // Час усунення (ГГ:ХХ:СС)

    // Властивість для об'єднаної дати та часу поломки (для обчислень)
    public DateTime BreakDownDateTime => ParseDateTime(BreakDownDateStr, BreakDownTimeStr);

    // Властивість для об'єднаної дати та часу усунення (для обчислень)
    public DateTime RepairDateTime => ParseDateTime(RepairDateStr, RepairTimeStr);

    // Метод для обчислення терміну усунення в днях
    public double RepairDurationDays
    {
        get
        {
            // Перевірка, щоб уникнути від'ємного часу (якщо дата усунення не встановлена або помилкова)
            if (RepairDateTime <= BreakDownDateTime) return 0;
            TimeSpan duration = RepairDateTime - BreakDownDateTime;
            return duration.TotalDays;
        }
    }

    // Приватний метод для безпечного перетворення рядків ДДММРРРР та ГГ:ХХ:СС у DateTime
    private DateTime ParseDateTime(string dateStr, string timeStr)
    {
        if (string.IsNullOrEmpty(dateStr) || string.IsNullOrEmpty(timeStr))
        {
            return DateTime.MinValue;
        }

        // Форматуємо вхідні рядки для парсингу: ДДММРРРР + ГГ:ХХ:СС
        string combinedString = $"{dateStr}{timeStr}";
        string format = "ddMMyyyyHH:mm:ss";

        // Використовуємо TryParseExact для безпечного парсингу
        if (DateTime.TryParseExact(combinedString, format, CultureInfo.InvariantCulture,
                                  DateTimeStyles.None, out DateTime result))
        {
            return result;
        }
        
        Console.WriteLine($"Помилка парсингу дати/часу: {dateStr} {timeStr} для {FullName}");
        return DateTime.MinValue; // Повертаємо мінімальне значення при помилці
    }
    
    public override string ToString()
    {
        return $"Абонент: {FullName} (Тел: {PhoneNumber}), Поломка: {BreakDownDateTime:dd.MM.yyyy HH:mm:ss}, Усунення: {RepairDateTime:dd.MM.yyyy HH:mm:ss}";
    }
}

// Головний клас з логікою обробки
public class PhoneNetworkProcessor
{
    private List<RepairApplication> Applications { get; set; }

    public PhoneNetworkProcessor()
    {
        // Приклад набору даних для тестування
        Applications = new List<RepairApplication>
        {
            // Поломка минулого місяця (Жовтень 2025)
            new RepairApplication { FullName = "Іваненко І.І.", PhoneNumber = "101", 
                                    BreakDownDateStr = "15102025", BreakDownTimeStr = "10:00:00", 
                                    RepairDateStr = "17102025", RepairTimeStr = "12:00:00" }, 
            
            // Поломка цього року, найдовша (довше 3 днів)
            new RepairApplication { FullName = "Петренко П.П.", PhoneNumber = "102", 
                                    BreakDownDateStr = "01092025", BreakDownTimeStr = "09:00:00", 
                                    RepairDateStr = "05092025", RepairTimeStr = "15:00:00" },
            
            // Поломка минулого року (2024)
            new RepairApplication { FullName = "Сидоренко С.С.", PhoneNumber = "103", 
                                    BreakDownDateStr = "20122024", BreakDownTimeStr = "14:00:00", 
                                    RepairDateStr = "21122024", RepairTimeStr = "16:00:00" },
                                    
            // Поломка цього року, коротка
            new RepairApplication { FullName = "Коваленко К.К.", PhoneNumber = "104", 
                                    BreakDownDateStr = "28102025", BreakDownTimeStr = "08:00:00", 
                                    RepairDateStr = "28102025", RepairTimeStr = "18:00:00" },
                                    
            // Поломка за межами "останній рік" (2023)
            new RepairApplication { FullName = "Мороз М.М.", PhoneNumber = "105", 
                                    BreakDownDateStr = "01012023", BreakDownTimeStr = "09:00:00", 
                                    RepairDateStr = "02012023", RepairTimeStr = "09:00:00" }
        };
    }

    // --- ФУНКЦІОНАЛЬНІ ВИМОГИ ---

    // 1. Вивести відомості щодо всіх заявок із зазначенням терміну усунення несправності у днях
    public void DisplayAllWithDuration()
    {
        Console.WriteLine("\n--- 1. Відомості щодо всіх заявок (з терміном усунення) ---");
        foreach (var app in Applications)
        {
            Console.WriteLine($"{app.ToString()}, Термін усунення: {app.RepairDurationDays:F2} днів");
        }
    }

    // 2. Вивести усі відомості про поломки за минулий місяць
    public void DisplayLastMonthBreakdowns()
    {
        DateTime now = DateTime.Now; // Припустимо, зараз 02.11.2025
        DateTime firstDayOfThisMonth = new DateTime(now.Year, now.Month, 1);
        DateTime firstDayOfLastMonth = firstDayOfThisMonth.AddMonths(-1);

        var lastMonthApps = Applications
            .Where(a => a.BreakDownDateTime >= firstDayOfLastMonth && a.BreakDownDateTime < firstDayOfThisMonth)
            .ToList();

        Console.WriteLine($"\n--- 2. Поломки за минулий місяць ({firstDayOfLastMonth:MMMM yyyy}) ---");
        if (lastMonthApps.Any())
        {
            foreach (var app in lastMonthApps)
            {
                Console.WriteLine(app.ToString());
            }
        }
        else
        {
            Console.WriteLine("Заявок за минулий місяць не знайдено.");
        }
    }

    // 3. Підрахувати кількість заявок на ремонт минулого року та відобразити інформацію
    public void DisplayLastYearApplications()
    {
        int lastYear = DateTime.Now.Year - 1; // 2024
        
        var lastYearApps = Applications
            .Where(a => a.BreakDownDateTime.Year == lastYear)
            .ToList();

        Console.WriteLine($"\n--- 3. Заявки за минулий рік ({lastYear}) ---");
        Console.WriteLine($"Кількість заявок: {lastYearApps.Count}");
        
        if (lastYearApps.Any())
        {
            foreach (var app in lastYearApps)
            {
                Console.WriteLine(app.ToString());
            }
        }
    }

    // 4. Знайти та відобразити інформацію про найдовше усунення несправності цього року
    public void DisplayLongestRepairThisYear()
    {
        int currentYear = DateTime.Now.Year; // 2025
        
        var currentYearApps = Applications
            .Where(a => a.BreakDownDateTime.Year == currentYear && a.RepairDurationDays > 0)
            .ToList();

        Console.WriteLine($"\n--- 4. Найдовше усунення несправності цього року ({currentYear}) ---");
        
        if (currentYearApps.Any())
        {
            // Сортуємо та беремо першу (найдовшу)
            var longestRepair = currentYearApps
                .OrderByDescending(a => a.RepairDurationDays)
                .FirstOrDefault();

            if (longestRepair != null)
            {
                Console.WriteLine($"Тривалість: {longestRepair.RepairDurationDays:F2} днів");
                Console.WriteLine(longestRepair.ToString());
            }
        }
        else
        {
            Console.WriteLine("Заявок на ремонт цього року не знайдено.");
        }
    }

    // 5. Вивести інформацію про всі поломки за останній рік (365 днів)
    public void DisplayLastYearBreakdowns()
    {
        DateTime oneYearAgo = DateTime.Now.AddYears(-1); // 02.11.2024
        
        var lastYearApps = Applications
            .Where(a => a.BreakDownDateTime >= oneYearAgo)
            .ToList();

        Console.WriteLine($"\n--- 5. Поломки за останній рік (з {oneYearAgo:dd.MM.yyyy}) ---");
        if (lastYearApps.Any())
        {
            foreach (var app in lastYearApps)
            {
                Console.WriteLine(app.ToString());
            }
        }
        else
        {
            Console.WriteLine("Заявок за останній рік не знайдено.");
        }
    }
}

// Точка входу для консольного тестування
public class Program
{
    public static void Main(string[] args)
    {
        // Встановлюємо культуру для коректного виводу назв місяців
        CultureInfo.CurrentCulture = new CultureInfo("uk-UA");
        
        var processor = new PhoneNetworkProcessor();
        
        processor.DisplayAllWithDuration();
        Console.WriteLine(new string('-', 50));
        
        processor.DisplayLastMonthBreakdowns();
        Console.WriteLine(new string('-', 50));

        processor.DisplayLastYearApplications();
        Console.WriteLine(new string('-', 50));

        processor.DisplayLongestRepairThisYear();
        Console.WriteLine(new string('-', 50));

        processor.DisplayLastYearBreakdowns();
    }
}