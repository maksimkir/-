using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

// ==============================================================================
// 1. КЛАС СТРУКТУРИ ДАНИХ
// ==============================================================================
public class RepairApplication
{
    // Поля для введення даних
    public string FullName { get; set; }        // ПІБ абонента
    public string PhoneNumber { get; set; }     // Номер телефону
    
    // Вхідні рядкові поля
    public string BreakDownDateStr { get; set; } 
    public string BreakDownTimeStr { get; set; } 
    public string RepairDateStr { get; set; }    
    public string RepairTimeStr { get; set; }    

    // Властивості DateTime для обчислень (Тільки для читання)
    [Browsable(false)] // Приховуємо в DataGridView
    public DateTime BreakDownDateTime => ParseDateTime(BreakDownDateStr, BreakDownTimeStr);
    [Browsable(false)]
    public DateTime RepairDateTime => ParseDateTime(RepairDateStr, RepairTimeStr);

    // Метод для обчислення терміну усунення в днях
    public double RepairDurationDays
    {
        get
        {
            if (RepairDateTime <= BreakDownDateTime || RepairDateTime == DateTime.MinValue) return 0.0;
            TimeSpan duration = RepairDateTime - BreakDownDateTime;
            return duration.TotalDays;
        }
    }
    
    // Властивість для відображення терміну усунення у DataGridView
    [DisplayName("Термін (днів)")]
    public string DurationDisplay => $"{RepairDurationDays:F2}";

    // Приватний метод для безпечного перетворення рядків ДДММРРРР та ГГ:ХХ:СС у DateTime
    private DateTime ParseDateTime(string dateStr, string timeStr)
    {
        if (string.IsNullOrEmpty(dateStr) || string.IsNullOrEmpty(timeStr))
        {
            return DateTime.MinValue;
        }

        string combinedString = $"{dateStr}{timeStr}";
        string format = "ddMMyyyyHH:mm:ss";

        if (DateTime.TryParseExact(combinedString, format, CultureInfo.InvariantCulture,
                                  DateTimeStyles.None, out DateTime result))
        {
            return result;
        }
        
        // В GUI ми не пишемо в Console, але можна вивести Debug.WriteLine або MessageBox
        return DateTime.MinValue;
    }
}

// ==============================================================================
// 2. КЛАС ЛОГІКИ ОБРОБКИ ДАНИХ
// ==============================================================================
public class PhoneNetworkProcessor
{
    // Робочий список, який буде оновлюватися через GUI
    public List<RepairApplication> Applications { get; set; }

    public PhoneNetworkProcessor()
    {
        // Приклад набору даних для тестування (можна видалити для чистого старту)
        Applications = new List<RepairApplication>
        {
            // Поломка минулого місяця (припустимо, зараз Листопад 2025)
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
                                    RepairDateStr = "21122024", RepairTimeStr = "16:00:00" }
        };
    }
    
    // 1. Повертає всі заявки з розрахованим терміном
    public List<RepairApplication> GetAllApplicationsWithDuration()
    {
        return Applications.ToList(); // Просто повертаємо список, властивість DurationDisplay зробить решту
    }

    // 2. Повертає поломки за минулий місяць
    public List<RepairApplication> GetLastMonthBreakdowns()
    {
        DateTime now = DateTime.Now;
        DateTime firstDayOfThisMonth = new DateTime(now.Year, now.Month, 1);
        DateTime firstDayOfLastMonth = firstDayOfThisMonth.AddMonths(-1);

        return Applications
            .Where(a => a.BreakDownDateTime >= firstDayOfLastMonth && a.BreakDownDateTime < firstDayOfThisMonth)
            .ToList();
    }

    // 3. Повертає заявки минулого року для підрахунку
    public List<RepairApplication> GetLastYearApplications()
    {
        int lastYear = DateTime.Now.Year - 1;
        
        return Applications
            .Where(a => a.BreakDownDateTime.Year == lastYear)
            .ToList();
    }

    // 4. Знаходить найдовше усунення несправності цього року
    public RepairApplication GetLongestRepairThisYear()
    {
        int currentYear = DateTime.Now.Year;
        
        var currentYearApps = Applications
            .Where(a => a.BreakDownDateTime.Year == currentYear && a.RepairDurationDays > 0)
            .ToList();
        
        return currentYearApps
            .OrderByDescending(a => a.RepairDurationDays)
            .FirstOrDefault();
    }

    // 5. Повертає всі поломки за останній рік (365 днів)
    public List<RepairApplication> GetLastYearBreakdowns()
    {
        DateTime oneYearAgo = DateTime.Now.AddYears(-1);
        
        return Applications
            .Where(a => a.BreakDownDateTime >= oneYearAgo)
            .ToList();
    }
}

// ==============================================================================
// 3. КЛАС ФОРМИ (GUI)
// ==============================================================================
public partial class Form1 : Form
{
    private PhoneNetworkProcessor processor = new PhoneNetworkProcessor();

