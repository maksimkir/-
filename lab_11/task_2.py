class MyIterator:
    def __init__(self, start):
        self.current = start

    def __iter__(self):
        return self

    def __next__(self):
        if self.current >= 0:
            result = self.current
            self.current -= 1
            return result
        else:
            raise StopIteration

my_obj = MyIterator(5)

for item in my_obj:
    print(item)