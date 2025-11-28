def filter_even_numbers(nums: list[int]) -> list[int]:
    result = []
    for num in nums:
        if num % 2 == 0:
            result.append(num)
    return result
print(filter_even_numbers([1, 2, 3, 4, 5, 6]))
def filter_even_numbers1(nums: list[int]) -> list[int]:
    return[num for num in nums if num % 2 == 0]
print(filter_even_numbers1([1, 2, 3, 4, 5, 6]))