    public Form1()
    {
        // Необхідний виклик для ініціалізації елементів форми (створюється дизайнером)
        InitializeComponent(); 
        
        // Налаштування DataGridView та початкове завантаження даних
        SetupDataGridView();
        LoadInitialData();
    }
    
    // Функція-заглушка для InitializeComponent (потрібна для компіляції)
    private void InitializeComponent()
    {
        // ... тут має бути код, згенерований дизайнером форми ...
        // Для демонстрації GUI-логіки припускаємо, що елементи існують
    }

    // Допоміжна функція для налаштування DataGridView
    private void SetupDataGridView()
    {
        dgvApplications.AutoGenerateColumns = true;
        dgvApplications.ReadOnly = true;
        dgvApplications.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
    }
    
    // Відображення даних у DataGridView
    private void DisplayData(List<RepairApplication> data)
    {
        dgvApplications.DataSource = null;
        dgvApplications.DataSource = data;
        txtOutput.Text = $"Знайдено записів: {data.Count}";
    }

    // Початкове завантаження всіх даних
    private void LoadInitialData()
    {
        DisplayData(processor.Applications);
    }
    
    // ==============================================================================
    // ОБРОБНИКИ ПОДІЙ ВВЕДЕННЯ ДАНИХ
    // ==============================================================================
    
    private void btnAddApplication_Click(object sender, EventArgs e)
    {
        // 1. Збір даних з полів
        var newApp = new RepairApplication
        {
            FullName = txtFullName.Text,
            PhoneNumber = txtPhone.Text,
            BreakDownDateStr = txtBreakDate.Text,
            BreakDownTimeStr = txtBreakTime.Text,
            RepairDateStr = txtRepairDate.Text,
            RepairTimeStr = txtRepairTime.Text
        };

        // 2. Валідація: Перевірка коректності парсингу дат/часу
        if (newApp.BreakDownDateTime == DateTime.MinValue || newApp.RepairDateTime == DateTime.MinValue)
        {
            MessageBox.Show("Будь ласка, перевірте коректність введених дат та часу (ДДММРРРР та ГГ:ХХ:СС).", "Помилка введення", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // 3. Додавання та оновлення інтерфейсу
        processor.Applications.Add(newApp);
        
        // Очищення полів введення
        txtFullName.Clear();
        txtPhone.Clear();
        txtBreakDate.Clear();
        txtBreakTime.Clear();
        txtRepairDate.Clear();
        txtRepairTime.Clear();
        
        // Відображення всього списку (включаючи нову заявку)
        DisplayData(processor.Applications);
        MessageBox.Show("Заявка успішно додана!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // ==============================================================================
    // ОБРОБНИКИ ПОДІЙ ФУНКЦІОНАЛЬНИХ ВИМОГ
    // ==============================================================================

    // 1. Вивести відомості щодо всіх заявок із зазначенням терміну усунення несправності у днях
    private void btnShowDuration_Click(object sender, EventArgs e)
    {
        DisplayData(processor.GetAllApplicationsWithDuration());
        txtOutput.Text = "Відображено всі заявки з терміном усунення.";
    }

    // 2. Вивести усі відомості про поломки за минулий місяць
    private void btnLastMonth_Click(object sender, EventArgs e)
    {
        var result = processor.GetLastMonthBreakdowns();
        DisplayData(result);
        
        DateTime firstDayOfLastMonth = DateTime.Now.AddMonths(-1);
        txtOutput.Text = $"Поломки за минулий місяць ({firstDayOfLastMonth:MMMM yyyy}): {result.Count} заявок.";
    }

    // 3. Підрахувати кількість заявок на ремонт минулого року та відобразити інформацію
    private void btnLastYearCount_Click(object sender, EventArgs e)
    {
        var result = processor.GetLastYearApplications();
        DisplayData(result);
        
        int lastYear = DateTime.Now.Year - 1;
        txtOutput.Text = $"Кількість заявок за {lastYear} рік: {result.Count}";
    }

    // 4. Знайти та відобразити інформацію про найдовше усунення несправності цього року
    private void btnLongestThisYear_Click(object sender, EventArgs e)
    {
        var longestRepair = processor.GetLongestRepairThisYear();
        
        if (longestRepair != null)
        {
            // Відображаємо лише одну знайдену заявку
            DisplayData(new List<RepairApplication> { longestRepair });
            txtOutput.Text = $"Найдовше усунення цього року: {longestRepair.DurationDisplay} днів.";
        }
        else
        {
            DisplayData(new List<RepairApplication>()); // Очищаємо DataGridView
            txtOutput.Text = "Найдовше усунення цього року не знайдено.";
        }
    }

    // 5. Вивести інформацію про всі поломки за останній рік (365 днів)
    private void btnLastYearAll_Click(object sender, EventArgs e)
    {
        var result = processor.GetLastYearBreakdowns();
        DisplayData(result);
        
        DateTime oneYearAgo = DateTime.Now.AddYears(-1);
        txtOutput.Text = $"Поломки за останній рік (з {oneYearAgo:dd.MM.yyyy}): {result.Count} заявок.";
    }
}