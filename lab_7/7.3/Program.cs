using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

// Клас  "Телефонна мережа"
public class RepairApplication
{
    public string FullName { get; set; }      
    public string PhoneNumber { get; set; } 
    public string BreakDownDateStr { get; set; }
    public string BreakDownTimeStr { get; set; }
    public string RepairDateStr { get; set; }   
    public string RepairTimeStr { get; set; }  
    public DateTime BreakDownDateTime => ParseDateTime(BreakDownDateStr, BreakDownTimeStr);

    public DateTime RepairDateTime => ParseDateTime(RepairDateStr, RepairTimeStr);

    public double RepairDurationDays
    {
        get
        {
            if (RepairDateTime <= BreakDownDateTime) return 0;
            TimeSpan duration = RepairDateTime - BreakDownDateTime;
            return duration.TotalDays;
        }
    }

    // безпечного перетворення рядків ДДММРРРР та ГГ:ХХ:СС у DateTime
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
        return DateTime.MinValue; 
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

            new RepairApplication { FullName = "Іваненко І.І.", PhoneNumber = "101",
                                    BreakDownDateStr = "15102025", BreakDownTimeStr = "10:00:00",
                                    RepairDateStr = "17102025", RepairTimeStr = "12:00:00" },

            new RepairApplication { FullName = "Петренко П.П.", PhoneNumber = "102",
                                    BreakDownDateStr = "01092025", BreakDownTimeStr = "09:00:00",
                                    RepairDateStr = "05092025", RepairTimeStr = "15:00:00" },

            new RepairApplication { FullName = "Сидоренко С.С.", PhoneNumber = "103",
                                    BreakDownDateStr = "20122024", BreakDownTimeStr = "14:00:00",
                                    RepairDateStr = "21122024", RepairTimeStr = "16:00:00" },

            new RepairApplication { FullName = "Коваленко К.К.", PhoneNumber = "104",
                                    BreakDownDateStr = "28102025", BreakDownTimeStr = "08:00:00",
                                    RepairDateStr = "28102025", RepairTimeStr = "18:00:00" },

            new RepairApplication { FullName = "Мороз М.М.", PhoneNumber = "105",
                                    BreakDownDateStr = "01012023", BreakDownTimeStr = "09:00:00",
                                    RepairDateStr = "02012023", RepairTimeStr = "09:00:00" }
        };
    }
 
 
    public void DisplayAllWithDuration()
    {
        Console.WriteLine("\n--- 1. Відомості щодо всіх заявок (з терміном усунення) ---");
        foreach (var app in Applications)
        {
            Console.WriteLine($"{app.ToString()}, Термін усунення: {app.RepairDurationDays:F2} днів");
        }
    }

    public void DisplayLastMonthBreakdowns()
    {
        DateTime now = DateTime.Now;
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

    public void DisplayLongestRepairThisYear()
    {
        int currentYear = DateTime.Now.Year; 
        
        var currentYearApps = Applications
            .Where(a => a.BreakDownDateTime.Year == currentYear && a.RepairDurationDays > 0)
            .ToList();

        Console.WriteLine($"\n--- 4. Найдовше усунення несправності цього року ({currentYear}) ---");
        
        if (currentYearApps.Any())
        {

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

    public void DisplayLastYearBreakdowns()
    {
        DateTime oneYearAgo = DateTime.Now.AddYears(-1); 
        
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

public class Program
{
    public static void Main(string[] args)
    {
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