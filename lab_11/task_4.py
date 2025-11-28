from typing import Iterator

def walk_tree(data: dict) -> Iterator[str]:
    for key, value in data.items():
        yield key
        if isinstance(value, dict):
            yield from walk_tree(value)


tree1 = {"a": {"b": {"c": 1}}, "d": 2}
print(list(walk_tree(tree1)))

''' key = 'a', value = {"b": {"c": 1}}
- yield 'a'
-   value — словник → рекурсія:
-       key = 'b', value = {"c": 1}
-       yield 'b'
-           value — словник → рекурсія:
-           key = 'c', value = 1
-           yield 'c'
-           value — не словник → нічого більше
-   key = 'd', value = 2
-   yield 'd'
-   value — не словник → нічого більше
'''
# це обхід поянення