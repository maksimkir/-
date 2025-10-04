import random

rows = 10  
cols = 12 
salary = [[random.randint(8000, 20000) for _ in range(cols)] for _ in range(rows)]

total_per_employee = [sum(emp) for emp in salary]

average_per_month = [round(sum(salary[row][col] for row in range(rows)) / rows, 2) for col in range(cols)]

print("Зарплати співробітників за місяцями:")
for i, emp in enumerate(salary, 1):
    print(f"Співробітник {i}: {emp}")

print("\nЗагальна зарплата кожного співробітника за рік:")
for i, total in enumerate(total_per_employee, 1):
    print(f"Співробітник {i}: {total} грн")

print("\nСередня зарплата по фірмі за кожний місяць:")
for i, avg in enumerate(average_per_month, 1):
    print(f"Місяць {i}: {avg} грн")