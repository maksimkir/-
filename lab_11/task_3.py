from typing import Iterator
import decimal


def float_range(start: float, stop: float, step: float) -> Iterator[float]:
    if step == 0:
        raise ValueError("Крок (step) не може дорівнювати 0")

    try:
        precision = abs(decimal.Decimal(str(step)).as_tuple().exponent)
        if precision == 0:  # Якщо крок - ціле число
            precision = 1
            # Додаємо невеликий запас точності
        precision = min(precision + 5, 15)
    except Exception:
        # Запасний варіант якщо крок дуже малий
        precision = 10

    current = start

    if step > 0:
        while current < stop:
            yield round(current, precision)
            current += step
    elif step < 0:
        while current > stop:
            yield round(current, precision)
            current += step


print("Приклад 1: Крок додатній")
print(list(float_range(1.0, 2.0, 0.3)))

print("Приклад 2: Крок від’ємний")
print(list(float_range(5.0, 3.0, -0.5)))

print("Приклад 3: Малий крок 0.1 (перші 3 елементи)")
print(list(float_range(0.0, 1.0, 0.1))[:3])

print("Приклад 4: Початок і кінець однакові")
print(list(float_range(0.0, 0.0, 1.0)))

print("Приклад 5: Крок від’ємний, але start < stop")
print(list(float_range(1.0, 2.0, -1.0)))

print("Приклад 6: Точність (приклад з 0.1)")
print(list(float_range(0.0, 0.4, 0.1)))

try:
    list(float_range(1.0, 5.0, 0.0))
except ValueError as e:
    print(f"Приклад 7: Успішно перехоплено помилку: {e}")