from array import *

def arraye():
    arr = array('i', [])
    size = int(input("Введіть розмір масиву: "))
    if size <= 0:
        print("Розмір має бути додатнім цілим числом.")
        return
    
    for _ in range(size):
        znach = int(input("Введіть елемент масиву: "))
        arr.append(znach)

    sum_unique(arr)

def sum_unique(arr):
    total = 0
    for val in arr:
        if arr.count(val) == 1: 
            total += val
    print("Сума унікальних елементів масиву:", total)

# запуск програми
arraye()

def main():
    arr = array('i', [1, 2, 3, 2, 4, 5, 1])
    total = 0
    for val in arr:
        if arr.count(val) == 1:
            total += val
    print("\nПриклад запуску з наперед визначеним масивом:", arr.tolist())
    print("Сума унікальних елементів масиву:", total)
    
main()